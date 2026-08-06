using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    public class BlocksReadTest : SqlServerDataAccess
    {
        private string testSql = "Select id, name, xtype, crdate from sysobjects";
        public BlocksReadTest() : base(
            new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                new HardCodedConnectionStringsResolver(
                    "Server=(local);Database=master;Integrated Security=SSPI;TrustServerCertificate=True"))
        )
        {

        }


        public IList<ExampleSysObject> ReadSysObjectsSync()
        {
            var cmd = CreateDbCommand(testSql);
            return Execute(cmd).ToListOf<ExampleSysObject>();
        }

        public IList<ExampleSysObject> ReadSysObjectsSyncDispose()
        {
            // here we clean up our mess better memoy management, but it is not the fastest way to do it
            using (var cmd = CreateDbCommand(testSql))
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

        public IList<ExampleSysObject> ReadSysObjectsSyncWithCustom()
        {
            var cmd = CreateDbCommand(testSql);
            return Execute(cmd).ToListOf<ExampleSysObject>(CustomT);
        }


        private ExampleSysObject FastestCustomT(IDataReader dr)
        {
            return new ExampleSysObject
            {
                id = dr.AsInt(0),
                name = dr.AsString(1),
                xtype = dr.AsString(2),
                crdate = dr.AsDateTime(3)
            };
        }

        public IList<ExampleSysObject> ReadSysObjectsSyncWithIndexedCustom()
        {
            var cmd = CreateDbCommand(testSql);
            return Execute(cmd).ToListOf<ExampleSysObject>(FastestCustomT);
        }


        public async Task<IList<ExampleSysObject>> ReadSysObjectsASync()
        {
            var cmd = CreateDbCommand(testSql);
            return await ExecuteAsync(cmd).ToListOf<ExampleSysObject>();
        }

        public async Task<IList<ExampleSysObject>> ReadSysObjectsASyncWithReaderAsync()
        {
            var cmd = CreateDbCommand(testSql);
            return await ExecuteAsync(cmd).ToListOfAsync<ExampleSysObject>();
        }


        public async Task<IList<ExampleSysObject>> ReadSysObjectsASyncWithCustom()
        {
            var cmd = CreateDbCommand(testSql);
            return await ExecuteAsync(cmd).ToListOf<ExampleSysObject>(CustomT);
        }

        public async Task<IList<ExampleSysObject>> ReadSysObjectsASyncWithDispose()
        {
            var cmd = CreateDbCommand(testSql);

            return await ExecuteAsync(cmd).ToListOfAsync<ExampleSysObject>();
        }

        public async Task<IList<ExampleSysObject>> ReadSysobjectsAsyncAsyncFetchSyncRead()
        {
            // this is the fastest way to read data async ?
            var cmd = CreateDbCommand(testSql);
            IDataReader r = await ExecuteAsync(cmd);
            return r.ToListOf<ExampleSysObject>();
        }
	}
}
