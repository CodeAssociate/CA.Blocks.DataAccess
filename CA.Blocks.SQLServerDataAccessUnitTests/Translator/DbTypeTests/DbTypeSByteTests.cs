using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeSByteTests : UnitTestDataAccess, IDisposable
    {

        private class SByteDataType
        {
            public sbyte Col { get; set; }
        }

        private void InsertTestDataSQL(short data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeSByteTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("smallint"));
            InsertTestDataSQL(-128);
            InsertTestDataSQL(0);
            InsertTestDataSQL(10);
            InsertTestDataSQL(100);
            InsertTestDataSQL(127);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<SByteDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-128, data[0].Col);
            Assert.Equal(127, data[4].Col);
        }
        
        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            sbyte testValue = 127;
            var parameter = testValue.ToSqlParameter("@value");
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {parameter}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SByteDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




