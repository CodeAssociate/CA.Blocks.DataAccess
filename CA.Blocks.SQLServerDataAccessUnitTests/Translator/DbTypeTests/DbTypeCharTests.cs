using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeCharTests : UnitTestDataAccess, IDisposable
    {
        private class CharDataType
        {
            public char Col { get; set; }
        }

        private void InsertTestDataSQL(char data)
        {
            var cmd = InsertTestDataCommand(data);
            cmd.Parameters["@value"].SqlDbType = System.Data.SqlDbType.Char;
            ExecuteNonQuery(cmd);
        }

        public DbTypeCharTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("char"));
            InsertTestDataSQL('A');
            InsertTestDataSQL('B');
            InsertTestDataSQL('C');
            InsertTestDataSQL('D');
            InsertTestDataSQL('E');

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
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<CharDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal('E', data[4].Col);
        }



        [Fact]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const char testvalue = 'A';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue|Char}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());

            //Asert
            Assert.Equal('A', data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const char testValue = 'A';
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value|Char}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CharDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




