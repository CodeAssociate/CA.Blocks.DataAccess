
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeBitTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(bool data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data? "1":"0"));
        }

        public DbTypeBitTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bit not null"));
            InsertTestDataSQL(true);
            InsertTestDataSQL(false);
        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataBitInt()
        {
            //Setup 
           
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteObjectList(cmd);
            //Assert
            Assert.True(data.Count == 2);
        }

        [Fact]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = this.ExecuteObjectList(cmd);

            //Asert
            Assert.Single(data);
        }


    }
}





