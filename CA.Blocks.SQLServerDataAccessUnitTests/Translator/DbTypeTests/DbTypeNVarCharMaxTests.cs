using System.Linq;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeNVarCharMaxTests : UnitTestDataAccess
    {
        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/configuring-parameters-and-parameter-data-types

        // This implicit conversion will fail if the string is larger than the maximum size of an NVarChar which is 4000 we testing above and below
        
        private string testDataValueForMax = string.Empty;
        private string testDataValueShort = string.Empty; 
        private class StringDataType
        {
            public string Col { get; set; }
        }


        private void InsertTestDataAsText(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("N'{0}'", data)));
        }

        [SetUp]
        public void Setup()
        {
            testDataValueForMax = string.Concat(Enumerable.Repeat("0123456789→", 500)); // Create a string 5500 char long
            testDataValueShort = string.Concat(Enumerable.Repeat("0123456789→", 300)); // Create a string 3300 char long
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NVarChar(Max) not null"));
            InsertTestDataAsText(testDataValueForMax);
            InsertTestDataAsText(testDataValueShort);
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
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(2, data.Count);
            ClassicAssert.AreEqual(testDataValueForMax, data[0] );
            ClassicAssert.AreEqual(testDataValueShort, data[1]);

        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<StringDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(2, data.Count);
            ClassicAssert.AreEqual(testDataValueForMax, data[0].Col);
            ClassicAssert.AreEqual(testDataValueShort, data[1].Col);
        }

        [Test]
        public void SelectDataWithLargeFilter()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testDataValueForMax.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }

        [Test]
        public void SelectDataWithLargeSmallFilter()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testDataValueShort.ToSqlParameter("@testValue"));
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }

    }
}