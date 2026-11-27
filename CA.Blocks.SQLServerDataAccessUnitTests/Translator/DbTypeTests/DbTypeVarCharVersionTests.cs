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
        public required Version Col { get; init; }
    }


    private void InsertTestData(string data)
    {
        ExecuteNonQuery(InsertTestDataCommand(data));
    }

    public DbTypeVarCharVersionTests()
        {
        ExecuteNonQuery(DropTestTableCommand());
        var create = CreateTestTableCommand("varchar", allowNull: true);
        create.CommandText = create.CommandText.Replace("[varchar]", "[varchar](20)");
        ExecuteNonQuery(create);
        InsertTestData(new Version(1,2).ToString());
        InsertTestData(new Version(1, 2,3).ToString());
        InsertTestData(new Version(1, 2, 3, 4).ToString());
        ExecuteNonQuery(InsertTestDataCommand<string?>(null));
    }

    public void Dispose()
        {
        ExecuteNonQuery(DropTestTableCommand());
    }

    [Fact]
    public void SelectAllData()
    {
        //Setup 
        var cmd = BasicSelectCommand();
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
        var value = testvalue.ToString();
        var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {value:@testValue}");
        cmd.Parameters.RemoveAt("@testValue");
        cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToFirstOrDefault<VersionDataType>();
        //Asert
        Assert.Equal(data.Col, testvalue);
    }
}



