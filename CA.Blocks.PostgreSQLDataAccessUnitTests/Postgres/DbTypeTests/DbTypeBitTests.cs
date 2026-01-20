using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
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
            ExecuteNonQuery(InsertTestDataSQL(data? "B'1'" : "B'0'"));
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
            ClassicAssert.AreEqual(2, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BoolDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(2, data.Count);
            ClassicAssert.AreEqual(true, data[0].Col);
        }


        // Not Postgress as boolean datatype so you do not use bit
        [Test]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const bool testvalue = true;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = B'1'"));
 
            //Act
            var data = this.ExecuteObjectList(cmd);

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }
    }
}
