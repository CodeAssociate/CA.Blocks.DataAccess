using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeNCharTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeNCharTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("NChar"));
            InsertTestDataSQL('�');
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
        public void SelectAllDataNChar()
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
        public void SelectAllDataNCharWithFilter()
        {
            //setup
            char testvalue = '�';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var parameter = testvalue.ToSqlParameter("@testValue", SpecificSQLCharType.NChar);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {parameter}");

            var data = t.Translate(Execute(cmd).ToDataRow());
            //Asert
            Assert.Equal(testvalue, data);
            
        }
    }
}




