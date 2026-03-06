using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccess.Builder;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;


namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [TestFixture]
    public class DbTypeBigIntArrayTests : UnitTestDataAccess
    {
        private class BigIntArrayDataType
        {
            public List<long> Col { get; set; }
        }

        private void InsertTestDataSQL(long[] data)
        {
            var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``} (col) values({data:@Data})")
                .BuildSqlCommand();
            ExecuteNonQuery(insertCmd);
        }
        [SetUp]
        public void Setup()
        {
            DefaultDbColToTypeProviderPostgresExtensions.AddPostgresArrayTypes();
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint[] not null"));
            InsertTestDataSQL([1, 2, 3]);
            InsertTestDataSQL([1, 3, 5]);
            InsertTestDataSQL([2, 4, 8]);
            InsertTestDataSQL([(long)int.MaxValue + (long)int.MaxValue, (long)int.MaxValue]);
        }

        [TearDown]
        public void TearDown()
        {
            //ExecuteNonQuery(DropTestTableSQL());
        }


        [Test]
        public void SelectAllDataToDataTable()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.ExecuteDataTable (cmd);
            //Assert
            Assert.That(data.Rows.Count, Is.EqualTo(4));
            Assert.That(data.Rows[1]["Col"], Is.EqualTo(new List<long> { 1, 3, 5 }));
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BigIntArrayDataType>(cmd);
            //Assert
            Assert.That(data.Count, Is.EqualTo(4));
            Assert.That(data[1].Col, Is.EqualTo(new List<long> { 1, 3, 5 }));
        }


        /*
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
        */
    }
}
