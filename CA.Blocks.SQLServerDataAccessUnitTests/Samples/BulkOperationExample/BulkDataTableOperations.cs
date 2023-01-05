using System.Diagnostics;
using CA.Blocks.DataAccess.DataTableHelpers;
using NUnit.Framework;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccessTestDataForUnitTests.TestSets.DateDimension;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.BulkOperationExample
{


    [TestFixture]
    public class BulkDataTableOperations : SqlServerDataAccess
    {
        public BulkDataTableOperations() : base(new SimpleConnectionStringDataAccessConfig(
            "Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))

        {

        }

        private string DropTestTableIfExistsSQL()
        {
            return @"
If Exists( Select * from  INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CABLOCKS_DateDimension_Example')
BEGIN
	drop table CABLOCKS_DateDimension_Example
END";
        }

        private string DropTestTableTypeIfExistsSQL()
        {
            return @"
If Exists( Select * from sys.table_types where name = 'CABLOCKS_DateDimension_Example_type')
BEGIN
	drop type dbo.CABLOCKS_DateDimension_Example_type
END
";
        }


        private string CreateTestTableSQL()
        {
            return @"
Create Table CABLOCKS_DateDimension_Example 
(
    [Date] Date not null PRIMARY KEY,
    [DateKey] varchar(8) not null,
    [Year]  smallint not null,
    [Month]  tinyint not null,
    [Day]  tinyint not null,
    [DayOfWeek]  tinyint not null,
    [DayOfYear]    smallint not null,
    [Quarter] char(2) not null,
    [QuarterKey]  varchar(6) not null,

    [MonthKey]  varchar(6) not null,
    [MonthShortName]  varchar(4) not null,
    [MonthName]   varchar(16) not null,
    [DayName] varchar(16) not null
)";
        }

        private string CreateTestTableTypeSQL()
        {
            return @"
Create Type dbo.CABLOCKS_DateDimension_Example_type as Table
(
    [Date] Date not null,
    [DateKey] varchar(8) not null,
    [Year]  smallint not null,
    [Month]  tinyint not null,
    [Day]  tinyint not null,
    [DayOfWeek]  tinyint not null,
    [DayOfYear]    smallint not null,
    [Quarter] char(2) not null,
    [QuarterKey]  varchar(6) not null,

    [MonthKey]  varchar(6) not null,
    [MonthShortName]  varchar(4) not null,
    [MonthName]   varchar(16) not null,
    [DayName] varchar(16) not null
)";
        }

        [OneTimeSetUp]
        public void Setup()
        {
            ExecuteNonQuery(CreateTextCommand(DropTestTableIfExistsSQL()));
            ExecuteNonQuery(CreateTextCommand(DropTestTableTypeIfExistsSQL()));

            ExecuteNonQuery(CreateTextCommand(CreateTestTableSQL()));
            ExecuteNonQuery(CreateTextCommand(CreateTestTableTypeSQL()));

        }

        [OneTimeTearDown]
        public void TearDown()
        {
           ExecuteNonQuery(CreateTextCommand(DropTestTableIfExistsSQL()));
           ExecuteNonQuery(CreateTextCommand(DropTestTableTypeIfExistsSQL()));
        }



        [Test, Order(1)]
        public void BulkInsertTest()
        {
            // This process inserts 365,243 rows 1000 years of date data. which is 4,748,159 cells, on local with fast desk this is 5 seconds
            var builder = new DateDimensionBuilder();
            var bulksql = @"insert into CABLOCKS_DateDimension_Example select * from @BulkInsertParam";
            var sw = new Stopwatch();

            for (int year = 2000; year < 3000; year+=10)
            {
                sw.Reset();
                var testData = builder.GenerateDateDimensions(year, year +9);
                Assert.IsNotNull(testData);

                sw.Start();
                var dt = testData.ToObjectDataTable();
                var dtgGenTime  = sw.ElapsedMilliseconds;

                var cmd = CreateTextCommand(bulksql)
                    .WithParameter(dt.ToDataTableSqlParameter("@BulkInsertParam",
                        "dbo.CABLOCKS_DateDimension_Example_type"));

                ExecuteNonQuery(cmd);
                sw.Stop();
                var executeTime = sw.ElapsedMilliseconds - dtgGenTime;
                TestContext.WriteLine($"Inserted {dt.Rows.Count} -> ObjectToDTTimems={dtgGenTime};DbExecuteTimems={executeTime}");
            }

            var countcmd = CreateTextCommand("Select count(*) from CABLOCKS_DateDimension_Example");
            var rowCount = ExecuteScalarAs<int>(countcmd);
            Assert.AreEqual(365243, rowCount);
        }
    }
}
