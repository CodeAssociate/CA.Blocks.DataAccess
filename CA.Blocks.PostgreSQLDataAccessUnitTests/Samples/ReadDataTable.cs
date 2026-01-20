using CA.Blocks.DataAccess.DataTableHelpers;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccessTestDataForUnitTests.ConnectionStringResolver;
using CA.Blocks.PostgreSQLDataAccess;
using NUnit.Framework;
using System.Data;


namespace CA.Blocks.PostgreSQLDataAccessUnitTests.Samples
{
    [TestFixture]
    public class ReadDataTable
    {

        public class PgTables
        {
            public string schemaname { get; set; }
            public string tablename { get; set; }
            public string tableowner { get; set; }
            public string? tablespace { get; set; }
            public bool hasindexes { get; set; }
            public bool hasrules { get; set; }
            public bool hastriggers { get; set; }
            public bool rowsecurity { get; set; }
        }

        public class ReadDataTableDataAccess : PostgresDataAccess
        {
          
            public ReadDataTableDataAccess() : base(
                new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                        new LocalFileConnectionStringResolver("PostgresDataAccessConnectionString.txt"))
            )
            {
            }

            public async Task<IList<PgTables>> GetInformationSchema()
            {
                var cmd = CreateTextCommand("select * from pg_catalog.pg_tables");
                return await ExecuteAsync(cmd).ToListOf<PgTables>();
            }
        }



        [Test]
        public async Task GetGetInformationSchema()
        {
            var target = new ReadDataTableDataAccess();
            var executeResult = await target.GetInformationSchema();
            foreach (var item in executeResult)
            {
                TestContext.WriteLine($"{item.schemaname}.{item.tablename} owned by {item.tableowner} (hasindexes={item.hasindexes},hastriggers={item.hastriggers})");
            }
            //TestContext.WriteLine(DataTableToTextHelper.OutPutAsAlignedText(executeResult));
        }

    }
}
