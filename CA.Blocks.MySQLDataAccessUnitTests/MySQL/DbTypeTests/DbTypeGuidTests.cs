using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
    [TestFixture]
    public class DbTypeGuidTests : UnitTestDataAccess
    {

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
            // My SQL  does not have a storage type for GUID best is char(36) to used as it or with binary storage 
            ExecuteNonQuery(CreateTestTable("char(36) not null"));
            InsertTestDataSQL(Guid.Empty);
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.Parse("CE69B300-F9EA-4F3B-BBA8-676D12737E3E"));
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
            ClassicAssert.AreEqual(5, data.Rows.Count);
        }



        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<GuidDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(Guid.Parse("CE69B300-F9EA-4F3B-BBA8-676D12737E3E"), data[4].Col);
        }

        [Test]
        public void SelectAllDataWithFilter()
        {
            //setup
            Guid testvalue = Guid.Parse("CE69B300-F9EA-4F3B-BBA8-676D12737E3E");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));
            
            //Act
            var data = ExecuteTo<GuidDataType>(cmd);
            
            //Asert
            ClassicAssert.AreEqual(testvalue, data.Col);
        }

        
        [Test]
        public void SelectAllDataWithFilterWithTranslator()
        {
            //setup
            Guid testvalue = Guid.Parse("CE69B300-F9EA-4F3B-BBA8-676D12737E3E");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<GuidDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            ClassicAssert.AreEqual(testvalue, data.Col);
        }
    }
}