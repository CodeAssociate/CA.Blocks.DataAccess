using CA.Blocks.DataAccess.DataTableHelpers;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.MySQLDataAccess;
using System.Data;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.MySQLDataAccessUnitTests.Base;

namespace CA.Blocks.MySQLDataAccessUnitTests.Samples;

public class MyCustomObject
{
    public required string Name {get; init;}
    public required DateTime CreateDate {get; init;}
    public required string TableType {get; init;}
    public int? RowCount {get; init;}
}
public class ReadDataTableDataAccess : UnitTestDataAccess
{
    public DataTable GetInformationSchema()
    {
        var cmd = CreateTextCommand(@"
Select *
FROM information_schema.tables");
        return ExecuteDataTable(cmd);
    }
    
    public async Task<IList<MyCustomObject>> MyCustomObjects()
    {
        var cmd = CreateTextCommand(@"
SELECT TABLE_NAME as Name, CREATE_TIME as CreateDate, TABLE_TYPE as TableType, TABLE_ROWS as RowCount 
FROM information_schema.tables");
        return await ExecuteAsync(cmd).ToListOf<MyCustomObject>();
    }
}
[Collection("MySQLDbTypeTests")]
public class ReadDataTableDataAccessTests(ITestOutputHelper output)
{
    [Fact]
    public void GetGetInformationSchema()
    {
        var target = new ReadDataTableDataAccess();
        var executeResult = target.GetInformationSchema();
        output.WriteLine(DataTableToTextHelper.OutPutAsAlignedText(executeResult));
    }
    [Fact]
    public async Task GetMyCustomObjects()
    {
        var target = new ReadDataTableDataAccess();
        var result = await target.MyCustomObjects();
        foreach (MyCustomObject myCustomObject in result)
        {
            output.WriteLine(
                $"Table'{myCustomObject.Name}' created on {myCustomObject.CreateDate:F} has {myCustomObject.RowCount??0} rows");
        }
    }
}

