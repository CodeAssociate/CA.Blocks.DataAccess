using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

public class DbDataReaderAsyncExtensions : DataReaderExtensionsBaseTests
{

    [Fact]
    public async Task DbDataReaderExtensions_ToListOfAsync()
    {
        var numberOfRecords = 10;
        var testDate = DateTime.Now;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToListOf<TestDataObject>();
        Assert.Equal(numberOfRecords, result.Count);
        Assert.Equal(result[numberOfRecords - 1].IntCol, numberOfRecords);
        Assert.Equal(0, result.Count(x => x.DateCol < testDate));
    }

    [Fact]
    public async Task DataReaderExtensions_ToDictionaryAsync()
    {
	    var numberOfRecords = 10;
	    var testDate = DateTime.Now;
	    var dataReader = GenerateTestDataReaderAsync(numberOfRecords);

	    var result = await dataReader.ToDictionaryAsync<int, TestDataObject>(x => x.IntCol);
	    Assert.Equal(numberOfRecords, result.Count);
	    Assert.Equal(result[numberOfRecords].IntCol, numberOfRecords);
    }


	[Fact]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_String()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<string>("StringCol");
        Assert.Equal(numberOfRecords, result.Count);
        Assert.Contains(numberOfRecords.ToString(), result[numberOfRecords - 1]);
    }

    [Fact]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_StringAsync()
    {
        var numberOfRecords = 10;
        var dataReaderTask = await GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnListAsync<string>("StringCol");
        Assert.Equal(numberOfRecords, result.Count);
        Assert.True(result[numberOfRecords - 1].Contains(numberOfRecords.ToString()));
    }


    [Fact]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_IntCustom()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<int>("intCol", (reader, s) => reader.AsInt(s));
        Assert.Equal(numberOfRecords, result.Count);
        Assert.Equal(numberOfRecords, result[numberOfRecords - 1]);
    }

    [Fact]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_IntCustomAsync()
    {
        var numberOfRecords = 10;
        var dataReaderTask = await GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnListAsync<int>("intCol", (reader, s) => reader.AsInt(s));
        Assert.Equal(numberOfRecords, result.Count);
        Assert.Equal(numberOfRecords, result[numberOfRecords - 1]);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet2()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(2, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet2Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(2, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);
    }


    [Fact]
    public async Task DataReaderExtensions_ToResultsSet3()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(3, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet3Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(3, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet4()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(4, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results4.Count);
        Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet4Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(4, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results4.Count);
        Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet5()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(5, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results4.Count);
        Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results5.Count);
        Assert.Equal(numberOfRecords * 5, result.Results5[numberOfRecords - 1].IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToResultsSet5Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(5, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.Equal(numberOfRecords, result.Results1.Count);
        Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results2.Count);
        Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results3.Count);
        Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results4.Count);
        Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);

        Assert.Equal(numberOfRecords, result.Results5.Count);
        Assert.Equal(numberOfRecords * 5, result.Results5[numberOfRecords - 1].IntCol);
    }


}
