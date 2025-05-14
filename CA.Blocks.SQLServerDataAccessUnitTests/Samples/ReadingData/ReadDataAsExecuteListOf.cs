using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
using NUnit.Framework;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess.Builder;

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

        public class SpWhoResult
        {
            public short spid { get; init; }
            public short ecid { get; init; }
            public string status { get; init; }
            public string loginame { get; init; }
            public string hostname { get; init; }
            public string blk { get; init; }
            public string dbname { get; init; }
            public string cmd { get; init; }
            public int request_id { get; init; }
        }

        public class ExampleReadDataAsExecuteListOf : SqlServerDataAccess
        {
            public ExampleReadDataAsExecuteListOf() : base( new SimpleConnectionStringDataAccessConfig(TestConnectionStrings.LOCAL_TEMP_DB))
            
            {

            }
            public IList<SpWhoResult> ExecSpWho()
            {
                var cmd = CreateStoredProcedureCommand("sp_Who");
                return Execute(cmd).ToListOf<SpWhoResult>();
            }

            // Move to performance benchmark
            public IList<SpWhoResult> ExecSpWhoAdonet()
            {
                var result = new List<SpWhoResult>();
                using (var connection = new SqlConnection("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
                {
                    connection.Open();
                    using (var command = new SqlCommand("Exec sp_who", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                
                                var product = new SpWhoResult
                                {
                                    spid = reader.GetInt16(reader.GetOrdinal("spid")),
                                    ecid = reader.GetInt16(reader.GetOrdinal("ecid")),
                                    status = reader.GetString(reader.GetOrdinal("status")),
                                    loginame = reader.GetString(reader.GetOrdinal("loginame")),
                                    hostname = reader.GetString(reader.GetOrdinal("hostname")),
                                    blk = reader.GetString(reader.GetOrdinal("blk")),
                                    dbname = reader.IsDBNull(reader.GetOrdinal("dbname")) ? null : reader.GetString(reader.GetOrdinal("dbname")),
                                    cmd = reader.GetString(reader.GetOrdinal("cmd")),
                                    request_id = reader.GetInt32(reader.GetOrdinal("request_id"))
                                };
                                result.Add(product);
                            }
                        }
                    }
                    connection.Close();
                }
                return result;
            }

            internal string ReadSysObjectsOfTypeSql => @"
SELECT  TOP 10 id as Id, name as Name, xtype as XType, crdate as CreateDate 
FROM  sysobjects 
WHERE xtype = @xtype";


			public IList<ExampleSysObject> ReadSysObjectsOfType(string xtype)
            {
                var cmd = CreateTextCommand(ReadSysObjectsOfTypeSql)
	                .WithParameter(xtype.ToSqlParameter("@xtype"));
                return Execute(cmd).ToListOf<ExampleSysObject>();
            }


            public IList<ExampleSysObject> ReadSysObjectsOfTypeInterpolatedString(string xtype)
            {
                var sqlBuilder = new SafeSqlBuilder();
                sqlBuilder.AddSql($"SELECT  TOP 10 id as Id, name as Name, xtype as XType, crdate as CreateDate FROM sysobjects WHERE xtype = {xtype:@xtype}");

                return Execute(sqlBuilder.BuildSqlCommand()).ToListOf<ExampleSysObject>();
            }



            public async Task<IList<ExampleSysObject>> ReadSysObjectsOfTypeUsingAsync(string xtype)
            {
                var cmd = CreateTextCommand(ReadSysObjectsOfTypeSql)
	                .WithParameter(xtype.ToSqlParameter("@xtype"));
                return await ExecuteAsync(cmd).ToListOf<ExampleSysObject>();
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
                var cmd = CreateTextCommand("Select top 1 * from Sysobjects");
                return ExecuteTo<ExampleSysObject2>(cmd);
            }

        }

        [Test]
        public void ExecuteSpWho()
        {
            var target = new ExampleReadDataAsExecuteListOf();
            var executeResult = target.ExecSpWho();
            foreach (var o in executeResult)
            {
                TestContext.WriteLine($"{o.spid},{o.ecid},{o.status},{o.loginame},{o.hostname},{o.blk},{o.dbname},{o.cmd},{o.request_id}");
            }
        }

        [Test]
        public void ExecuteSpWhoAdoNet()
        {
            var target = new ExampleReadDataAsExecuteListOf();
            var executeResult = target.ExecSpWhoAdonet();
            foreach (var o in executeResult)
            {
                TestContext.WriteLine($"{o.spid},{o.ecid},{o.status},{o.loginame},{o.hostname},{o.blk},{o.dbname},{o.cmd},{o.request_id}");
            }
        }

        [Test]
        public void ExecuteToListOfDev()
        {
            var target = new ExampleReadDataAsExecuteListOf();
            var executeResult = target.ReadSysObjectsOfType("S");

            foreach (var o in executeResult)
            {
                TestContext.WriteLine($"{o.Id},{o.Name},{o.XType},{o.CreateDate}");
            }

            var executeResult2 = target.ReadSysObjectsOfType2("S");

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

        private long GetSysObjectByNameSyncInterpolatedString(ExampleReadDataAsExecuteListOf target, bool printResults)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var executeResult = target.ReadSysObjectsOfTypeInterpolatedString("U");
            sw.Stop();
            if (printResults)
            {
                printResultsOut(executeResult);
            }
            return sw.ElapsedTicks / 10;
        }


        private async Task<long> GetSysObjectByNameAsyncSync(ExampleReadDataAsExecuteListOf target, bool printResults)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var executeResult = await target.ReadSysObjectsOfTypeUsingAsync("U");
            sw.Stop();
            if (printResults)
            {
                printResultsOut(executeResult);
            }

            return sw.ElapsedTicks / 10;
        }


        [Test]
        public async Task GetSysObjectByNameSyncVsrAsync()
        {

            var target = new ExampleReadDataAsExecuteListOf();
            GetSysObjectByNameSync(target, true);
            await GetSysObjectByNameAsyncSync(target, true);

            for (int i = 1; i < 10; i++)
            {
                var syncTime = GetSysObjectByNameSync(target, false);
                var asyncTime = await GetSysObjectByNameAsyncSync(target, false);

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

        [Test]
        public void GetSysObjectByNameInterpolatedString()
        {

            var target = new ExampleReadDataAsExecuteListOf();
            GetSysObjectByNameSync(target, true);
            GetSysObjectByNameSyncInterpolatedString(target, true);

            for (int i = 1; i < 10; i++)
            {
                var directTime = GetSysObjectByNameSync(target, false);
                var interpolatedTime = GetSysObjectByNameSyncInterpolatedString(target, false);

                if (directTime < interpolatedTime)
                {
                    TestContext.WriteLine($"try {i} InterpolatedWinner - {directTime} vrs {interpolatedTime}");
                }
                else
                {
                    TestContext.WriteLine($"try {i} DirectWinner - {directTime} vrs {interpolatedTime}");
                }
            }
        }
    }
}