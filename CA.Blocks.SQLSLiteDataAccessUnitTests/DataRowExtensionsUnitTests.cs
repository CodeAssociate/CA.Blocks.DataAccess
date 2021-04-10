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
        /// <summary>
        /// Create a test table with the first row as null and the second row as the testData value for type dbType
        /// </summary>
        private DataTable CreateTestTable(Type dbType, object testData)
        {
            DataTable result = new DataTable();
            DataColumn dcKey = new DataColumn("key", typeof(int));
            result.Columns.Add(dcKey);
            DataColumn dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, null);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            return result;
        }
        
        private void AssertNullable<T>(Nullable<T> expected, Nullable<T> actual)  where T : struct 
        {
            if (expected.HasValue)
            {
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual.Value);
            }
            else
            {
                Assert.IsFalse(actual.HasValue);
            }
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

            var dt = CreateTestTable(typeof(byte[]), expected);

            var actual = dt.Rows[rowNumber].AsBinary("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBinary(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBinary(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void GetValueFromRowAsBool(int rowNumber, bool expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);

            var actual = dt.Rows[rowNumber].AsBool("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBool(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsBool(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void GetValueFromRowAsBool_AsNullTests(int rowNumber, bool? expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);

            var actual = dt.Rows[rowNumber].AsNullBool("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullBool(1);
            AssertNullable(expected, actual);
            
            actual = dt.Rows[rowNumber].AsNullBool(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(1, (byte)0)]
        [TestCase(1, (byte)213)]
        public void GetValueFromRowAsByte(int rowNumber, byte expected)
        {
            var dt = CreateTestTable(typeof(byte), expected);

            var actual = dt.Rows[rowNumber].AsByte("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsByte(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsByte(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, (byte)0)]
        [TestCase(1, (byte)128)]
        public void GetValueFromRowAsByte_AsNullTests(int rowNumber, byte? expected)
        {
            var dt = CreateTestTable(typeof(byte), expected);

            var actual = dt.Rows[rowNumber].AsNullByte("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullByte(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullByte(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, '*')]
        [TestCase(1, '0')]
        public void GetValueFromRowAsChar(int rowNumber, char expected)
        {
            var dt = CreateTestTable(typeof(char), expected);

            var actual = dt.Rows[rowNumber].AsChar("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsChar(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsChar(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, 'a')]
        [TestCase(1, 'Z')]
        public void GetValueFromRowAsChar_AsNullTests(int rowNumber, char? expected)
        {
            var dt = CreateTestTable(typeof(char), expected);

            var actual = dt.Rows[rowNumber].AsNullChar("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullChar(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullChar(dt.Columns["col"]);
            AssertNullable(expected, actual);
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

            var dt = CreateTestTable(typeof(DateTime), expected);

            var actual = dt.Rows[rowNumber].AsDateTime("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDateTime(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDateTime(dt.Columns["col"]);
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

            var dt = CreateTestTable(typeof(DateTime), expected);

            var actual = dt.Rows[rowNumber].AsNullDateTime("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullDateTime(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullDateTime(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(1, 987.456)]
        public void GetValueFromRowAsDecimal(int rowNumber, decimal expected)
        {
            var dt = CreateTestTable(typeof(decimal), expected);

            decimal? actual = dt.Rows[rowNumber].AsDecimal("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDecimal(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDecimal(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, 987.456)]
        public void GetValueFromRowAsDecimal_AsNullTests(int rowNumber, decimal? expected)
        {
            var dt = CreateTestTable(typeof(decimal), expected);

            var actual = dt.Rows[rowNumber].AsNullDecimal("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullDecimal(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullDecimal(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, (double)987.456)]
        public void GetValueFromRowAsDouble(int rowNumber, double expected)
        {
            var dt = CreateTestTable(typeof(double), expected);

            double? actual = dt.Rows[rowNumber].AsDouble("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDouble(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsDouble(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, 987.456)]
        public void GetValueFromRowAsDouble_AsNullTests(int rowNumber, double? expected)
        {
            var dt = CreateTestTable(typeof(double), expected);

            var actual = dt.Rows[rowNumber].AsNullDouble("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullDouble(1);
            AssertNullable(expected, actual);
            
            actual = dt.Rows[rowNumber].AsNullDouble(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat(int rowNumber, float expected)
        {
            var dt = CreateTestTable(typeof(float), expected);

            float? actual = dt.Rows[rowNumber].AsFloat("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsFloat(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsFloat(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat_AsNullTests(int rowNumber, float? expected)
        {
            var dt = CreateTestTable(typeof(float), expected);

            var actual = dt.Rows[rowNumber].AsNullFloat("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullFloat(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullFloat(dt.Columns["col"]);
            AssertNullable(expected, actual);
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

            var dt = CreateTestTable(typeof(Guid), expected);

            var actual = dt.Rows[rowNumber].AsGuid("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsGuid(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsGuid(dt.Columns["col"]);
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

            var dt = CreateTestTable(typeof(Guid), expected);

            var actual = dt.Rows[rowNumber].AsNullGuid("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullGuid(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullGuid(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(1, 1234567)]
        [TestCase(1, int.MinValue)]
        [TestCase(1, int.MaxValue)]
        public void GetValueFromRowAsInt(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(int), expected);

            int? actual = dt.Rows[rowNumber].AsInt("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsInt(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsInt(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, 1456789)]
        public void GetValueFromRowAsInt_AsNullTests(int rowNumber, int? expected)
        {
            var dt = CreateTestTable(typeof(int), expected);

            var actual = dt.Rows[rowNumber].AsNullInt("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullInt(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullInt(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(1, (long)1234567890)]
        [TestCase(1, long.MinValue)]
        [TestCase(1, long.MaxValue)]
        public void GetValueFromRowAsLong(int rowNumber, long expected)
        {
            var dt = CreateTestTable(typeof(long), expected);

            var actual = dt.Rows[rowNumber].AsLong("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, (long)123456789)]
        public void GetValueFromRowAsLong_AsNullTests(int rowNumber, long? expected)
        {
            var dt = CreateTestTable(typeof(long), expected);

            var actual = dt.Rows[rowNumber].AsNullLong("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullLong(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullLong(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        



        [Test]
        [TestCase(1, (sbyte)122)]
        [TestCase(1, sbyte.MinValue)]
        [TestCase(1, sbyte.MaxValue)]
        public void GetValueFromRowAsSbyte(int rowNumber, sbyte expected)
        {
            var dt = CreateTestTable(typeof(sbyte), expected);

            var actual = dt.Rows[rowNumber].AsSbyte("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsSbyte(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsSbyte(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, (sbyte)-12)]
        [TestCase(1, (sbyte)13)]
        public void GetValueFromRowAsSbyte_AsNullTests(int rowNumber, sbyte? expected)
        {
            var dt = CreateTestTable(typeof(sbyte), expected);

            var actual = dt.Rows[rowNumber].AsNullSbyte("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullSbyte(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullSbyte(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, (short)1442)]
        [TestCase(1, short.MinValue)]
        [TestCase(1, short.MaxValue)]
        public void GetValueFromRowAsShort(int rowNumber, short expected)
        {
            var dt = CreateTestTable(typeof(short), expected);

            var actual = dt.Rows[rowNumber].AsShort("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsShort(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsShort(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, (short)-12)]
        [TestCase(1, (short)13)]
        public void GetValueFromRowAsShort_AsNullTests(int rowNumber, short? expected)
        {
            var dt = CreateTestTable(typeof(short), expected);

            var actual = dt.Rows[rowNumber].AsNullShort("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullShort(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullShort(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, (float)1442.34)]
        [TestCase(1, float.MinValue)]
        [TestCase(1, float.MaxValue)]
        public void GetValueFromRowAsSingle(int rowNumber, float expected)
        {
            var dt = CreateTestTable(typeof(float), expected);

            var actual = dt.Rows[rowNumber].AsSingle("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsSingle(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsSingle(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (float)-12.23)]
        [TestCase(1,(float)13.56)]
        public void GetValueFromRowAsShort_AsNullTests(int rowNumber, float? expected)
        {
            var dt = CreateTestTable(typeof(float), expected);

            var actual = dt.Rows[rowNumber].AsNullSingle("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullSingle(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullSingle(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(1, "12345")]
        [TestCase(1, "734^%$%$^$^%")]
        public void GetValueFromRowAsString(int rowNumber, string expected)
        {
            var dt = CreateTestTable(typeof(string), expected);

            var actual = dt.Rows[rowNumber].AsString("col");
            if (expected != null)
                Assert.AreEqual(expected, actual);
            else
                Assert.IsTrue(actual == null);

            actual = dt.Rows[rowNumber].AsString(1);
            if (expected != null)
                Assert.AreEqual(expected, actual);
            else
                Assert.IsTrue(actual == null);

            actual = dt.Rows[rowNumber].AsString(dt.Columns["col"]);
            if (expected != null)
                Assert.AreEqual(expected, actual);
            else
                Assert.IsTrue(actual == null);
        }

        [Test]
        [TestCase(1, "now")]
        [TestCase(1, "00:00:00")]
        [TestCase(1, "23:44:34.333")]
        public void GetValueFromRowAsTimeSpan(int rowNumber, string testDate)
        {
            TimeSpan? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                if (testDate == "now")
                {
                    expected = DateTime.Now.TimeOfDay;
                }
                else
                {
                    expected = TimeSpan.Parse(testDate);
                }
            }

            var dt = CreateTestTable(typeof(TimeSpan), expected);

            var actual = dt.Rows[rowNumber].AsTimeSpan("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsTimeSpan(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsTimeSpan(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, "22:21:20")]
        public void GetValueFromRowAsTimeSpan_AsNullTests(int rowNumber, string testDate)
        {
            TimeSpan? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = TimeSpan.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(TimeSpan), expected);

            var actual = dt.Rows[rowNumber].AsNullTimeSpan("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullTimeSpan(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullTimeSpan(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        [Test]
        [TestCase(1, (uint)1234567890)]
        [TestCase(1, uint.MinValue)]
        [TestCase(1, uint.MaxValue)]
        public void GetValueFromRowAsUInt(int rowNumber, uint expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);

            var actual = dt.Rows[rowNumber].AsUInt("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsUInt(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsUInt(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, (uint)123456789)]
        [TestCase(1, uint.MinValue)]
        [TestCase(1, uint.MaxValue)]
        public void GetValueFromRowAsUInt_AsNullTests(int rowNumber, uint? expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);

            var actual = dt.Rows[rowNumber].AsNullUInt("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullUInt(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullUInt(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }

        [Test]
        [TestCase(1, (ulong)1234567890)]
        [TestCase(1, ulong.MinValue)]
        [TestCase(1, ulong.MaxValue)]
        public void GetValueFromRowAsULong(int rowNumber, ulong expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);

            var actual = dt.Rows[rowNumber].AsULong("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsULong(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsULong(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, (ulong)123456789)]
        [TestCase(1, ulong.MinValue)]
        [TestCase(1, ulong.MaxValue)]
        public void GetValueFromRowAsULong_AsNullTests(int rowNumber, ulong? expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);

            var actual = dt.Rows[rowNumber].AsNullULong("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullULong(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullULong(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        
        
        [Test]
        [TestCase(1, (ushort)12345)]
        [TestCase(1, ushort.MinValue)]
        [TestCase(1, ushort.MaxValue)]
        public void GetValueFromRowAsUShort(int rowNumber, ushort expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);

            var actual = dt.Rows[rowNumber].AsUShort("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsUShort(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsUShort(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, (ushort)12345)]
        [TestCase(1, ushort.MinValue)]
        [TestCase(1, ushort.MaxValue)]
        public void GetValueFromRowAsULong_AsNullTests(int rowNumber, ushort? expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);

            var actual = dt.Rows[rowNumber].AsNullUShort("col");
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullUShort(1);
            AssertNullable(expected, actual);

            actual = dt.Rows[rowNumber].AsNullUShort(dt.Columns["col"]);
            AssertNullable(expected, actual);
        }
        // this logic is not part of this  class it should be a string extension
        /*
        [Test]
        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(1, "1,2,3,4", ',')]
        [TestCase(1, "1|2|5", '|')]
        public void GetValueFromRowAsShortList(int rowNumber, string testDate, char delimiter)
        {
            List<short> expected = new List<short>();
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                var sarray = testDate.Split(delimiter);
                expected.AddRange(sarray.Select(short.Parse));
            }

            IList<short> actual;
            var dt = CreateTestTable(typeof(string), testDate);

            actual = dt.Rows[rowNumber].AsShortList("col", delimiter);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(1, "1,2,3,4", ',')]
        [TestCase(1, "1|2|5", '|')]
        public void GetValueFromRowAsIntList(int rowNumber, string testDate, char delimiter)
        {
            List<int> expected = new List<int>();
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                var sarray = testDate.Split(delimiter);
                expected.AddRange(sarray.Select(int.Parse));
            }

            IList<int> actual;
            var dt = CreateTestTable(typeof(string), testDate);

            actual = dt.Rows[rowNumber].AsIntList("col", delimiter);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(1, "1,2,3,4", ',')]
        [TestCase(1, "1|2|5", '|')]
        public void GetValueFromRowAsLongList(int rowNumber, string testDate, char delimiter)
        {
            List<long> expected = new List<long>();
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                var sarray = testDate.Split(delimiter);
                expected.AddRange(sarray.Select(long.Parse));
            }

            IList<long> actual;
            var dt = CreateTestTable(typeof(string), testDate);

            actual = dt.Rows[rowNumber].AsLongList("col", delimiter);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(1, "1,2,3,4", ',')]
        [TestCase(1, "1|2|5", '|')]
        public void GetValueFromRowAsStringList(int rowNumber, string testDate, char delimiter)
        {
            List<string> expected = new List<string>();
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                var sarray = testDate.Split(delimiter);
                expected.AddRange(sarray.ToList());
            }

            IList<string> actual;
            var dt = CreateTestTable(typeof(string), testDate);

            actual = dt.Rows[rowNumber].AsStringList("col", delimiter);
            Assert.AreEqual(expected, actual);
        }

    */

    }
}