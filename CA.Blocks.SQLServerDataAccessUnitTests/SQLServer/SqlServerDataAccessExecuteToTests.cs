using System.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    internal class temp
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    [TestFixture]
    public class SqlServerDataAccessExecuteTo : UnitTestDataAccess
    {


        [Test]
        public void ExecuteToListOfDev()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = ExecuteToListOf<temp>(cmd);
            foreach (var o in result)
            {
                TestContext.WriteLine($"{o.id},{o.name}");
            }
        }
    }

}
