using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite
{


    // Shows how to use the ExecuteObjectList to get a list of dyanmic objects from a SQL query.
    // This is handy for very quick development
    [TestFixture]
    public class SqlLiteLocalFIleDataAccessTests : LocalFileUnitTestDataAccess
    {

        [SetUp]
        public void CreateMasterTestTable()
        {
            var cmd = CreateTextCommand("create table if not exists Test1 (id int identity(1,1), col int)");
            ExecuteNonQuery(cmd);
        }

        public IList<sqliteMaster> GetSqlliteMasterObjects()
        {
	        var cmd = CreateTextCommand("Select * from sqlite_master");
	        return Execute(cmd).ToListOf<sqliteMaster>();
		}


        [Test]
        public void GetsqliteMasterData()
        {
	        var results = GetSqlliteMasterObjects();

			foreach (var o in results)
            {
                TestContext.WriteLine($"{o.name},{o.type},{o.rootpage},{o.sql}");
            }
        }

        [Test]
        public void CreateDataTest()
        {
            var sql = "Insert into Test1(col) values (@i)"; 
            for (int i = 1; i< 100; i++)
            {
                var cmd = CreateTextCommand(sql);
                cmd.Parameters.Add(i.ToSqlParameter("@i"));
                ExecuteNonQuery(cmd);
            }

        }

    }
}
