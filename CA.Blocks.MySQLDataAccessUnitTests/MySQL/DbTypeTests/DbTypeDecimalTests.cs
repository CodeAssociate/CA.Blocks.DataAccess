using System;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
[Collection("MySQLDbTypeTests")]
public class DbTypeDecimalTests : UnitTestDataAccess, IDisposable
    {

        private class DecimalDataType
        {
            public Decimal Col { get; set; }
        }


        private void InsertTestDataSQL(double data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"{data}"));
        }

        public DbTypeDecimalTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("decimal(20,10) not null"));
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
            var data = ExecuteToListOf<DecimalDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1.2m, data[0].Col);
            Assert.Equal(123456789.9876543300m, data[4].Col);
        }
        [Fact]
public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<DecimalDataType>(cmd);

            //Asert
            Assert.Equal(3, data.Count);
        }


    }
}



