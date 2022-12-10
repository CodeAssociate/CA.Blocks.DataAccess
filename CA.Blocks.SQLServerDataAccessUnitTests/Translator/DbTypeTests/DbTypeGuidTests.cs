using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeGuidTests : UnitTestDataAccess
    {
        private const string TestGuidValue = "CE69B300-F9EA-4F3B-BBA8-676D12737E3E";
        private class GuidDataType
        {
            public Guid Col { get; set; }
        }


        private void InsertTestDataSQL(Guid data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data.ToString()}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("UniqueIdentifier  not null"));
            InsertTestDataSQL(Guid.Empty);
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.Parse(TestGuidValue));
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
            var data = ExecuteToListOf<GuidDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(Guid.Parse(TestGuidValue), data[4].Col);
        }

        [Test]
        public void SelectAllDataTimeWithFilter()
        {
            //setup
            Guid testvalue = Guid.Parse(TestGuidValue);
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

    
            //Act
            var data = ExecuteTo<GuidDataType>(cmd);

            //Asert
            Assert.AreEqual(testvalue, data.Col);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            Guid testValue = Guid.Parse(TestGuidValue);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<GuidDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.AreEqual(testValue, data.Col);
        }
    }
}
