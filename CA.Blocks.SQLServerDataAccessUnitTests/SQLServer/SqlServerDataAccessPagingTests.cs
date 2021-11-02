using System.Data;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using CA.Blocks.DataAccess.Model.Paging;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
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


        [Test]
        public void GetBasicPagingRequest_NoSpecifiedOrder()
        {
            var target = new UnitTestDataAccess();
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            DataTable dt = target.ExecuteDataTable(cmd, new PagingRequest(10, 0));
            Assert.AreEqual(10, dt.Rows.Count);
        }
    }
}
