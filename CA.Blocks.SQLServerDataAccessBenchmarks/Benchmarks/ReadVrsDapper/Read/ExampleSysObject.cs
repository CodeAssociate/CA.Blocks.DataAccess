using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    public class ExampleSysObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public string xtype { get; set; }
        public DateTime crdate { get; set; }
    }
}
