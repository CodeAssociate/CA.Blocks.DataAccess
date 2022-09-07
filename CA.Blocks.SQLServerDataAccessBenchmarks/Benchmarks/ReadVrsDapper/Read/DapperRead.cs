using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    public  class DapperReadTest
    {

        public IList<ExampleSysObject> ReadSysobjects()
        {
            var sql = "Select * from sysobjects";
            using (var connection = new SqlConnection("Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
            {
                connection.Open();
                return connection.Query<ExampleSysObject>(sql).ToList();
            }
        }
    }
}
