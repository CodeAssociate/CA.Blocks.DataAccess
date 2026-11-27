using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeMoneyTests : UnitTestDataAccess, IDisposable
    {

        private class MoneyDataType
        {
            public decimal Col { get; set; }
        }


        private void InsertTestDataSQL(decimal data)
        {
            var cmd = InsertTestDataCommand(data);
            cmd.Parameters["@value"].SqlDbType = System.Data.SqlDbType.Money;
            ExecuteNonQuery(cmd);
        }

        public DbTypeMoneyTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("money"));
            InsertTestDataSQL(-1.2M);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456M);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.9876M);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<decimal>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<MoneyDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2M, data[0].Col);
            Assert.Equal(123456789.9876M, data[4].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const decimal testvalue = 123M;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {testvalue:@testValue|Money}");

            //Act
            var data = Execute(cmd).ToListOf<MoneyDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const Decimal testValue = 123.456M;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value|Money}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<MoneyDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




