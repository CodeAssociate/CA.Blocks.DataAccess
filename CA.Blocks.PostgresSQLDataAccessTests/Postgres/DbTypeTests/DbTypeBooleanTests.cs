using CA.Blocks.DataAccess;
using CA.Blocks.PostgresSQLDataAccessTests.Base;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeBooleanTests : UnitTestDataAccess, IDisposable
    {
        private class BoolDataType
        {
            public bool Col { get; set; }
        }

        private void InsertTestDataSQL(bool data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data ? "TRUE" : "FALSE"));
        }

        public DbTypeBooleanTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("boolean not null"));
            InsertTestDataSQL(true);
            InsertTestDataSQL(false);
        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
            base.Dispose();
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.ExecuteObjectList(cmd);
            //Assert
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BoolDataType>(cmd);
            //Assert
            Assert.Equal(2, data.Count);
            Assert.True(data[0].Col);
        }


        [Fact]
        public void SelectAllDataWithFilter()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testvalue")).WithParameter(testvalue.ToPostgresParameter("testvalue"));

            //Act
            var data = this.ExecuteObjectList(cmd);

            //Asert
            Assert.Single(data);
        }
    }
}
