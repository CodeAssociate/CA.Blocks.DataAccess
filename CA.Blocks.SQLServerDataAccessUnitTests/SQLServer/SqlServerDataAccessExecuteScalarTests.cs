using System;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    public class SqlServerDataAccessExecuteScalarTests : SqlServerDataAccess
    {
        public SqlServerDataAccessExecuteScalarTests() 
            : base( new DataAccessConfig(
                    new DataAccessConfigOptions { ConnectionStringKey = "localsqlserverhost" },
                    new LocalSqlServerUnitTestStringsResolver()))
        {
   
        }


        [Fact]
        public void ExecuteExecuteScalarByte()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as tinyint) as col");
            // act
            var result = ExecuteScalarAs<byte>(cmd);
            //Assert
            Assert.Equal((byte)1, result);
        }

        [Fact]
        public void ExecuteExecuteScalarShort()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as smallint) as col");
            // act
            var result = ExecuteScalarAs<short>(cmd);
            //Assert
            Assert.Equal((short)1, result);
        }

        [Fact]
        public void ExecuteExecuteScalarInt()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as int) as col");
            // act
            var result = ExecuteScalarAs<int>(cmd);
            //Assert
            Assert.Equal((int)1, result);
        }


        [Fact]
        public void ExecuteExecuteScalarIntAsync()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as int) as col");
            // act
            var result = ExecuteScalarAsAsync<int>(cmd);
            Console.WriteLine(result.Status);
            result.Wait();
            Assert.Equal(TaskStatus.RanToCompletion, result.Status);
            //Assert
            Assert.Equal((int)1, result.Result);
        }


        [Fact]
        public void ExecuteExecuteScalarNullInt()
        {
            // Setup
            var cmd = CreateTextCommand("Select null as col");
            // act
            var result = ExecuteScalarAs<int?>(cmd);
            //Assert
            Assert.Null(result);
        }


        [Fact]
        public void ExecuteExecuteScalarIntWithConvert()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
            // act
            var result = ExecuteScalarWithConvertAs<int>(cmd);
            //Assert
            Assert.Equal((int)123, result);
        }


        [Fact]
        public void ExecuteExecuteScalarLong()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as bigint) as col");
            // act
            var result = ExecuteScalarAs<long>(cmd);
            //Assert
            Assert.Equal((long)1, result);
        }


        [Fact]
        public void ExecuteExecuteScalarGuid()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast('D79DB3C0-E5BE-4045-A37B-6DB923D37123' as uniqueidentifier) as col");
            // act
            var result = ExecuteScalarAs<Guid>(cmd);
            //Assert
            Assert.Equal(new Guid("D79DB3C0-E5BE-4045-A37B-6DB923D37123"), result);
        }


        [Fact]
        public void ExecuteExecuteScalarString()
        {
            // Setup
            var cmd = CreateTextCommand("Select 'String Value' as col ");
            // act
            var result = ExecuteScalarAsString(cmd);
            //Assert
            Assert.Equal("String Value", result);
        }



    }
}




