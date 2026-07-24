using System;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    [Collection("DbIntegrationTests")]
    public class ReadDataSingleRow
    {

        public class ExampleSysObject2
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime refdate { get; set; }
        }

        [Collection("DbIntegrationTests")]
        public class ExampleReadDataSingleRow : UnitTestDataAccess
        {
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



