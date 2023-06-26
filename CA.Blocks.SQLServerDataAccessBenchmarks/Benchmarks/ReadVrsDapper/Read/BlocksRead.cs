using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using Microsoft.CodeAnalysis;

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
    }
}
