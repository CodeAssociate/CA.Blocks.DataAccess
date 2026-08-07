using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccessUnitTests.Base;

namespace CA.Blocks.DataAccessUnitTests
{
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


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public void GetValueFromRowAsBool(bool? expected)
        {
            bool? actual;
            var dr = CreateTestTable(typeof(bool), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsBool("col");
                Assert.Equal(expected, actual);

                actual = dr.AsBool(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullBool("col");
                Assert.Null(actual);

                actual = dr.AsNullBool(1);
                Assert.Null(actual);
            }
        }


        [Theory]
        [InlineData((byte)123)]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData(null)]
        public void GetValueFromRowAsByte(byte? expected)
        {
            byte? actual;
            var dr = CreateTestTable(typeof(byte), expected);
            dr.Read();
            
                if (expected.HasValue)
                {
                    actual = dr.AsByte("col");
                    Assert.Equal(expected, actual);

                    actual = dr.AsByte(1);
                    Assert.Equal(expected, actual);
                }
                else
                {
                    actual = dr.AsNullByte("col");
                    Assert.Null(actual);

                    actual = dr.AsNullByte(1);
                    Assert.Null(actual);
                }
        }

        [Theory]
        [InlineData((short)12345)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(null)]
        public void GetValueFromRowAsShort(short? expected)
        {
            short? actual;
            var dr = CreateTestTable(typeof(short), expected);
            dr.Read();
            
                if (expected.HasValue)
                {
                    actual = dr.AsShort("col");
                    Assert.Equal(expected, actual);

                    actual = dr.AsShort(1);
                    Assert.Equal(expected, actual);
                }
                else
                {
                    actual = dr.AsNullShort("col");
                    Assert.Null(actual);

                    actual = dr.AsNullShort(1);
                    Assert.Null(actual);
                }
        }

        [Theory]
        [InlineData(1234567)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(null)]
        public void GetValueFromRowAsInt(int? expected)
        {
            int? actual;
            var dr = CreateTestTable(typeof(int), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsInt("col");
                Assert.Equal(expected, actual);

                actual = dr.AsInt(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullInt("col");
                Assert.Null(actual);

                actual = dr.AsNullInt(1);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData(1234567L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(null)]
        public void GetValueFromRowAsLong(long? expected)
        {
            long? actual;
            var dr = CreateTestTable(typeof(long), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsLong("col");
                Assert.Equal(expected, actual);

                actual = dr.AsLong(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullLong("col");
                Assert.Null(actual);

                actual = dr.AsNullLong(1);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData("new")]
        [InlineData("empty")]
        [InlineData("7009B509-098F-4AF4-97C8-CF354B4E0D77")]
        [InlineData(null)]
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
                Assert.Equal(expected, actual);

                actual = dr.AsGuid(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullGuid("col");
                Assert.Null(actual);

                actual = dr.AsNullGuid(1);
                Assert.Null(actual);
            }
        }



        [Theory]
        [InlineData(0d)]
        [InlineData(1234567.345d)]
        [InlineData(-1234567890.1234567d)]
        [InlineData(1234567890.12345678d)]
        [InlineData(null)]
        public void GetValueFromRowAsDecimal(double? expected)
        {
            decimal? expectedDecimal = expected.HasValue ? (decimal?)expected.Value : null;
            decimal? actual;
            var dr = CreateTestTable(typeof(decimal), expectedDecimal);
            dr.Read();
            if (expectedDecimal.HasValue)
            {
                actual = dr.AsDecimal("col");
                Assert.Equal(expectedDecimal, actual);

                actual = dr.AsDecimal(1);
                Assert.Equal(expectedDecimal, actual);
            }
            else
            {
                actual = dr.AsNullDecimal("col");
                Assert.Null(actual);

                actual = dr.AsNullDecimal(1);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData(0d)]
        [InlineData(1234567.345d)]
        [InlineData(-1234567890.1234567d)]
        [InlineData(1234567890.12345678d)]
        [InlineData(null)]
        public void GetValueFromRowAsDouble(double? expected)
        {
            double? actual;
            var dr = CreateTestTable(typeof(double), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsDouble("col");
                Assert.Equal(expected, actual);

                actual = dr.AsDouble(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullDouble("col");
                Assert.Null(actual);

                actual = dr.AsNullDouble(1);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData((Single)0)]
        [InlineData((Single)1234567.345)]
        [InlineData((Single) (-12567890.1234567))]
        [InlineData((Single)12345890.12345678)]
        [InlineData(null)]
        public void GetValueFromRowAsSingle(Single? expected)
        {

            Single? actual;
            var dr = CreateTestTable(typeof(Single), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsSingle("col");
                Assert.Equal(expected, actual);

                actual = dr.AsSingle(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullSingle("col");
                Assert.Null(actual);

                actual = dr.AsNullSingle(1);
                Assert.Null(actual);
            }
        }


        [Theory]
        [InlineData('a')]
        [InlineData('Z')]
        [InlineData('*')]
        [InlineData('~')]
        [InlineData(null)]
        public void GetValueFromRowAsChar(char? expected)
        {
            Char? actual;
            var dr = CreateTestTable(typeof(char), expected);
            dr.Read();
            if (expected.HasValue)
            {
                actual = dr.AsChar("col");
                Assert.Equal(expected, actual);

                actual = dr.AsChar(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullChar("col");
                Assert.Null(actual);

                actual = dr.AsNullChar(1);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData("now")]
        [InlineData("2-Jan-2019")]
        [InlineData("2-Jan-2019 12:39:22")]
        [InlineData("2-Jan-2019 23:59:59")]
        [InlineData("2-Jan-2019 00:00:01.333")]
        [InlineData(null)]
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
                Assert.Equal(expected, actual);

                actual = dr.AsDateTime(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullDateTime("col");
                Assert.Null(actual);

                actual = dr.AsNullDateTime(1);
                Assert.Null(actual);
            }
        }



        [Theory]
        [InlineData("asdasd")]
        [InlineData("asd-0432 ")]
        [InlineData("&*(*(&*Y()))")]
        [InlineData("")]
        [InlineData(null)]
        public void GetValueFromRowAsString(string? expected)
        {
            string? actual;
            var dr = CreateTestTable(typeof(string), expected);
            dr.Read();
            if (expected != null)
            {
                actual = dr.AsString("col");
                Assert.Equal(expected, actual);

                actual = dr.AsString(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsString("col");
                Assert.Null(actual);

                actual = dr.AsString(1);
                Assert.Null(actual);

                actual = dr.AsString("col", true);
                Assert.Equal(string.Empty, actual);

                actual = dr.AsString(1, true);
                Assert.Equal(string.Empty, actual);
            }
        }


        [Theory]
        [InlineData("now")]
        [InlineData("00:00:00")]
        [InlineData("23:44:34.333")]
        [InlineData(null)]
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
                Assert.Equal(expected, actual);

                actual = dr.AsTimeSpan(1);
                Assert.Equal(expected, actual);
            }
            else
            {
                actual = dr.AsNullTimeSpan("col");
                Assert.Null(actual);

                actual = dr.AsNullTimeSpan(1);
                Assert.Null(actual);
            }
        }
    }
}
