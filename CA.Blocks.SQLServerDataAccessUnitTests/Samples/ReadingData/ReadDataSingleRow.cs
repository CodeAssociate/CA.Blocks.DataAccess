using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CA.Blocks.DataAccess.DI;
using NUnit.Framework;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    [TestFixture]
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
                new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
            )
            {

            }


            public ExampleSysObject2 GetSysObjectByName()
            {
                var cmd = CreateTextCommand("Select  id, name, refdate  from Sysobjects where name = 'sysobjects'");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }

            public ExampleSysObject2 GetSysObjectByName2()
            {
                var cmd = CreateTextCommand("Select *from Sysobjects where name = 'sysobjects'");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }
        }



        [Test]
        public void GetSysObjectByName()
        {
            var target = new ExampleReadDataSingleRow();
            var executeResult = target.GetSysObjectByName();

            TestContext.WriteLine($"{executeResult.id},{executeResult.name},{executeResult.refdate}");

            var executeResult2 = target.GetSysObjectByName2();

            TestContext.WriteLine($"{executeResult2.id},{executeResult2.name},{executeResult2.refdate}");
        }

    }
}