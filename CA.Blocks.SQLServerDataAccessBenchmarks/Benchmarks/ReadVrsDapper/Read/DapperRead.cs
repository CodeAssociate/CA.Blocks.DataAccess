using Dapper;
using Microsoft.Data.SqlClient;


namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    public  class DapperReadTest
    {
        private string testSql = "Select id, name, xtype, crdate from sysobjects";

        public IList<ExampleSysObject> ReadSysobjects()
        {
            using (var connection = new SqlConnection("Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
            {
                connection.Open();
                return connection.Query<ExampleSysObject>(testSql).ToList();
            }
        }

        public async Task<IList<ExampleSysObject>> ReadSysobjectsAsync()
        {
	        await using (var connection = new SqlConnection("Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
	        {
		        await connection.OpenAsync();
		        return (await connection.QueryAsync<ExampleSysObject>(testSql)).ToList();
	        }
        }
	}
}
