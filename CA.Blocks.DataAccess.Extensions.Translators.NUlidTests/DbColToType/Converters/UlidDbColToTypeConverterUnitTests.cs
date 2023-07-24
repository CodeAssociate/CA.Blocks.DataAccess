using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests.DbColToType.Converters
{
    [TestFixture]
    public class UlidDbColToTypeConverterUnitTests : BaseDbColToTypeConverterTests
    {


        [Test]
        public void DbColToTypeConverterTest_ValidUlid()
        {
            var UlidAsString = "01H3V724QTMH8TV1BHPE6Z5AV4";
            var expected = new Ulid(UlidAsString);
            var dt = CreateTestTable(typeof(string), UlidAsString);
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new UlidDbColToTypeConverter();


            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        public void DbColToTypeConverterTest_WithBinaryData()
        {
            var UlidAsString = "01H3V724QTMH8TV1BHPE6Z5AV4";
            var expected = new Ulid(UlidAsString);
            var dt = CreateTestTable(typeof(byte[]), expected.ToByteArray());
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new UlidDbColToTypeConverter();


            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }



    }
}
