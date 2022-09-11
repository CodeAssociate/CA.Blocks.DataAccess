using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.Extensions;

public class DataReaderExtensionsBaseTests
{
    protected class TestDataObject
    {
        public int IntCol { get; set; }
        public string StringCol { get; set; }
        public Guid GuidCol { get; set; }
        public DateTime DateCol { get; set; }
    }

    protected DataTable GenerateTestData(int count)
    {
        return GenerateTestData(1, count);
    }

    protected DataTable GenerateTestData(int start, int count)
    {
        var testData = new DataTable();
        testData.Columns.Add("IntCol", typeof(int));
        testData.Columns.Add("StringCol", typeof(string));
        testData.Columns.Add("GuidCol", typeof(Guid));
        testData.Columns.Add("DateCol", typeof(DateTime));
        testData.AcceptChanges();
        for (var i = start; i <= (start  + count - 1); i++)
        {
            testData.Rows.Add(i, $"row#{i}", Guid.NewGuid(), DateTime.Now.AddMinutes(i));
        }
        testData.AcceptChanges();
        return testData;
    }

    public DataSet GenerateTestSet(int tableCount, int count)
    {
        var result = new DataSet();
        for(int i = 0; i < tableCount; i++)
        {
            result.Tables.Add(GenerateTestData((i * count) + 1, count));
        }

        return result;
    }
    protected IDataReader GenerateTestDataSetReader(int tableCount, int count)
    {
        return GenerateTestSet(tableCount, count).CreateDataReader();
    }
    
    protected IDataReader GenerateTestDataReader(int count)
    {
        return GenerateTestData(count).CreateDataReader();
    }

    protected IDataReader GenerateTestDataReader(int start, int count)
    {
        return GenerateTestData(start, count).CreateDataReader();
    }

    protected async Task<DbDataReader> GenerateTestDataReaderAsync(int count)
    {
        await Task.Delay(1);
        return GenerateTestData(count).CreateDataReader();
    }

    protected async Task<DbDataReader> GenerateTestDataReaderAsync(int start, int count)
    {
        await Task.Delay(1);
        return GenerateTestData(start, count).CreateDataReader();
    }

    protected async Task<DbDataReader>GenerateTestDataSetReaderAsync(int tableCount, int count)
    {
        await Task.Delay(1);
        return GenerateTestSet(tableCount, count).CreateDataReader();
    }

}