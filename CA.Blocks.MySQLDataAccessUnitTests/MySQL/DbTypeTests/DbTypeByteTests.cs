using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
[Collection("MySQLDbTypeTests")]
public class DbTypeByteTests : UnitTestDataAccess, IDisposable
    {
        private class ByteDataType
        {
            public sbyte Col { get; set; }
        }

        private void InsertTestDataSQL(sbyte data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        public DbTypeByteTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("tinyint not null"));
            InsertTestDataSQL(0);
            InsertTestDataSQL(1);
            InsertTestDataSQL(2);
            InsertTestDataSQL(4);
            InsertTestDataSQL(127);
            InsertTestDataSQL(-128);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }
        [Fact]
public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            Assert.Equal(6, data.Count);
        }
        [Fact]
public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            Assert.Equal(6, data.Count);
            Assert.Equal(127, data[4].Col);
            Assert.Equal(-128, data[5].Col);
        }
        [Fact]
public void SelectAllDataByteWithFilter ()
        {
            //setup
            const byte testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);

            //Asert
            Assert.Equal(1, data.Count);
        }


    }
}



