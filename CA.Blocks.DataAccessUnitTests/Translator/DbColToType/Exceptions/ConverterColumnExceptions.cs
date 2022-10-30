using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Exceptions
{
    [TestFixture]
    public class ConverterColumnExceptions : BaseDbColToTypeConverterTests
    {
        [Test]
        public void DbColToTypeConverterException_BadDataException()
        {
            var dt = CreateTestTable(typeof(string), "NotANumber");
            var dataRow = GetDataRow(1, dt);
            var target = new IntDbColToTypeConverter();

            var ex = Assert.Throws<ConverterColumnBadDataException>(() =>
            {
               
                _ = target.GetData(dataRow, 1);
            });



        }


        [Test]
        public void DbColToTypeConverterException_ColumnNotFoundExceptionName()
        {
            var dt = CreateTestTable(typeof(string), "GoodValue");
            var dataRow = GetDataRow(1, dt);
            var target = new StringDbColToTypeConverter();

            var ex = Assert.Throws<ConverterColumnNotFoundException>(() =>
            {

                _ = target.GetData(dataRow, "DoesNotExist");
            });
        }

        [Test]
        public void DbColToTypeConverterException_ColumnNotFoundExceptionName_Reader()
        {
            var dr = CreateTestTable(typeof(string), "GoodValue").CreateDataReader();
            var target = new StringDbColToTypeConverter();

            var ex = Assert.Throws<ConverterColumnNotFoundException>(() =>
            {

                _ = target.GetData(dr, "DoesNotExist");
            });

        }


        [Test]
        public void DbColToTypeConverterException_ColumnNotFoundExceptionIndex()
        {
            var dt = CreateTestTable(typeof(string), "GoodValue");
            var dataRow = GetDataRow(1, dt);
            var target = new StringDbColToTypeConverter();

            var ex = Assert.Throws<ConverterColumnNotFoundException>(() =>
            {

                _ = target.GetData(dataRow, 5);
            });

        }

    }
}

