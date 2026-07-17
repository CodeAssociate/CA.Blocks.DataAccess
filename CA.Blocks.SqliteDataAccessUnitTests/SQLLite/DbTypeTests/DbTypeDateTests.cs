using System;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeDateTests : UnitTestDataAccess, IDisposable
    {
        // SQLite does not have a storage class set aside for storing dates and/or times.Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values:
        // TEXT as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS").
        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL($" date('{data:o}') "));
        }
        public DbTypeDateTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("Date not null"));
            InsertTestDataSQL(DateTime.Now.Date);
            InsertTestDataSQL(DateTime.Now.AddDays(1).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(-1).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(100).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(-100).Date);
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
            Console.WriteLine(DataTableToText(ExecuteDataTable(cmd)));
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            DateTime testValue = DateTime.Now.Date;
            var cmd = CreateTextCommand(SelectTestDataSQL(), $"Where col = date(@testValue)");
            cmd.Parameters.Add(testValue.ToSqlParameter("@testValue"));
            // Note 
            // sqlite does not have a storage class set aside for storing dates and/or times.
            // the blocks wil use string in ISO-8601 format the use sqllite functions on those values
            // Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values: see https://www.sqlite.org/lang_datefunc.html,
            // Use the datetime('{datetime:o}') C# roundtrip function to get the string

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<DateTime>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.Single(data);
        }


    }
}






