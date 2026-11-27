
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeDoubleTests : UnitTestDataAccess, IDisposable
    {

        private class DoubleDataType
        {
            public Double Col { get; set; }
        }


        private void InsertTestDataSQL(double data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeDoubleTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("float"));
            InsertTestDataSQL(-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.987654321);
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
            var data = Execute(cmd).ToSingleNamedColumnList<double>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<DoubleDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2, data[0].Col);
            Assert.Equal(123456789.987654321, data[4].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const double testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToListOf<DoubleDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }
        
        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const double testValue = 123.456;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DoubleDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }
    }
}




