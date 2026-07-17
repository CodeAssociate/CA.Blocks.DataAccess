using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    public class DbTypeCharTests : UnitTestDataAccess, IDisposable
    {
        private class CharDataType
        {
            public char Col { get; set; }
        }

        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        public DbTypeCharTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("char not null"));
            InsertTestDataSQL('A');
            InsertTestDataSQL('B');
            InsertTestDataSQL('C');
            InsertTestDataSQL('D');
            InsertTestDataSQL('E');

        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
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
            var data = ExecuteToListOf<CharDataType>(cmd);
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
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            Assert.Equal('A', data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const char testValue = 'A';
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CharDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




