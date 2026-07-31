using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeDateTimeTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL($" datetime('{data:o}') "));
        }
        public DbTypeDateTimeTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTime not null"));
            InsertTestDataSQL(DateTime.Now.AddMinutes(1));
            InsertTestDataSQL(DateTime.Now.AddDays(1));
            InsertTestDataSQL(DateTime.Now.AddDays(-1));
            InsertTestDataSQL(DateTime.Now.AddDays(100));
            InsertTestDataSQL(DateTime.Now.AddDays(-100));
        }
        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataDateTime()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<DateTime>(UNIT_TEST_COL_NAME);
            //Assert
            Console.WriteLine(DataTableToText(Execute(cmd).ToDataTable()));
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            DateTime testvalue = DateTime.Now;
            //var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            var cmd = CreateTextCommand(SelectTestDataSQL(), $"Where col >= datetime('{testvalue:o}')");
            //cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<DateTime>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.True(data.Count == 3);
        }


    }
}





