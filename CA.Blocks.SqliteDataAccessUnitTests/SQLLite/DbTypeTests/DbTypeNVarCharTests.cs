using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    [TestFixture]
    public class DbTypeNVarCharTests : UnitTestDataAccess
    {
        private const string  TEST_DATA = "nvarchar data";

        private void InsertTestDataAsTextSQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NVarChar(50) not null"));
            InsertTestDataAsTextSQL(TEST_DATA);
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL("ä");
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllDataBinary()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<string>(UNIT_TEST_COL_NAME);
            //Assert
            ClassicAssert.AreEqual(6, data.Count);
            ClassicAssert.AreEqual(TEST_DATA, data[0]);
            ClassicAssert.AreEqual("ä", data[5]);
        }

        
        [Test]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<string>(UNIT_TEST_COL_NAME);

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }

    }
}
