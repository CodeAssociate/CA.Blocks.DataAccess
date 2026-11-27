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


        private void InsertTestDataSQL(decimal data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeDecimalTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            var createCommand = CreateTestTableCommand("decimal");
            createCommand.CommandText = createCommand.CommandText.Replace("[decimal]", "[decimal](20,10)");
            ExecuteNonQuery(createCommand);
            InsertTestDataSQL(-1.2M);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456M);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.98765433M);
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
            var data = Execute(cmd).ToListOf<DecimalDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2M, data[0].Col);
            Assert.Equal(123456789.98765433M, data[4].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const decimal testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToListOf<DecimalDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const Decimal testValue = 123.456M;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DecimalDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




