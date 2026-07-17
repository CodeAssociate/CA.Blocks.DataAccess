using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
public class DbTypeBigIntTests : UnitTestDataAccess, IDisposable
    {
        private class BigIntDataType
        {
            public long Col { get; set; }
        }

        private void InsertTestDataSQL(long data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        public DbTypeBigIntTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint not null"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL((long)int.MaxValue + (long)int.MaxValue);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }
        [Fact]
public void SelectAllData()
        {
            //Setup 
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
			//Assert
			Assert.Equal(5, data.Count);
        }
        [Fact]
public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BigIntDataType>(cmd);
			//Assert
			Assert.Equal(5, data.Count);
			Assert.Equal(-1, data[0].Col);
        }
        [Fact]
public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const long testvalue = 123; 
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

			//Asert
			Assert.Equal(3, data.Count);
        }
        [Fact]
public void SelectAllDataBigIntWithFilterWithParameters()
        {
            //setup
            const long testvalue = 123;
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue")
                .WithParameter(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

			//Asert
			Assert.Equal(3, data.Count);
        }
    }
}


