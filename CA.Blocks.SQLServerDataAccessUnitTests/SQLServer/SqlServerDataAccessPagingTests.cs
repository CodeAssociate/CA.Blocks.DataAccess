using System.Data;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using CA.Blocks.DataAccess.Model.Paging;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    public class SqlServerDataAccessPagingTests : UnitTestDataAccess
    {
        [Fact]
        public void GetBasicPagingRequest()
        {
            var target = new UnitTestDataAccess();
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            DataTable dt = target.ExecuteDataTable(cmd, new PagingRequest(10, 0, "ID"));
            Assert.Equal(10, dt.Rows.Count);
        }


        [Fact]
        public void GetBasicPagingRequest_NoSpecifiedOrder()
        {
            var target = new UnitTestDataAccess();
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            DataTable dt = target.ExecuteDataTable(cmd, new PagingRequest(10, 0));
            Assert.Equal(10, dt.Rows.Count);
        }
    }
}




