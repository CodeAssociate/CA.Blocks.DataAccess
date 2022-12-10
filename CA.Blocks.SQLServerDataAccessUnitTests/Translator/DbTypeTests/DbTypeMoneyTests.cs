using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeMoneyTests : UnitTestDataAccess
    {

        private class MoneyDataType
        {
            public decimal Col { get; set; }
        }


        private void InsertTestDataSQL(double data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("money not null"));
            InsertTestDataSQL(-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.9876);
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteDataTable(cmd);
            //Assert
            Assert.AreEqual(5, data.Rows.Count);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<MoneyDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(-1.2, data[0].Col);
            Assert.AreEqual(123456789.9876, data[4].Col);
        }

        [Test]
        public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<MoneyDataType>(cmd);

            //Asert
            Assert.AreEqual(3, data.Count);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const Decimal testValue = 123.456M;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<MoneyDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.AreEqual(testValue, data.Col);
        }

    }
}
