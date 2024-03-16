using Dapper;
using Microsoft.Data.SqlClient;


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

        public async Task<IList<ExampleSysObject>> ReadSysobjectsAsync()
        {
	        var sql = "Select * from sysobjects";
	        await using (var connection = new SqlConnection("Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
	        {
		        await connection.OpenAsync();
		        return (await connection.QueryAsync<ExampleSysObject>(sql)).ToList();
	        }
        }
	}
}
