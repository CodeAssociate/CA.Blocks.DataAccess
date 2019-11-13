using System;
using System.Data;
using System.Text;
using CA.Blocks.DataAccess;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests
{
    [TestFixture]
    public class DataRowExtensionsUnitTests
    {

        private DataTable CreateTestTable(Type dbType, object testData)
        {
            DataTable result = new DataTable();
            DataColumn dckey = new DataColumn("key", typeof(int));
            result.Columns.Add(dckey);
            DataColumn dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, null);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            return result;
        }
        

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 987.456)]
        public void GetValueFromRowAsDecimal_AsNullTests(int rowNumber, decimal? expected)
        {
            decimal? actual;
            var dt = CreateTestTable(typeof(decimal), expected);

            actual = dt.Rows[rowNumber].AsNullDecimal("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDecimal(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDecimal(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, 987.456)]
        public void GetValueFromRowAsDecimal(int rowNumber, decimal expected)
        {
            decimal? actual;
            var dt = CreateTestTable(typeof(decimal), expected);

            actual = dt.Rows[rowNumber].AsDecimal("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDecimal(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDecimal(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, (double)987.456)]
        public void GetValueFromRowAsDouble_AsNullTests(int rowNumber, double? expected)
        {
            double? actual;
            var dt = CreateTestTable(typeof(double), expected);

            actual = dt.Rows[rowNumber].AsNullDouble("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDouble(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDouble(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (double)987.456)]
        public void GetValueFromRowAsDouble(int rowNumber, double expected)
        {
            double? actual;
            var dt = CreateTestTable(typeof(double), expected);

            actual = dt.Rows[rowNumber].AsDouble("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDouble(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDouble(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat_AsNullTests(int rowNumber, float? expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(float), expected);

            actual = dt.Rows[rowNumber].AsNullFloat("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullFloat(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullFloat(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat(int rowNumber, float expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(float), expected);

            actual = dt.Rows[rowNumber].AsFloat("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsFloat(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsFloat(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "7009B509-098F-4AF4-97C8-CF354B4E0D76")]
        public void GetValueFromRowAsGuid_AsNullTests(int rowNumber, string testDate)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = Guid.Parse(testDate);
            }

            Guid? actual;
            var dt = CreateTestTable(typeof(Guid), expected);

            actual = dt.Rows[rowNumber].AsNullGuid("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullGuid(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullGuid(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, "new")]
        [TestCase(1, "empty")]
        [TestCase(1, "7009B509-098F-4AF4-97C8-CF354B4E0D77")]

        public void GetValueFromRowAsGuid(int rowNumber, string testDate)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                if (testDate == "new")
                {
                    expected = Guid.NewGuid();
                }
                else if (testDate == "empty")
                {
                    expected = Guid.Empty;
                }
                else
                {
                    expected = Guid.Parse(testDate);
                }
            }

            Guid actual;
            var dt = CreateTestTable(typeof(Guid), expected);

            actual = dt.Rows[rowNumber].AsGuid("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsGuid(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsGuid(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        


        [Test]
        [TestCase(0, null)]
        [TestCase(1, (long)123456789)]
        public void GetValueFromRowAsLong_AsNullTests(int rowNumber, long? expected)
        {
            long? actual;
            var dt = CreateTestTable(typeof(long), expected);

            actual = dt.Rows[rowNumber].AsNullLong("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullLong(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullLong(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (long)1234567890)]
        [TestCase(1, long.MinValue)]
        [TestCase(1, long.MaxValue)]
        public void GetValueFromRowAsLong(int rowNumber, long expected)
        {
            long actual;
            var dt = CreateTestTable(typeof(long), expected);

            actual = dt.Rows[rowNumber].AsLong("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, (int)1456789)]
        public void GetValueFromRowAsInt_AsNullTests(int rowNumber, int? expected)
        {
            int? actual;
            var dt = CreateTestTable(typeof(int), expected);

            actual = dt.Rows[rowNumber].AsNullInt("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullInt(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullInt(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, 1234567)]
        [TestCase(1, int.MinValue)]
        [TestCase(1, int.MaxValue)]
        public void GetValueFromRowAsInt(int rowNumber, int expected)
        {
            int? actual;
            var dt = CreateTestTable(typeof(int), expected);

            actual = dt.Rows[rowNumber].AsInt("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsInt(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsInt(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void GetValueFromRowAsBool_AsNullTests(int rowNumber, bool? expected)
        {
            bool? actual;
            var dt = CreateTestTable(typeof(bool), expected);

            actual = dt.Rows[rowNumber].AsNullBool("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullBool(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullBool(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void GetValueFromRowAsBool(int rowNumber, bool expected)
        {
            bool actual;
            var dt = CreateTestTable(typeof(bool), expected);

            actual = dt.Rows[rowNumber].AsBool("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBool(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBool(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, (byte)0)]
        [TestCase(1, (byte)128)]
        public void GetValueFromRowAsBtye_AsNullTests(int rowNumber, byte? expected)
        {
            byte? actual;
            var dt = CreateTestTable(typeof(byte), expected);

            actual = dt.Rows[rowNumber].AsNullByte("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullByte(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullByte(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (byte)0)]
        [TestCase(1, (byte)213)]
        public void GetValueFromRowAsByte(int rowNumber, byte expected)
        {
            byte actual;
            var dt = CreateTestTable(typeof(byte), expected);

            actual = dt.Rows[rowNumber].AsByte("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsByte(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsByte(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, 'a')]
        [TestCase(1, 'Z')]
        public void GetValueFromRowAsChar_AsNullTests(int rowNumber, char? expected)
        {
            char? actual;
            var dt = CreateTestTable(typeof(char), expected);

            actual = dt.Rows[rowNumber].AsNullChar("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullChar(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullChar(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, '*')]
        [TestCase(1, '0')]
        public void GetValueFromRowAsChar(int rowNumber, char expected)
        {
            char actual;
            var dt = CreateTestTable(typeof(char), expected);

            actual = dt.Rows[rowNumber].AsChar("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsChar(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsChar(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }

        // There is not null value for Binary it is simple a zero length array
        [Test]
        [TestCase(0, null)]
        [TestCase(1, "Test Data ")]
        [TestCase(1, "Test Date 2")]
        [TestCase(1, "")]
        public void GetValueFromRowAsBinary(int rowNumber, string testValue)
        {
            byte[] expected = null;
            if (testValue != null)
            {
                expected = Encoding.ASCII.GetBytes(testValue);
            }
            byte[] actual;
            var dt = CreateTestTable(typeof(byte[]), expected);

            actual = dt.Rows[rowNumber].AsBinary("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBinary(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBinary(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "1-Jan-2019")]
        public void GetValueFromRowAsDateTime_AsNullTests(int rowNumber, string testDate)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = DateTime.Parse(testDate);
            }

            DateTime? actual;
            var dt = CreateTestTable(typeof(DateTime), expected);

            actual = dt.Rows[rowNumber].AsNullDateTime("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDateTime(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullDateTime(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, "now")]
        [TestCase(1, "2-Jan-2019")]
        [TestCase(1, "2-Jan-2019 12:39:22")]
        [TestCase(1, "2-Jan-2019 23:59:59")]
        [TestCase(1, "2-Jan-2019 00:00:01.333")]
        public void GetValueFromRowAsDateTime(int rowNumber, string testDate)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                if (testDate == "now")
                {
                    expected = DateTime.Now;
                }
                else
                {
                    expected = DateTime.Parse(testDate);
                }
            }

            DateTime actual;
            var dt = CreateTestTable(typeof(DateTime), expected);

            actual = dt.Rows[rowNumber].AsDateTime("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDateTime(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDateTime(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



    }
}