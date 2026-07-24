using CA.Blocks.DataAccess.DataTableHelpers;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.MySQLDataAccess;
using System.Data;
using CA.Blocks.MySQLDataAccessUnitTests.Base;

namespace CA.Blocks.MySQLDataAccessUnitTests.Samples;

public class ReadDataTableDataAccess : UnitTestDataAccess
{
    public DataTable GetInformationSchema()
    {
        var cmd = CreateTextCommand("select *  from information_schema.tables");
        return ExecuteDataTable(cmd);
    }
}
[Collection("MySQLDbTypeTests")]
public class ReadDataTableDataAccessTests
{
    [Fact]
    public void GetGetInformationSchema()
    {
        var target = new ReadDataTableDataAccess();
        var executeResult = target.GetInformationSchema();
        Console.WriteLine(DataTableToTextHelper.OutPutAsAlignedText(executeResult));
    }
}

