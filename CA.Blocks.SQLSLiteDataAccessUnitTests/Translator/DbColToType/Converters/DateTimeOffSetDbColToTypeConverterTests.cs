using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class DateTimeOffSetDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, "now")]
        [TestCase(1, "2-Jan-2019")]
        [TestCase(1, "2-Jan-2019 12:39:22")]
        [TestCase(1, "2-Jan-2019 23:59:59")]
        [TestCase(1, "2-Jan-2019 00:00:01.333")]
        public void DbColToTypeConverterTest(int rowNumber, string testDate)
        {
            DateTimeOffset? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = testDate == "now" ? DateTimeOffset.Now : DateTimeOffset.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(DateTimeOffset), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new DateTimeOffSetDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "1-Jan-2019")]
        public void NullDbColToTypeConverterTest(int rowNumber, string testDate)
        {
            DateTimeOffset? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = DateTimeOffset.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(DateTimeOffset), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullDateTimeOffSetDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}