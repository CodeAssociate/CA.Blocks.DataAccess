using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using Xunit;

namespace CA.Blocks.DataAccess.Generators.Tests
{
    public enum SampleStatus
    {
        Inactive = 0,
        Active = 1,
        Pending = 2
    }

    [GenerateDbRowTranslator]
    public class SampleCustomer
    {
        public int Id { get; init; }
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalBalance { get; set; }
        public bool IsActive { get; set; }
        public Guid UserGuid { get; set; }
        public SampleStatus Status { get; set; }
        public int? OptionalCount { get; set; }
        public string? Description { get; set; }
    }

    [GenerateDbRowTranslator]
    public class SampleCustomMapped
    {
        [DbColToSourceName("customer_id")]
        public int Id { get; set; }

        [DbColToSourceName("full_name")]
        public string Name { get; set; } = string.Empty;

        [DbColToSourceName("tags")]
        [DbColToTypeConverter(typeof(IntListDbColToTypeConverter), ',')]
        public IList<int>? TagList { get; set; }
    }

    [GenerateDbRowTranslator(ThrowIfColumnNotFound = false)]
    public class SamplePartialDto
    {
        public int Id { get; set; }
        public string? OptionalExtra { get; set; }
    }

    public class GeneratedTranslatorRuntimeTests
    {
        [Fact]
        public void Translate_IDataReader_MapsAllProperties()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("created_date", typeof(DateTime));
            dt.Columns.Add("total_balance", typeof(decimal));
            dt.Columns.Add("is_active", typeof(bool));
            dt.Columns.Add("user_guid", typeof(Guid));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("optional_count", typeof(int));
            dt.Columns.Add("description", typeof(string));

            var expectedGuid = Guid.NewGuid();
            var expectedDate = new DateTime(2026, 1, 15, 10, 30, 0);

            dt.Rows.Add(42, "Alice Smith", expectedDate, 125.50m, true, expectedGuid, "Active", 10, "A loyal customer");

            using var reader = dt.CreateDataReader();
            Assert.True(reader.Read());

            var translator = SampleCustomerDbRowTranslator.Instance;
            var customer = translator.Translate(reader);

            Assert.Equal(42, customer.Id);
            Assert.Equal("Alice Smith", customer.Name);
            Assert.Equal(expectedDate, customer.CreatedDate);
            Assert.Equal(125.50m, customer.TotalBalance);
            Assert.True(customer.IsActive);
            Assert.Equal(expectedGuid, customer.UserGuid);
            Assert.Equal(SampleStatus.Active, customer.Status);
            Assert.Equal(10, customer.OptionalCount);
            Assert.Equal("A loyal customer", customer.Description);
        }

        [Fact]
        public void Translate_DataRow_And_DataTable_MapsSuccessfully()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("created_date", typeof(DateTime));
            dt.Columns.Add("total_balance", typeof(decimal));
            dt.Columns.Add("is_active", typeof(bool));
            dt.Columns.Add("user_guid", typeof(Guid));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("optional_count", typeof(int));
            dt.Columns.Add("description", typeof(string));

            var guid = Guid.NewGuid();
            var now = DateTime.UtcNow;

            dt.Rows.Add(1, "Bob", now, 50.0m, false, guid, "Pending", DBNull.Value, DBNull.Value);

            var translator = SampleCustomerDbRowTranslator.Instance;
            var customer = translator.Translate(dt.Rows[0]);

            Assert.Equal(1, customer.Id);
            Assert.Equal("Bob", customer.Name);
            Assert.False(customer.IsActive);
            Assert.Equal(SampleStatus.Pending, customer.Status);
            Assert.Null(customer.OptionalCount);
            Assert.Null(customer.Description);

            var list = translator.Translate(dt);
            Assert.Single(list);
            Assert.Equal("Bob", list[0].Name);
        }

        [Fact]
        public void Translate_CustomSourceNameAndConverter_Works()
        {
            var dt = new DataTable();
            dt.Columns.Add("customer_id", typeof(int));
            dt.Columns.Add("full_name", typeof(string));
            dt.Columns.Add("tags", typeof(string));

            dt.Rows.Add(100, "Charlie", "1,2,3");

            using var reader = dt.CreateDataReader();
            reader.Read();

            var translator = SampleCustomMappedDbRowTranslator.Instance;
            var result = translator.Translate(reader);

            Assert.Equal(100, result.Id);
            Assert.Equal("Charlie", result.Name);
            Assert.Equal(new[] { 1, 2, 3 }, result.TagList);
        }

        [Fact]
        public void MissingColumn_Throws_ConverterColumnNotFoundException_WhenStrict()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            // name column is missing!

            dt.Rows.Add(1);

            using var reader = dt.CreateDataReader();
            reader.Read();

            var translator = SampleCustomerDbRowTranslator.Instance;
            Assert.Throws<ConverterColumnNotFoundException>(() => translator.Translate(reader));
        }

        [Fact]
        public void MissingColumn_DoesNotThrow_WhenThrowIfColumnNotFoundIsFalse()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));

            dt.Rows.Add(99);

            using var reader = dt.CreateDataReader();
            reader.Read();

            var translator = SamplePartialDtoDbRowTranslator.Instance;
            var result = translator.Translate(reader);

            Assert.Equal(99, result.Id);
            Assert.Null(result.OptionalExtra);
        }

        [Fact]
        public void Translate_IDataReader_HandlesNullsCorrectly()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("created_date", typeof(DateTime));
            dt.Columns.Add("total_balance", typeof(decimal));
            dt.Columns.Add("is_active", typeof(bool));
            dt.Columns.Add("user_guid", typeof(Guid));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("optional_count", typeof(int));
            dt.Columns.Add("description", typeof(string));

            dt.Rows.Add(1, "Bob", DateTime.UtcNow, 10.0m, true, Guid.NewGuid(), "Active", DBNull.Value, DBNull.Value);

            using var reader = dt.CreateDataReader();
            Assert.True(reader.Read());

            var translator = SampleCustomerDbRowTranslator.Instance;
            var customer = translator.Translate(reader);

            Assert.Null(customer.OptionalCount);
            Assert.Null(customer.Description);
        }

        [Fact]
        public void DefaultDbRowTranslatorProvider_Resolves_AutoRegisteredTranslator()
        {
            var resolved = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SampleCustomer>();
            Assert.NotNull(resolved);
            Assert.IsType<SampleCustomerDbRowTranslator>(resolved);
        }
    }
}
