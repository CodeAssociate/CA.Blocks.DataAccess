using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeBitTests : UnitTestDataAccess
    {
        private class BoolDataType
        {
            public bool Col { get; set; }
        }

        private void InsertTestDataSQL(bool data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data? "1":"0"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bit not null"));
            InsertTestDataSQL(true);
            InsertTestDataSQL(false);
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
            var data = this.ExecuteObjectList(cmd);
            //Assert
            Assert.AreEqual(2, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BoolDataType>(cmd);
            //Assert
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(true, data[0].Col);
        }

        [Test]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = this.ExecuteObjectList(cmd);

            //Asert
            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testvalue.ToSqlParameter("@value"));
            
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BoolDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.AreEqual(testvalue, data.Col);
        }
        
    }
}
