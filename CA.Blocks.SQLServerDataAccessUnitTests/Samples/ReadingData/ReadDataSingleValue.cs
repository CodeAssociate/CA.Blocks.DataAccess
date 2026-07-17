using System;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    public class ReadDataSingleValue
    {

        public class ExampleReadDataSingleValue: SqlServerDataAccess
        {
            public ExampleReadDataSingleValue() : base(
                new DataAccessConfig( new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new HardCodedConnectionStringsResolver(TestConnectionStrings.LOCAL_TEMP_DB))
            )
            {

            }

            public object GetSysObjectsCountReturnObject()
            {
                var cmd = CreateTextCommand("Select count(*) from Sysobjects");
                return ExecuteScalar(cmd);
            }

            public int GetSysObjectsCount()
            {
                var cmd = CreateTextCommand("Select count(*) from Sysobjects");
                return ExecuteScalarAs<int>(cmd);
            }

            public int? GetValueThatMightBeNull()
            {
                var cmd = CreateTextCommand("Select id from Sysobjects where 1=2"); // zero rows
                return ExecuteScalarAs<int?>(cmd);
            }
            public int? GetValueThatMightBeNull2()
            {
                var cmd = CreateTextCommand("Select null as col"); // 1 row value null
                return ExecuteScalarAs<int?>(cmd);
            }

            public int GetValueThatMustBeConverted()
            {
                var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
                return ExecuteScalarWithConvertAs<int>(cmd);
            }

            public DateTime GetDateTimeValue()
            {
                var cmd = CreateTextCommand("Select Getdate() as col");
                return ExecuteScalarAs<DateTime>(cmd);
            }
        }



        [Fact]
        public void GetSysObjectsCount()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetSysObjectsCount();

            Console.WriteLine($"{executeResult}");
        }

        [Fact]
        public void GetSysObjectsCountReturnObject()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetSysObjectsCountReturnObject();

            Console.WriteLine($"{executeResult}");
        }


        [Fact]
        public void GetValueThatMightBeNull()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetValueThatMightBeNull();
            Assert.Null(executeResult);

        }

        [Fact]
        public void GetValueThatMightBeNull2()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetValueThatMightBeNull2();
            Assert.Null(executeResult);

        }

        [Fact]
        public void GetValueThatMustBeConverted()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetValueThatMustBeConverted();
            Console.WriteLine($"{executeResult}");

        }

        [Fact]
        public void GetDateTimeValue()
        {
            var target = new ExampleReadDataSingleValue();
            var executeResult = target.GetDateTimeValue();
            Console.WriteLine($"{executeResult}");

        }
    }
}



