using System.Data;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests.DbColToType.Converters;

public class CustomerWithUlid
{
    public int Id { get; set; }
    public Ulid CustomerUlid { get; set; }
    public Ulid? OptionalUlid { get; set; }
}

public class UlidDbRowTranslatorTests
{
    [Fact]
    public void DefaultDbColToTypeProvider_ResolvesUlidConverters()
    {
        var converter = DefaultDbColToTypeProvider.DefaultInstance.Resolve<Ulid>();
        Assert.NotNull(converter);
        Assert.IsType<UlidDbColToTypeConverter>(converter);

        var nullConverter = DefaultDbColToTypeProvider.DefaultInstance.Resolve<Ulid?>();
        Assert.NotNull(nullConverter);
        Assert.IsType<NullUlidDbColToTypeConverter>(nullConverter);

        var typeofConverter = DefaultDbColToTypeProvider.DefaultInstance.Resolve(typeof(Ulid));
        Assert.NotNull(typeofConverter);
        Assert.IsType<UlidDbColToTypeConverter>(typeofConverter);
    }

    [Fact]
    public void DefaultDbRowTranslatorProvider_TranslatesModelWithUlid_FromDataReader()
    {
        var ulid1 = Ulid.NewUlid();
        var ulid2 = Ulid.NewUlid();

        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("CustomerUlid", typeof(string));
        dt.Columns.Add("OptionalUlid", typeof(string));

        dt.Rows.Add(1, ulid1.ToString(), ulid2.ToString());
        dt.Rows.Add(2, ulid2.ToString(), DBNull.Value);

        var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CustomerWithUlid>();
        Assert.NotNull(translator);

        using var reader = dt.CreateDataReader();
        var list = new List<CustomerWithUlid>();
        while (reader.Read())
        {
            list.Add(translator.Translate(reader));
        }

        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0].Id);
        Assert.Equal(ulid1, list[0].CustomerUlid);
        Assert.Equal(ulid2, list[0].OptionalUlid);

        Assert.Equal(2, list[1].Id);
        Assert.Equal(ulid2, list[1].CustomerUlid);
        Assert.Null(list[1].OptionalUlid);
    }

    [Fact]
    public void DefaultDbRowTranslatorProvider_TranslatesModelWithUlid_FromDataTable()
    {
        var ulid1 = Ulid.NewUlid();
        var ulid2 = Ulid.NewUlid();

        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("CustomerUlid", typeof(byte[]));
        dt.Columns.Add("OptionalUlid", typeof(byte[]));

        dt.Rows.Add(1, ulid1.ToByteArray(), ulid2.ToByteArray());
        dt.Rows.Add(2, ulid2.ToByteArray(), DBNull.Value);

        var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CustomerWithUlid>();
        Assert.NotNull(translator);

        var list = translator.Translate(dt);

        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0].Id);
        Assert.Equal(ulid1, list[0].CustomerUlid);
        Assert.Equal(ulid2, list[0].OptionalUlid);

        Assert.Equal(2, list[1].Id);
        Assert.Equal(ulid2, list[1].CustomerUlid);
        Assert.Null(list[1].OptionalUlid);
    }
}
