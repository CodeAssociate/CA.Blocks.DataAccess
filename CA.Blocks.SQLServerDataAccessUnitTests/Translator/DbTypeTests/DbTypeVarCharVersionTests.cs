using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

    [Collection("DbIntegrationTests")]
public class DbTypeVarCharVersionTests : UnitTestDataAccess, IDisposable
{

    private class VersionDataType
    {
        public Version Col { get; set; }
    }


    private void InsertTestData(string data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
    }

    public DbTypeVarCharVersionTests()
        {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("varchar(20) null"));
        InsertTestData(new Version(1,2).ToString());
        InsertTestData(new Version(1, 2,3).ToString());
        InsertTestData(new Version(1, 2, 3, 4).ToString());
        ExecuteNonQuery(InsertTestDataSQL("null"));
    }

    public new void Dispose()
        {
        ExecuteNonQuery(DropTestTableSQL());
    }

    [Fact]
    public void SelectAllData()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act

        var data = Execute(cmd).ToListOf<VersionDataType>();
        //Assert
        Assert.Equal(4, data.Count);
        Assert.Equal(new Version("1.2"), data[0].Col);
        Assert.Equal(new Version("1.2.3"), data[1].Col);
        Assert.Equal(new Version("1.2.3.4"), data[2].Col);
        Assert.Null(data[3].Col);
    }



    [Fact]
    public void SelectDataWithFilter()
    {
        //setup
        var testvalue = new Version(1,2,3);
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
        cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToFirstOrDefault<VersionDataType>();
        //Asert
        Assert.Equal(data.Col, testvalue);
    }
}



