using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    public class DbTypeSByteTests : UnitTestDataAccess, IDisposable
    {

        private class SByteDataType
        {
            public sbyte Col { get; set; }
        }

        private void InsertTestDataSQL(short data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        public DbTypeSByteTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("smallint not null"));
            InsertTestDataSQL(-128);
            InsertTestDataSQL(0);
            InsertTestDataSQL(10);
            InsertTestDataSQL(100);
            InsertTestDataSQL(127);
        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<SByteDataType>(cmd);
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
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SByteDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




