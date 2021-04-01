using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using CA.Blocks.DataAccess.DI;
using NUnit.Framework;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    [TestFixture]
    public class ReadDataAsExecuteListOf
    {
        public class ExampleSysObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string XType { get; set; }
            public DateTime CreateDate { get; set; }
        }

        public class ExampleSysObject2
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime refdate { get; set; }
        }

        public class ExampleReadDataAsExecuteListOf : SqlServerDataAccess
        {
            public ExampleReadDataAsExecuteListOf() : base(
                new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
            )
            {

            }

            public IList<ExampleSysObject> ReadSysObjectsOfType(string xtype)
            {
                var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype").WithParameter(xtype.ToSqlParameter("@xtype"));
                return ExecuteToListOf<ExampleSysObject>(cmd);
            }

            public IList<ExampleSysObject> ReadSysObjectsOfTypeUsingAsync(string xtype)
            {
                var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype").WithParameter(xtype.ToSqlParameter("@xtype"));
                var result = ExecuteToListOfAsync<ExampleSysObject>(cmd);
                result.Wait();
                return result.Result;
            }

            public IList<ExampleSysObject> ReadSysObjectsOfType2(string xtype)
            {
                var cmd = CreateTextCommand(
                        "Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype")
                    .WithParameters(new List<SqlParameter> {xtype.ToSqlParameter("@xtype")});
                return ExecuteToListOf<ExampleSysObject>(cmd);
            }

            public ExampleSysObject GetSysObjectById(int Id)
            {
                var cmd = CreateTextCommand("Select top 1 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where Id = @Id")
                    .WithParameters(new List<SqlParameter> { Id.ToSqlParameter("@Id") });
                return ExecuteTo<ExampleSysObject>(cmd);
            }

            public ExampleSysObject2 GetSysObjectByName()
            {
                var cmd = CreateTextCommand("Select * from Sysobjects where name = 'sysobjects'");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }

        }


        [Test]
        public void ExecuteToListOfDev()
        {
            var target = new ExampleReadDataAsExecuteListOf();
            var executeResult = target.ReadSysObjectsOfType("U");

            foreach (var o in executeResult)
            {
                TestContext.WriteLine($"{o.Id},{o.Name},{o.XType},{o.CreateDate}");
            }

            var executeResult2 = target.ReadSysObjectsOfType2("U");

            foreach (var o in executeResult)
            {
                TestContext.WriteLine($"{o.Id},{o.Name},{o.XType},{o.CreateDate}");
            }

            var sysObjectById = target.GetSysObjectById(executeResult2[0].Id);
            TestContext.WriteLine($"{sysObjectById.Id},{sysObjectById.Name},{sysObjectById.XType},{sysObjectById.CreateDate}");
        }


        [Test]
        public void GetSysObjectByName()
        {
            var target = new ExampleReadDataAsExecuteListOf();
            var executeResult = target.GetSysObjectByName();

            TestContext.WriteLine($"{executeResult.id},{executeResult.name},{executeResult.refdate}");
        }


        private void printResultsOut(IList<ExampleSysObject>  results)
        {
            foreach (var o in results)
            {
                TestContext.WriteLine($"{o.Id},{o.Name},{o.XType},{o.CreateDate}");
            }
        }


        private long GetSysObjectByNameSync(ExampleReadDataAsExecuteListOf target, bool printResults)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var executeResult = target.ReadSysObjectsOfType("U");
            sw.Stop();
            if (printResults)
            {
                printResultsOut(executeResult);
            }
            return sw.ElapsedTicks / 10;
        }

        private long GetSysObjectByNameAsyncSync(ExampleReadDataAsExecuteListOf target, bool printResults)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var executeResult = target.ReadSysObjectsOfTypeUsingAsync("U");
            sw.Stop();
            if (printResults)
            {
                printResultsOut(executeResult);
            }

            return sw.ElapsedTicks / 10;
        }


        [Test]
        public void GetSysObjectByNameSyncVsrAsync()
        {

            var target = new ExampleReadDataAsExecuteListOf();
            GetSysObjectByNameSync(target, true);
            GetSysObjectByNameAsyncSync(target, true);

            for (int i = 1; i < 10; i++)
            {
                var syncTime = GetSysObjectByNameSync(target, false);
                var asyncTime = GetSysObjectByNameAsyncSync(target, false);

                if (syncTime < asyncTime)
                {
                    TestContext.WriteLine($"try {i} SyncWinner - {syncTime} vrs {asyncTime}" );
                }
                else
                {
                    TestContext.WriteLine($"try {i} ASyncWinner - {syncTime} vrs {asyncTime}" );
                }

            }

        }

    }
}