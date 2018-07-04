using System.Data;
using System.Data.SqlClient;
using CA.CoreBlocks.DataAccess.Paging;
using CA.CoreBlocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.CoreBlocks.SQLServerDataAccessUnitTests.SQLServer
{
    [TestFixture]
    public class SqlServerDataAccessPagingTests : UnitTestDataAccess
    {
        [Test]
        public void GetBasicPagingRequest()
        {
            var target = new UnitTestDataAccess();
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            DataTable dt = target.ExecuteDataTable(cmd, new PagingRequest(10, 0, "ID"));
            Assert.AreEqual(10, dt.Rows.Count);
        }
    }


}
