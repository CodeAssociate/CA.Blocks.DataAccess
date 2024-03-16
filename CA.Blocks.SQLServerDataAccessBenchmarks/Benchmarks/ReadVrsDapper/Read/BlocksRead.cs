using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    public class BlocksReadTest : SqlServerDataAccess
    {
        public BlocksReadTest() : base(
            new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                new HardCodedConnectionStringsResolver(
                    "Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
        )
        {

        }

        public IList<ExampleSysObject> ReadSysobjects()
        {
            var cmd = CreateTextCommand("Select * from sysobjects");
            return Execute(cmd).ToListOf<ExampleSysObject>();

        }

        public IList<ExampleSysObject> ReadSysobjectsDispose()
        {
	        using (var cmd = CreateTextCommand("Select * from sysobjects"))
	        {
		        return Execute(cmd).ToListOf<ExampleSysObject>();
			}
        }

		private ExampleSysObject CustomT(IDataReader dr)
        {
            return  new ExampleSysObject
            {
                id = dr.AsInt("id"),
                name = dr.AsString("name"),
                xtype = dr.AsString("xtype"),
                crdate = dr.AsDateTime("crdate")
            };
        }

        public IList<ExampleSysObject> ReadSysobjects2()
        {
            var cmd = CreateTextCommand("Select * from sysobjects");
            return Execute(cmd).ToListOf<ExampleSysObject>(CustomT);
        }


        public async Task<IList<ExampleSysObject>> ReadSysobjectsAsync()
        {
	        var cmd = CreateTextCommand("Select * from sysobjects");
	        return await ExecuteAsync(cmd).ToListOf<ExampleSysObject>();
        }

	}
}
