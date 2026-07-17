using System.Collections.Generic;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer;

public class SqlServerParameterAsDataTableTests : SqlServerDataAccess
{

    public SqlServerParameterAsDataTableTests()
        : base(new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "localsqlserverhost" },
            new LocalSqlServerUnitTestStringsResolver()))
    {
    }

    private void EnsureTableTypeExists(string type, string spec)
    {
        var createType = $@"
If NOT Exists (Select * from sys.types where Name = '{GetTypeNameFor(type)}' and is_user_defined = 1 and is_table_type = 1)
BEGIN
	CREATE TYPE dbo.{GetTypeNameFor(type)} AS TABLE (Value {spec})
END";
        var cmd = CreateTextCommand(createType);
        ExecuteNonQuery(cmd);
    }

    private string GetTypeNameFor(string type)
    {
        return $"{type}ValueDataTable";
    }


    [Fact]
    public void TestWithIntArray()
    {
        // Setup
        var testList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        EnsureTableTypeExists("Int", "Int");


        var cmd = CreateTextCommand("Select Value from @testList");
        // In the Query Above we have specified a @testid Now pass in the parameter
        cmd.Parameters.Add(testList.ToValueDataTableSqlParameter("@testList", $"dbo.{GetTypeNameFor("Int")}"));

        // act
        var list = Execute(cmd).ToSingleNamedColumnList<int>("Value");
        Assert.Equal(testList.Count, list.Count);
    }

    [Fact]
    public void TestWithStringArray()
    {
        // Setup
        var testList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        EnsureTableTypeExists("String", "varchar(32)");


        var cmd = CreateTextCommand("Select Value from @testList");
        // In the Query Above we have specified a @testid Now pass in the parameter
        cmd.Parameters.Add(testList.ToValueDataTableSqlParameter("@testList", $"dbo.{GetTypeNameFor("String")}"));

        // act
        var list = Execute(cmd).ToSingleNamedColumnList<string>("Value");
        Assert.Equal(testList.Count, list.Count);
    }
}



