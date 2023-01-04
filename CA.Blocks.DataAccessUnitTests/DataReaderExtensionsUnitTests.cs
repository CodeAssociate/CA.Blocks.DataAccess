using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccessUnitTests.Base;

namespace CA.Blocks.DataAccessUnitTests
{
    [TestFixture]
    public class DataReaderExtensionsUnitTests
    {
        // we use a simple mock of reader using a data table
        private IDataReader CreateTestTable(Type dbType, object? testData)
        {
            DataTable result = new DataTable();
            DataColumn dckey = new DataColumn("key", typeof(int));
            result.Columns.Add(dckey);
            DataColumn dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, testData);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            
            return new MockDataReader(result);
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        public void GetValueFromRowAsBool(bool? expected)
        {
            bool? actual;
            var dr = CreateTestTable(typeof(bool), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsBool("col");
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsBool(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullBool("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullBool(1);
                Assert.That(actual, Is.Null);
            }
        }


        [Test]
        [TestCase(123)]
        [TestCase(byte.MinValue)]
        [TestCase(byte.MaxValue)]
        [TestCase(null)]
        public void GetValueFromRowAsByte(byte? expected)
        {
            byte? actual;
            var dr = CreateTestTable(typeof(byte), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsByte("col");
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsByte(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullByte("col");
                Assert.IsNull(actual);

                actual = dr.AsNullByte(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase(12345)]
        [TestCase(short.MinValue)]
        [TestCase(short.MaxValue)]
        [TestCase(null)]
        public void GetValueFromRowAsShort(short? expected)
        {
            short? actual;
            var dr = CreateTestTable(typeof(short), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsShort("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsShort(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullShort("col");
                Assert.IsNull(actual);

                actual = dr.AsNullShort(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase(1234567)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(null)]
        public void GetValueFromRowAsInt(int? expected)
        {
            int? actual;
            var dr = CreateTestTable(typeof(int), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsInt("col");
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsInt(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullInt("col");
                Assert.IsNull(actual);

                actual = dr.AsNullInt(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase(1234567)]
        [TestCase(long.MinValue)]
        [TestCase(long.MaxValue)]
        [TestCase(null)]
        public void GetValueFromRowAsLong(long? expected)
        {
            long? actual;
            var dr = CreateTestTable(typeof(long), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsLong("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsLong(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullLong("col");
                Assert.IsNull(actual);

                actual = dr.AsNullLong(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase("new")]
        [TestCase("empty")]
        [TestCase("7009B509-098F-4AF4-97C8-CF354B4E0D77")]
        [TestCase(null)]
        public void GetValueFromRowAsGuid(string testData)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testData))
            {
                if (testData == "new")
                {
                    expected = Guid.NewGuid();
                }
                else if (testData == "empty")
                {
                    expected = Guid.Empty;
                }
                else
                {
                    expected = Guid.Parse(testData);
                }
            }

            Guid? actual;
            var dr = CreateTestTable(typeof(Guid), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsGuid("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsGuid(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullGuid("col");
                Assert.IsNull(actual);

                actual = dr.AsNullGuid(1);
                Assert.IsNull(actual);
            }
        }



        [Test]
        [TestCase(0)]
        [TestCase(1234567.345)]
        [TestCase(-1234567890.1234567)]
        [TestCase(1234567890.12345678)]
        [TestCase(null)]
        public void GetValueFromRowAsDecimal(decimal? expected)
        {
            decimal? actual;
            var dr = CreateTestTable(typeof(decimal), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsDecimal("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsDecimal(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullDecimal("col");
                Assert.IsNull(actual);

                actual = dr.AsNullDecimal(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(1234567.345)]
        [TestCase(-1234567890.1234567)]
        [TestCase(1234567890.12345678)]
        [TestCase(null)]
        public void GetValueFromRowAsDouble(double? expected)
        {
            double? actual;
            var dr = CreateTestTable(typeof(double), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsDouble("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsDouble(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullDouble("col");
                Assert.IsNull(actual);

                actual = dr.AsNullDouble(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase((Single)0)]
        [TestCase((Single)1234567.345)]
        [TestCase((Single) (-12567890.1234567))]
        [TestCase((Single)12345890.12345678)]
        [TestCase(null)]
        public void GetValueFromRowAsSingle(Single? expected)
        {

            Single? actual;
            var dr = CreateTestTable(typeof(Single), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsSingle("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsSingle(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullSingle("col");
                Assert.IsNull(actual);

                actual = dr.AsNullSingle(1);
                Assert.IsNull(actual);
            }
        }


        [Test]
        [TestCase('a')]
        [TestCase('Z')]
        [TestCase('*')]
        [TestCase('~')]
        [TestCase(null)]
        public void GetValueFromRowAsChar(char? expected)
        {
            Char? actual;
            var dr = CreateTestTable(typeof(char), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsChar("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsChar(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullChar("col");
                Assert.IsNull(actual);

                actual = dr.AsNullChar(1);
                Assert.IsNull(actual);
            }
        }

        [Test]
        [TestCase("now")]
        [TestCase("2-Jan-2019")]
        [TestCase("2-Jan-2019 12:39:22")]
        [TestCase("2-Jan-2019 23:59:59")]
        [TestCase("2-Jan-2019 00:00:01.333")]
        [TestCase(null)]
        public void GetValueFromRowAsDateTime(string testData)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testData))
            {
                if (testData == "now")
                {
                    expected = DateTime.Now;
                }
                else
                {
                    expected = DateTime.Parse(testData);
                }
            }

            DateTime? actual;
            var dr = CreateTestTable(typeof(DateTime), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsDateTime("col");
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsDateTime(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullDateTime("col");
                Assert.IsNull(actual);

                actual = dr.AsNullDateTime(1);
                Assert.IsNull(actual);
            }
        }



        [Test]
        [TestCase("asdasd")]
        [TestCase("asd-0432 ")]
        [TestCase("&*(*(&*Y()))")]
        [TestCase("")]
        [TestCase(null)]
        public void GetValueFromRowAsString(string expected)
        {
            string actual;
            var dr = CreateTestTable(typeof(string), expected);
            dr.Read();
            if (expected != null)
            {
                actual = dr.AsString("col");
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsString(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsString("col");
                Assert.IsNull(actual);

                actual = dr.AsString(1);
                Assert.IsNull(actual);

                actual = dr.AsString("col", true);
                Assert.That(actual, Is.EqualTo(string.Empty));

                actual = dr.AsString(1, true);
                Assert.That(actual, Is.EqualTo(string.Empty));
            }
        }




        //[Test]
        //[TestCase(0, null)]
        //[TestCase(1, (ulong)123456789)]
        //public void GetValueFromRowAsLong_AsNullTests(int rowNumber, ulong? expected)
        //{
        //    ulong? actual;
        //    var dt = CreateTestTable(typeof(ulong), expected);

        //    actual = dt.Rows[rowNumber].AsNullULong("col");
        //    if (expected.HasValue)
        //        Assert.AreEqual(expected, actual.Value);
        //    else
        //        Assert.IsFalse(actual.HasValue);

        //    actual = dt.Rows[rowNumber].AsNullULong(1);
        //    if (expected.HasValue)
        //        Assert.AreEqual(expected, actual.Value);
        //    else
        //        Assert.IsFalse(actual.HasValue);

        //    actual = dt.Rows[rowNumber].AsNullULong(dt.Columns["col"]);
        //    if (expected.HasValue)
        //        Assert.AreEqual(expected, actual.Value);
        //    else
        //        Assert.IsFalse(actual.HasValue);
        //}

        //[Test]
        //[TestCase(1, (ulong)1234567890)]
        //[TestCase(1, ulong.MinValue)]
        //[TestCase(1, ulong.MaxValue)]
        //public void GetValueFromRowAsULong(int rowNumber, ulong expected)
        //{
        //    ulong actual;
        //    var dt = CreateTestTable(typeof(ulong), expected);

        //    actual = dt.Rows[rowNumber].AsULong("col");
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsULong(1);
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsULong(dt.Columns["col"]);
        //    Assert.AreEqual(expected, actual);
        //}




        //[Test]
        //[TestCase(1, (sbyte)122)]
        //[TestCase(1, sbyte.MinValue)]
        //[TestCase(1, sbyte.MaxValue)]
        //public void GetValueFromRowAsULong(int rowNumber, sbyte expected)
        //{
        //    sbyte actual;
        //    var dt = CreateTestTable(typeof(sbyte), expected);

        //    actual = dt.Rows[rowNumber].AsSbyte("col");
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsSbyte(1);
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsSbyte(dt.Columns["col"]);
        //    Assert.AreEqual(expected, actual);
        //}


        //// There is not null value for Binary it is simple a zero length array
        //[Test]
        //[TestCase(0, null)]
        //[TestCase(1, "Test Data ")]
        //[TestCase(1, "Test Date 2")]
        //[TestCase(1, "")]
        //public void GetValueFromRowAsBinary(int rowNumber, string testValue)
        //{
        //    byte[] expected = null;
        //    if (testValue != null)
        //    {
        //        expected = Encoding.ASCII.GetBytes(testValue);
        //    }
        //    byte[] actual;
        //    var dt = CreateTestTable(typeof(byte[]), expected);

        //    actual = dt.Rows[rowNumber].AsBinary("col");
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsBinary(1);
        //    Assert.AreEqual(expected, actual);

        //    actual = dt.Rows[rowNumber].AsBinary(dt.Columns["col"]);
        //    Assert.AreEqual(expected, actual);
        //}

        
        [Test]
        [TestCase("now")]
        [TestCase("00:00:00")]
        [TestCase("23:44:34.333")]
        [TestCase(null)]
        public void GetValueFromRowAsTimeSpan(string testData)
        {
            TimeSpan? expected = null;
            if (!string.IsNullOrWhiteSpace(testData))
            {
                if (testData == "now")
                {
                    expected = DateTime.Now.TimeOfDay;
                }
                else
                {
                    expected = TimeSpan.Parse(testData);
                }
            }

            TimeSpan? actual;
            var dr = CreateTestTable(typeof(TimeSpan), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsTimeSpan("col");
                Assert.AreEqual(expected, actual);

                actual = dr.AsTimeSpan(1);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                actual = dr.AsNullTimeSpan("col");
                Assert.IsNull(actual);

                actual = dr.AsNullTimeSpan(1);
                Assert.IsNull(actual);
            }
        }
    }
}