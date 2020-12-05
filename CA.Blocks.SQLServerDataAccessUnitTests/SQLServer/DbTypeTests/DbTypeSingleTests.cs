using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer.DbTypeTests
{
    [TestFixture]
    public class DbTypeSingleTests : UnitTestDataAccess
    {

        private class SingleDataType
        {
            public Single Col { get; set; }
        }


        private void InsertTestDataSQL(float data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("real not null"));
            InsertTestDataSQL((float)-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL((float)123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL((float)123456789.987654321);
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
            var data = ExecuteToListOf<SingleDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(-(float)1.2, data[0].Col);
        }

        [Test]
        public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<SingleDataType>(cmd);

            //Asert
            Assert.AreEqual(3, data.Count);
        }


    }
}
