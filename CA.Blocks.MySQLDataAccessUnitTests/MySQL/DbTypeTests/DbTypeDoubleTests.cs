using System;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
public class DbTypeDoubleTests : UnitTestDataAccess, IDisposable
    {

        private class DoubleDataType
        {
            public Double Col { get; set; }
        }


        private void InsertTestDataSQL(double data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"{data}"));
        }

        public DbTypeDoubleTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("double not null"));
            InsertTestDataSQL(-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL(123456789.987654321);
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
            var data = ExecuteDataTable(cmd);
            //Assert
            Assert.Equal(5, data.Rows.Count);
        }
        [Fact]
public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<DoubleDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2, data[0].Col);
            Assert.Equal(123456789.987654321, data[4].Col);
        }
        [Fact]
public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<DoubleDataType>(cmd);

            //Asert
            Assert.Equal(3, data.Count);
        }


    }
}


