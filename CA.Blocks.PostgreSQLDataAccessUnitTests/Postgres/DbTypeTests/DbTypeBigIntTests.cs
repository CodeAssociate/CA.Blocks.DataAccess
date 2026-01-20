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
    public class DbTypeBigIntTests : UnitTestDataAccess
    {
        private class BigIntDataType
        {
            public long Col { get; set; }
        }

        private void InsertTestDataSQL(long data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint not null"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL((long)int.MaxValue + (long)int.MaxValue);
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
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
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
            var data = ExecuteToListOf<BigIntDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(-1, data[0].Col);
        }


        [Test]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const long testvalue = 123; 
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
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
            const long testvalue = 123;
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
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
            const long testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testvalue")).WithParameter(testvalue.ToPostgresParameter("testvalue"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BigIntDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            ClassicAssert.AreEqual(123, data.Col);
        }
    }
}
