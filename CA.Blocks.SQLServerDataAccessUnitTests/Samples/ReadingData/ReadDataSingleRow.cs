using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    public class ReadDataSingleRow
    {

        public class ExampleSysObject2
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime refdate { get; set; }
        }

        public class ExampleReadDataSingleRow : SqlServerDataAccess
        {
            public ExampleReadDataSingleRow() : base(
                new DataAccessConfig( new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new HardCodedConnectionStringsResolver(TestConnectionStrings.LOCAL_TEMP_DB))
            )
            {

            }


            public ExampleSysObject2 GetSysObjectByName()
            {
                var cmd = CreateTextCommand("Select top 1 id, name, refdate  from Sysobjects");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }

            public ExampleSysObject2 GetSysObjectByName2()
            {
                var cmd = CreateTextCommand("Select top 1 * from Sysobjects");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }
        }



        [Fact]
        public void GetSysObjectByName()
        {
            var target = new ExampleReadDataSingleRow();
            var executeResult = target.GetSysObjectByName();

            Console.WriteLine($"{executeResult.id},{executeResult.name},{executeResult.refdate}");

            var executeResult2 = target.GetSysObjectByName2();

            Console.WriteLine($"{executeResult2.id},{executeResult2.name},{executeResult2.refdate}");
        }

    }
}



