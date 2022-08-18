using System.Data;
using System.Data.Common;
using BenchmarkDotNet.Attributes;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.DataReaderExtensions;


/// <summary>
/// This tests ony the Translators, use using an in memory data table, So there zero blocking IO. 
/// </summary>
[MemoryDiagnoser]
public class AsyncVrsSyncWithAutoNameAndIdBenchmarks
{

    /* Results
    BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
    Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
    .NET SDK=6.0.303
      [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
      Job-WMXPWV : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

    InvocationCount=1  UnrollFactor=1

    |                             Method |       Mean |      Error |     StdDev |     Median | Ratio | RatioSD | Allocated |
    |----------------------------------- |-----------:|-----------:|-----------:|-----------:|------:|--------:|----------:|
    |                       ToListOfSync |  66.154 us |  2.7033 us |  7.6246 us |  62.000 us |  1.00 |    0.00 |      5 KB |
    |  ToListOfSyncCustomTranslateByName |   8.735 us |  0.1743 us |  0.2007 us |   8.700 us |  0.13 |    0.01 |      1 KB |
    |    ToListOfSyncCustomTranslateById |   7.582 us |  0.1470 us |  0.1510 us |   7.500 us |  0.11 |    0.01 |      1 KB |
    |                      ToListOfAsync | 156.901 us | 16.4982 us | 48.3862 us | 156.100 us |  2.45 |    0.83 |      8 KB |
    | ToListOfASyncCustomTranslateByName |  35.868 us |  6.2391 us | 18.2982 us |  28.100 us |  0.55 |    0.29 |      3 KB |
    |   ToListOfASyncCustomTranslateById |  32.618 us |  5.5833 us | 16.2868 us |  29.700 us |  0.51 |    0.25 |      3 KB |

     // * Legends *
      Mean      : Arithmetic mean of all measurements
      Error     : Half of 99.9% confidence interval
      StdDev    : Standard deviation of all measurements
      Median    : Value separating the higher half of all measurements (50th percentile)
      Ratio     : Mean of the ratio distribution ([Current]/[Baseline])
      RatioSD   : Standard deviation of the ratio distribution ([Current]/[Baseline])
      Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
      1 us      : 1 Microsecond (0.000001 sec)    
     */

    protected class TestDataObject
    {
        public int IntCol { get; set; }
        public string StringCol { get; set; }
        public Guid GuidCol { get; set; }
        public DateTime DateCol { get; set; }
    }

    private DataTable GenerateTestData(int count)
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
        for (var i = start; i <= (start + count - 1); i++)
        {
            testData.Rows.Add(i, $"row#{i}", Guid.NewGuid(), DateTime.Now.AddMinutes(i));
        }
        testData.AcceptChanges();
        return testData;
    }
    protected IDataReader GenerateTestSet(int count)
    {
        return GenerateTestData(1).CreateDataReader();
    }

    protected async Task<DbDataReader> GenerateTestDataReaderAsync(int count)
    {
        await Task.Delay(1);
        return GenerateTestData(count).CreateDataReader();
    }


    private TestDataObject CustomTranslateByName(IDataReader dr)
    {
        var result = new TestDataObject
        {
            IntCol = dr.AsInt("IntCol"),
            StringCol = dr.AsString("StringCol"),
            GuidCol = dr.AsGuid("GuidCol"),
            DateCol = dr.AsDateTime("DateCol")
        };
        return result;
    }

    private TestDataObject CustomTranslateById(IDataReader dr)
    {
        var result = new TestDataObject
        {
            IntCol = dr.AsInt(0),
            StringCol = dr.AsString(1),
            GuidCol = dr.AsGuid(2),
            DateCol = dr.AsDateTime(3)
        };
        return result;
    }

    private IDataReader _syncTarget;

    [IterationSetup(Targets = new[]
        { nameof(ToListOfSync), nameof(ToListOfSyncCustomTranslateByName), nameof(ToListOfSyncCustomTranslateById) })]
    public void IterationSetup()
    {
        _syncTarget = GenerateTestSet(10);
    }

    
    [Benchmark(Baseline = true)]
    public int ToListOfSync()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _syncTarget.ToListOf<TestDataObject>();
        return result.Count;
    }

    [Benchmark]
    public int ToListOfSyncCustomTranslateByName()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _syncTarget.ToListOf<TestDataObject>(CustomTranslateByName);
        return result.Count;
    }

    [Benchmark]
    public int ToListOfSyncCustomTranslateById()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _syncTarget.ToListOf<TestDataObject>(CustomTranslateById);
        return result.Count;
    }

    

    private DbDataReader _asyncTarget;

    [IterationSetup(Targets = new[]
        {  nameof(ToListOfAsync), nameof(ToListOfASyncCustomTranslateByName), nameof(ToListOfASyncCustomTranslateById) })]
    public void IterationSetupAsync()
    {
        var task = GenerateTestDataReaderAsync(10);
        task.Wait();
        _asyncTarget = task.Result;
    }

    [Benchmark]
    public async Task<int> ToListOfAsync()
    {
        //var asyncTarget = await GenerateTestDataReaderAsync(10);
        var result = await _asyncTarget.ToListOfAsync<TestDataObject>();
        return result.Count;
    }

    [Benchmark]
    public async Task<int> ToListOfASyncCustomTranslateByName()
    {
        //var asyncTarget = await GenerateTestDataReaderAsync(10);
        var result = await _asyncTarget.ToListOfAsync<TestDataObject>(CustomTranslateByName);
        return result.Count;
    }

    [Benchmark]
    public async Task<int> ToListOfASyncCustomTranslateById()
    {
        //var asyncTarget = await GenerateTestDataReaderAsync(10);
        var result = await _asyncTarget.ToListOfAsync<TestDataObject>(CustomTranslateById);
        return result.Count;
    }

    public void IterationCleanup()
    {
        _syncTarget = null;
        _asyncTarget = null;
    }
}