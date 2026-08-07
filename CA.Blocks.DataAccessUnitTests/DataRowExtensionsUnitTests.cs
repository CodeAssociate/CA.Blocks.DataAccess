
using System.Data;
using System.Text;
using CA.Blocks.DataAccess;


namespace CA.Blocks.DataAccessUnitTests
{
        public class DataRowExtensionsUnitTests
    {
        /// <summary>
        /// Create a test table with the first row as null and the second row as the testData value for type dbType
        /// </summary>
        private static DataTable CreateTestTable(Type dbType, object? testData)
        {
            var result = new DataTable();
            var dcKey = new DataColumn("key", typeof(int));
            result.Columns.Add(dcKey);
            var dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, null);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            return result;
        }
        
        private static DataRow GetDataRow(int rowNumber, DataTable sourceDataTable)
        {
            return sourceDataTable.Rows[rowNumber];
        }

        private static IDataReader GetDataReader(int rowNumber, DataTable sourceDataTable)
        {
            var dataReader = sourceDataTable.CreateDataReader();
            for (int i = 0; i <= rowNumber; i++)
            {
                dataReader.Read();
            }
            return dataReader;
        }
        
        private static void AssertNullable<T>(Nullable<T> expected, Nullable<T> actual)  where T : struct 
        {
            if (expected.HasValue)
            {
                Assert.NotNull(actual);
                Assert.Equal(expected, actual.Value);
            }
            else
            {
                Assert.False(actual.HasValue);
            }
        }
        
        // There is no null value for Binary it is a zero length array
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "Test Data ")]
        [InlineData(1, "Test Date 2")]
        [InlineData(1, "")]
        public void GetValueFromRowAsBinary(int rowNumber, string? testValue)
        {
            byte[] expected = Array.Empty<byte>();
            if (testValue != null)
            {
                expected = Encoding.ASCII.GetBytes(testValue);
            }
            var dt = CreateTestTable(typeof(byte[]), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
                Assert.Equal(expected, dataRow.AsBinary("col"));
                Assert.Equal(expected, dataRow.AsBinary(1));
                Assert.Equal(expected, dataRow.AsBinary(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsBinary("col"));
                Assert.Equal(expected, dataReader.AsBinary(1));
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public void GetValueFromRowAsBool(int rowNumber, bool expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsBool("col"));
                Assert.Equal(expected, dataRow.AsBool(1));
                Assert.Equal(expected, dataRow.AsBool(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsBool("col"));
                Assert.Equal(expected, dataReader.AsBool(1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public void GetValueFromRowAsBool_AsNullTests(int rowNumber, bool? expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullBool("col"));
            AssertNullable(expected, dataRow.AsNullBool(1));
            AssertNullable(expected, dataRow.AsNullBool(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullBool("col"));
            AssertNullable(expected, dataReader.AsNullBool(1));
        }

        [Theory]
        [InlineData(1, (byte)0)]
        [InlineData(1, (byte)213)]
        public void GetValueFromRowAsByte(int rowNumber, byte expected)
        {
            var dt = CreateTestTable(typeof(byte), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsByte("col"));
                Assert.Equal(expected, dataRow.AsByte(1));
                Assert.Equal(expected, dataRow.AsByte(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsByte("col"));
                Assert.Equal(expected, dataReader.AsByte(1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (byte)0)]
        [InlineData(1, (byte)128)]
        public void GetValueFromRowAsByte_AsNullTests(int rowNumber, byte? expected)
        {
            var dt = CreateTestTable(typeof(byte), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullByte("col"));
            AssertNullable(expected, dataRow.AsNullByte(1));
            AssertNullable(expected, dataRow.AsNullByte(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullByte("col"));
            AssertNullable(expected, dataReader.AsNullByte(1));
        }
        
        [Theory]
        [InlineData(1, '*')]
        [InlineData(1, '0')]
        public void GetValueFromRowAsChar(int rowNumber, char expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsChar("col"));
                Assert.Equal(expected, dataRow.AsChar(1));
                Assert.Equal(expected, dataRow.AsChar(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsChar("col"));
                Assert.Equal(expected, dataReader.AsChar(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 'a')]
        [InlineData(1, 'Z')]
        public void GetValueFromRowAsChar_AsNullTests(int rowNumber, char? expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullChar("col"));
            AssertNullable(expected, dataRow.AsNullChar(1));
            AssertNullable(expected, dataRow.AsNullChar(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullChar("col"));
            AssertNullable(expected, dataReader.AsNullChar(1));
        }

        [Theory]
        [InlineData(1, "now")]
        [InlineData(1, "2-Jan-2019")]
        [InlineData(1, "2-Jan-2019 12:39:22")]
        [InlineData(1, "2-Jan-2019 23:59:59")]
        [InlineData(1, "2-Jan-2019 00:00:01.333")]
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
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsDateTime("col"));
                Assert.Equal(expected, dataRow.AsDateTime(1));
                Assert.Equal(expected, dataRow.AsDateTime(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsDateTime("col"));
                Assert.Equal(expected, dataReader.AsDateTime(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "1-Jan-2019")]
        public void GetValueFromRowAsDateTime_AsNullTests(int rowNumber, string? testDate)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = DateTime.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(DateTime), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullDateTime("col"));
            AssertNullable(expected, dataRow.AsNullDateTime(1));
            AssertNullable(expected, dataRow.AsNullDateTime(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullDateTime("col"));
            AssertNullable(expected, dataReader.AsNullDateTime(1));
        }

        [Theory]
        [InlineData(1, 987.456d)]
        public void GetValueFromRowAsDecimal(int rowNumber, double expected)
        {
            var expectedDecimal = (decimal)expected;
            var dt = CreateTestTable(typeof(decimal), expectedDecimal);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expectedDecimal, dataRow.AsDecimal("col"));
                Assert.Equal(expectedDecimal, dataRow.AsDecimal(1));
                Assert.Equal(expectedDecimal, dataRow.AsDecimal(dt.Columns["col"]!));

                Assert.Equal(expectedDecimal, dataReader.AsDecimal("col"));
                Assert.Equal(expectedDecimal, dataReader.AsDecimal(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 987.456d)]
        public void GetValueFromRowAsDecimal_AsNullTests(int rowNumber, double? expected)
        {
            decimal? expectedDecimal = expected.HasValue ? (decimal?)expected.Value : null;
            var dt = CreateTestTable(typeof(decimal), expectedDecimal);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expectedDecimal, dataRow.AsNullDecimal("col"));
            AssertNullable(expectedDecimal, dataRow.AsNullDecimal(1));
            AssertNullable(expectedDecimal, dataRow.AsNullDecimal(dt.Columns["col"]!));
            
            AssertNullable(expectedDecimal, dataReader.AsNullDecimal("col"));
            AssertNullable(expectedDecimal, dataReader.AsNullDecimal(1));
        }
        
        [Theory]
        [InlineData(1, (double)987.456)]
        public void GetValueFromRowAsDouble(int rowNumber, double expected)
        {
            var dt = CreateTestTable(typeof(double), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            

                Assert.Equal(expected, dataRow.AsDouble("col"));
                Assert.Equal(expected, dataRow.AsDouble(1));
                Assert.Equal(expected, dataRow.AsDouble(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsDouble("col"));
                Assert.Equal(expected, dataReader.AsDouble(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 987.456)]
        public void GetValueFromRowAsDouble_AsNullTests(int rowNumber, double? expected)
        {
            var dt = CreateTestTable(typeof(double), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullDouble("col"));
            AssertNullable(expected, dataRow.AsNullDouble(1));
            AssertNullable(expected, dataRow.AsNullDouble(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullDouble("col"));
            AssertNullable(expected, dataReader.AsNullDouble(1));
        }
        
        [Theory]
        [InlineData(1, (float)987.456)]
        public void GetValueFromRowAsFloat(int rowNumber, float expected)
        {
            var dt = CreateTestTable(typeof(float), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsFloat("col"));
                Assert.Equal(expected, dataRow.AsFloat(1));
                Assert.Equal(expected, dataRow.AsFloat(dt.Columns["col"]!));

                // Single / float  a float is a single The use of "float" in C# seems to be a throwback to its C/C++ heritage. a float" still maps to the System.Single type in C# so use single where you can
                Assert.Equal(expected, dataReader.AsSingle("col"));
                Assert.Equal(expected, dataReader.AsSingle(1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (float)987.456)]
        public void GetValueFromRowAsFloat_AsNullTests(int rowNumber, float? expected)
        {
            var dt = CreateTestTable(typeof(float), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullFloat("col"));
            AssertNullable(expected, dataRow.AsNullFloat(1));
            AssertNullable(expected, dataRow.AsNullFloat(dt.Columns["col"]!));
            
            // Single / float  a float is a single The use of "float" in C# seems to be a throwback to its C/C++ heritage. a float" still maps to the System.Single type in C# so use single where you can
            AssertNullable(expected, dataReader.AsNullSingle("col"));
            AssertNullable(expected, dataReader.AsNullSingle(1));
        }
        
        [Theory]
        [InlineData(1, "new")]
        [InlineData(1, "empty")]
        [InlineData(1, "7009B509-098F-4AF4-97C8-CF354B4E0D77")]

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
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsGuid("col"));
                Assert.Equal(expected, dataRow.AsGuid(1));
                Assert.Equal(expected, dataRow.AsGuid(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsGuid("col"));
                Assert.Equal(expected, dataReader.AsGuid(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "7009B509-098F-4AF4-97C8-CF354B4E0D76")]
        public void GetValueFromRowAsGuid_AsNullTests(int rowNumber, string? testDate)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = Guid.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(Guid), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            AssertNullable(expected, dataRow.AsNullGuid("col"));
            AssertNullable(expected, dataRow.AsNullGuid(1));
            AssertNullable(expected, dataRow.AsNullGuid(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullGuid("col"));
            AssertNullable(expected, dataReader.AsNullGuid(1));
        }

        [Theory]
        [InlineData(1, 1234567)]
        [InlineData(1, int.MinValue)]
        [InlineData(1, int.MaxValue)]
        public void GetValueFromRowAsInt(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsInt("col"));
                Assert.Equal(expected, dataRow.AsInt(1));
                Assert.Equal(expected, dataRow.AsInt(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsInt("col"));
                Assert.Equal(expected, dataReader.AsInt(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 1456789)]
        public void GetValueFromRowAsInt_AsNullTests(int rowNumber, int? expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullInt("col"));
            AssertNullable(expected, dataRow.AsNullInt(1));
            AssertNullable(expected, dataRow.AsNullInt(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullInt("col"));
            AssertNullable(expected, dataReader.AsNullInt(1));
        }
        
        [Theory]
        [InlineData(1, (long)1234567890)]
        [InlineData(1, long.MinValue)]
        [InlineData(1, long.MaxValue)]
        public void GetValueFromRowAsLong(int rowNumber, long expected)
        {
            var dt = CreateTestTable(typeof(long), expected);
            
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsLong("col"));
                Assert.Equal(expected, dataRow.AsLong(1));
                Assert.Equal(expected, dataRow.AsLong(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsLong("col"));
                Assert.Equal(expected, dataReader.AsLong(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (long)123456789)]
        public void GetValueFromRowAsLong_AsNullTests(int rowNumber, long? expected)
        {
            var dt = CreateTestTable(typeof(long), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullLong("col"));
            AssertNullable(expected, dataRow.AsNullLong(1));
            AssertNullable(expected, dataRow.AsNullLong(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullLong("col"));
            AssertNullable(expected, dataReader.AsNullLong(1));
        }
        
        [Theory]
        [InlineData(1, (sbyte)122)]
        [InlineData(1, sbyte.MinValue)]
        [InlineData(1, sbyte.MaxValue)]
        public void GetValueFromRowAsSbyte(int rowNumber, sbyte expected)
        {
            var dt = CreateTestTable(typeof(sbyte), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsSbyte("col"));
                Assert.Equal(expected, dataRow.AsSbyte(1));
                Assert.Equal(expected, dataRow.AsSbyte(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsSbyte("col"));
                Assert.Equal(expected, dataReader.AsSbyte(1));
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (sbyte)-12)]
        [InlineData(1, (sbyte)13)]
        public void GetValueFromRowAsSbyte_AsNullTests(int rowNumber, sbyte? expected)
        {
            var dt = CreateTestTable(typeof(sbyte), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullSbyte("col"));
            AssertNullable(expected, dataRow.AsNullSbyte(1));
            AssertNullable(expected, dataRow.AsNullSbyte(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullSbyte("col"));
            AssertNullable(expected, dataReader.AsNullSbyte(1));
        }
        
        [Theory]
        [InlineData(1, (short)1442)]
        [InlineData(1, short.MinValue)]
        [InlineData(1, short.MaxValue)]
        public void GetValueFromRowAsShort(int rowNumber, short expected)
        {
            var dt = CreateTestTable(typeof(short), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var actual = dataRow.AsShort("col");
            Assert.Equal(expected, actual);

            actual = dataRow.AsShort(1);
            Assert.Equal(expected, actual);

            actual = dataRow.AsShort(dt.Columns["col"]!);
            Assert.Equal(expected, actual);

            actual = dataReader.AsShort("col");
            Assert.Equal(expected, actual);

            actual = dataReader.AsShort(1);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (short)-12)]
        [InlineData(1, (short)13)]
        public void GetValueFromRowAsShort_AsNullTests(int rowNumber, short? expected)
        {
            var dt = CreateTestTable(typeof(short), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullShort("col"));
            AssertNullable(expected, dataRow.AsNullShort(1));
            AssertNullable(expected, dataRow.AsNullShort(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullShort("col"));
            AssertNullable(expected, dataReader.AsNullShort(1));
        }
        
        [Theory]
        [InlineData(1, (float)1442.34)]
        [InlineData(1, float.MinValue)]
        [InlineData(1, float.MaxValue)]
        public void GetValueFromRowAsSingle(int rowNumber, float expected)
        {
            var dt = CreateTestTable(typeof(float), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsSingle("col"));
                Assert.Equal(expected, dataRow.AsSingle(1));
                Assert.Equal(expected, dataRow.AsSingle(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsSingle("col"));
                Assert.Equal(expected, dataReader.AsSingle(1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (float)-12.23)]
        [InlineData(1,(float)13.56)]
        public void GetValueFromRowAsSingle_AsNullTests(int rowNumber, float? expected)
        {
            var dt = CreateTestTable(typeof(float), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            AssertNullable(expected, dataRow.AsNullSingle("col"));
            AssertNullable(expected, dataRow.AsNullSingle(1));
            AssertNullable(expected, dataRow.AsNullSingle(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullSingle("col"));
            AssertNullable(expected, dataReader.AsNullSingle(1));
        }

        [Theory]
        [InlineData(1, "")]
        [InlineData(1, "12345")]
        [InlineData(1, "734^%$%$^$^%")]
        public void GetValueFromRowAsString(int rowNumber, string expected)
        {
            var dt = CreateTestTable(typeof(string), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            

                Assert.Equal(expected, dataRow.AsString("col"));
                Assert.Equal(expected, dataRow.AsString(1));
                Assert.Equal(expected, dataRow.AsString(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsString("col"));
                Assert.Equal(expected, dataReader.AsString(1));
        }
        
        
        [Theory]
        [InlineData(0, null)]
        public void GetValueFromRowAsString_NullTests(int rowNumber, string? data)
        {
            var dt = CreateTestTable(typeof(string), data);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Null(dataRow.AsString("col"));
                Assert.Null(dataRow.AsString(1));
                Assert.Null(dataRow.AsString(dt.Columns["col"]!));

                Assert.Null(dataReader.AsString("col"));
                Assert.Null(dataReader.AsString(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        public void GetValueFromRowAsString_NullTestsEmptyString(int rowNumber, string? data)
        {
            var dt = CreateTestTable(typeof(string), data);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(string.Empty, dataRow.AsString("col", true));
                Assert.Equal(string.Empty, dataRow.AsString(1, true));
                Assert.Equal(string.Empty, dataRow.AsString(dt.Columns["col"]!, true));

                Assert.Equal(string.Empty, dataReader.AsString("col", true));
                Assert.Equal(string.Empty, dataReader.AsString(1, true));
        }

        [Theory]
        [InlineData(1, "now")]
        [InlineData(1, "00:00:00")]
        [InlineData(1, "23:44:34.333")]
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
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsTimeSpan("col"));
                Assert.Equal(expected, dataRow.AsTimeSpan(1));
                Assert.Equal(expected, dataRow.AsTimeSpan(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsTimeSpan("col"));
                Assert.Equal(expected, dataReader.AsTimeSpan(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "22:21:20")]
        public void GetValueFromRowAsTimeSpan_AsNullTests(int rowNumber, string? testDate)
        {
            TimeSpan? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = TimeSpan.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(TimeSpan), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullTimeSpan("col"));
            AssertNullable(expected, dataRow.AsNullTimeSpan(1));
            AssertNullable(expected, dataRow.AsNullTimeSpan(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullTimeSpan("col"));
            AssertNullable(expected, dataReader.AsNullTimeSpan(1));
        }
        
        [Theory]
        [InlineData(1, (uint)1234567890)]
        [InlineData(1, uint.MinValue)]
        [InlineData(1, uint.MaxValue)]
        public void GetValueFromRowAsUInt(int rowNumber, uint expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);
            var dataRow = GetDataRow(rowNumber, dt);

            var dataReader = GetDataReader(rowNumber, dt);
           

            
                Assert.Equal(expected, dataRow.AsUInt("col"));
                Assert.Equal(expected, dataRow.AsUInt(1));
                Assert.Equal(expected, dataRow.AsUInt(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsUInt("col"));
                Assert.Equal(expected, dataReader.AsUInt(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (uint)123456789)]
        [InlineData(1, uint.MinValue)]
        [InlineData(1, uint.MaxValue)]
        public void GetValueFromRowAsUInt_AsNullTests(int rowNumber, uint? expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullUInt("col"));
            AssertNullable(expected, dataRow.AsNullUInt(1));
            AssertNullable(expected, dataRow.AsNullUInt(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullUInt("col"));
            AssertNullable(expected, dataReader.AsNullUInt(1));
        }

        [Theory]
        [InlineData(1, (ulong)1234567890)]
        [InlineData(1, ulong.MinValue)]
        [InlineData(1, ulong.MaxValue)]
        public void GetValueFromRowAsULong(int rowNumber, ulong expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsULong("col"));
                Assert.Equal(expected, dataRow.AsULong(1));
                Assert.Equal(expected, dataRow.AsULong(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsULong("col"));
                Assert.Equal(expected, dataReader.AsULong(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (ulong)123456789)]
        [InlineData(1, ulong.MinValue)]
        [InlineData(1, ulong.MaxValue)]
        public void GetValueFromRowAsULong_AsNullTests(int rowNumber, ulong? expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullULong("col"));
            AssertNullable(expected, dataRow.AsNullULong(1));
            AssertNullable(expected, dataRow.AsNullULong(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullULong("col"));
            AssertNullable(expected, dataReader.AsNullULong(1));
        }
        
        
        [Theory]
        [InlineData(1, (ushort)12345)]
        [InlineData(1, ushort.MinValue)]
        [InlineData(1, ushort.MaxValue)]
        public void GetValueFromRowAsUShort(int rowNumber, ushort expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            
                Assert.Equal(expected, dataRow.AsUShort("col"));
                Assert.Equal(expected, dataRow.AsUShort(1));
                Assert.Equal(expected, dataRow.AsUShort(dt.Columns["col"]!));

                Assert.Equal(expected, dataReader.AsUShort("col"));
                Assert.Equal(expected, dataReader.AsUShort(1));
        }
        
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (ushort)12345)]
        [InlineData(1, ushort.MinValue)]
        [InlineData(1, ushort.MaxValue)]
        public void GetValueFromRowAsUShort_AsNullTests(int rowNumber, ushort? expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            
            AssertNullable(expected, dataRow.AsNullUShort("col"));
            AssertNullable(expected, dataRow.AsNullUShort(1));
            AssertNullable(expected, dataRow.AsNullUShort(dt.Columns["col"]!));
            
            AssertNullable(expected, dataReader.AsNullUShort("col"));
            AssertNullable(expected, dataReader.AsNullUShort(1));
        }

    }
}
