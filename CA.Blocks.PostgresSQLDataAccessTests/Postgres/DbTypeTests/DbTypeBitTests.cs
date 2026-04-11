using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [Collection("DbTypeTests")]
    public class DbTypeBitTests : UnitTestDataAccess, IDisposable
    {
        private class BoolDataType
        {
            public bool Col { get; set; }
        }

        private void InsertTestDataSQL(bool data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data? "B'1'" : "B'0'"));
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
            Assert.Equal(true, data[0].Col);
        }


        // Not Postgress as boolean datatype so you do not use bit
        [Fact]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = B'1'"));

            //Act
            var data = this.ExecuteObjectList(cmd);

            //Asert
            Assert.Equal(1, data.Count);
        }
    }
}
