using System;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    [TestFixture]
    public class SqlServerDataAccessExecuteScalarTests : SqlServerDataAccess
    {
        public SqlServerDataAccessExecuteScalarTests() 
            : base( new DataAccessConfig(
                    new DataAccessConfigOptions { ConnectionStringKey = "localsqlserverhost" },
                    new LocalSqlServerUnitTestStringsResolver()))
        {
   
        }


        [Test]
        public void ExecuteExecuteScalarByte()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as tinyint) as col");
            // act
            var result = ExecuteScalarAs<byte>(cmd);
            //Assert
            Assert.AreEqual((byte)1, result);
        }

        [Test]
        public void ExecuteExecuteScalarShort()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as smallint) as col");
            // act
            var result = ExecuteScalarAs<short>(cmd);
            //Assert
            Assert.AreEqual((short)1, result);
        }

        [Test]
        public void ExecuteExecuteScalarInt()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as int) as col");
            // act
            var result = ExecuteScalarAs<int>(cmd);
            //Assert
            Assert.AreEqual((int)1, result);
        }


        [Test]
        public void ExecuteExecuteScalarIntAsync()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as int) as col");
            // act
            var result = ExecuteScalarAsAsync<int>(cmd);
            TestContext.WriteLine(result.Status);
            result.Wait();
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            //Assert
            Assert.AreEqual((int)1, result.Result);
        }


        [Test]
        public void ExecuteExecuteScalarNullInt()
        {
            // Setup
            var cmd = CreateTextCommand("Select null as col");
            // act
            var result = ExecuteScalarAs<int?>(cmd);
            //Assert
            Assert.IsNull(result);
        }


        [Test]
        public void ExecuteExecuteScalarIntWithConvert()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
            // act
            var result = ExecuteScalarWithConvertAs<int>(cmd);
            //Assert
            Assert.AreEqual((int)123, result);
        }


        [Test]
        public void ExecuteExecuteScalarLong()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast(1 as bigint) as col");
            // act
            var result = ExecuteScalarAs<long>(cmd);
            //Assert
            Assert.AreEqual((long)1, result);
        }


        [Test]
        public void ExecuteExecuteScalarGuid()
        {
            // Setup
            var cmd = CreateTextCommand("Select Cast('D79DB3C0-E5BE-4045-A37B-6DB923D37123' as uniqueidentifier) as col");
            // act
            var result = ExecuteScalarAs<Guid>(cmd);
            //Assert
            Assert.AreEqual(new Guid("D79DB3C0-E5BE-4045-A37B-6DB923D37123"), result);
        }


        [Test]
        public void ExecuteExecuteScalarString()
        {
            // Setup
            var cmd = CreateTextCommand("Select 'String Value' as col ");
            // act
            var result = ExecuteScalarAsString(cmd);
            //Assert
            Assert.AreEqual("String Value", result);
        }



    }
}
