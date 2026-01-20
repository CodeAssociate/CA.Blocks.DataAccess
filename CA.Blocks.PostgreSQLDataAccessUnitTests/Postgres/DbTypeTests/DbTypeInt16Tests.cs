using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using Npgsql;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [TestFixture]
    public class DbTypeInt16Tests : UnitTestDataAccess
    {
        private class ShortDataType
        {
            public short Col { get; set; }
        }

        private void InsertTestDataSQL(short data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("int not null"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(short.MaxValue);
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
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ShortDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(-1, data[0].Col);
        }


        [Test]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const int testvalue = 123; 
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
            cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }

        
        [Test]
        public void SelectAllDataBigIntWithFilterWithParameters()
        {
            //setup
            const int testvalue = 123;
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"))
                .WithParameters(new List<NpgsqlParameter>
                {
                    testvalue.ToPostgresParameter("@testValue")
                });

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Assert
            ClassicAssert.AreEqual(3, data.Count);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testvalue")).WithParameter(testvalue.ToPostgresParameter("testvalue"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ShortDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            ClassicAssert.AreEqual(123, data.Col);
        }
    }
}
