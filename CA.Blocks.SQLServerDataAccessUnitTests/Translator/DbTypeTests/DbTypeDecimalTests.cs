using CA.Blocks.DataAccess.Translator.Extensions;
using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeDecimalTests : UnitTestDataAccess, IDisposable
    {

        private class DecimalDataType
        {
            public Decimal Col { get; set; }
        }


        private void InsertTestDataSQL(double data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"{data}"));
        }

        public DbTypeDecimalTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("decimal(20,10) not null"));
            InsertTestDataSQL(-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.987654321);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteDataTable(cmd);
            //Assert
            Assert.Equal(5, data.Rows.Count);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<DecimalDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2M, data[0].Col);
            Assert.Equal(123456789.98765433M, data[4].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<DecimalDataType>(cmd);

            //Asert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const Decimal testValue = 123.456M;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DecimalDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




