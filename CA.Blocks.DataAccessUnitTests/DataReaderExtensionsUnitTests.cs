using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccessUnitTests.Base;

namespace CA.Blocks.DataAccessUnitTests
{
    [TestFixture]
    public class DataReaderExtensionsUnitTests
    {
        // we use a simple mock of reader using a data table
        private static IDataReader CreateTestTable(Type dbType, object? testData)
        {
            var result = new DataTable();
            var dcKey = new DataColumn("key", typeof(int));
            result.Columns.Add(dcKey);
            var dc = new DataColumn("col", dbType);
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
            Assert.Multiple(() =>
            {
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
                    Assert.That(actual, Is.Null);

                    actual = dr.AsNullByte(1);
                    Assert.That(actual, Is.Null);
                }
            });
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
            Assert.Multiple(() =>
            {
                if (expected.HasValue)
                {
                    actual = dr.AsShort("col");
                    Assert.That(actual, Is.EqualTo(expected));

                    actual = dr.AsShort(1);
                    Assert.That(actual, Is.EqualTo(expected));
                }
                else
                {
                    actual = dr.AsNullShort("col");
                    Assert.That(actual, Is.Null);

                    actual = dr.AsNullShort(1);
                    Assert.That(actual, Is.Null);
                }
            });
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
                Assert.That(actual, Is.Null);

                actual = dr.AsNullInt(1);
                Assert.That(actual, Is.Null);
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsLong(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullLong("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullLong(1);
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        [TestCase("new")]
        [TestCase("empty")]
        [TestCase("7009B509-098F-4AF4-97C8-CF354B4E0D77")]
        [TestCase(null)]
        public void GetValueFromRowAsGuid(string? testData)
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsGuid(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullGuid("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullGuid(1);
                Assert.That(actual, Is.Null);
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsDecimal(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullDecimal("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullDecimal(1);
                Assert.That(actual, Is.Null);
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsDouble(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullDouble("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullDouble(1);
                Assert.That(actual, Is.Null);
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsSingle(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullSingle("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullSingle(1);
                Assert.That(actual, Is.Null);
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsChar(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullChar("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullChar(1);
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        [TestCase("now")]
        [TestCase("2-Jan-2019")]
        [TestCase("2-Jan-2019 12:39:22")]
        [TestCase("2-Jan-2019 23:59:59")]
        [TestCase("2-Jan-2019 00:00:01.333")]
        [TestCase(null)]
        public void GetValueFromRowAsDateTime(string? testData)
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
                Assert.That(actual, Is.Null);

                actual = dr.AsNullDateTime(1);
                Assert.That(actual, Is.Null);
            }
        }



        [Test]
        [TestCase("asdasd")]
        [TestCase("asd-0432 ")]
        [TestCase("&*(*(&*Y()))")]
        [TestCase("")]
        [TestCase(null)]
        public void GetValueFromRowAsString(string? expected)
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
                Assert.That(actual, Is.Null);

                actual = dr.AsString(1);
                Assert.That(actual, Is.Null);

                actual = dr.AsString("col", true);
                Assert.That(actual, Is.EqualTo(string.Empty));

                actual = dr.AsString(1, true);
                Assert.That(actual, Is.EqualTo(string.Empty));
            }
        }


        [Test]
        [TestCase("now")]
        [TestCase("00:00:00")]
        [TestCase("23:44:34.333")]
        [TestCase(null)]
        public void GetValueFromRowAsTimeSpan(string? testData)
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
                Assert.That(actual, Is.EqualTo(expected));

                actual = dr.AsTimeSpan(1);
                Assert.That(actual, Is.EqualTo(expected));
            }
            else
            {
                actual = dr.AsNullTimeSpan("col");
                Assert.That(actual, Is.Null);

                actual = dr.AsNullTimeSpan(1);
                Assert.That(actual, Is.Null);
            }
        }
    }
}