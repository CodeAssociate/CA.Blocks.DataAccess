using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

[TestFixture]
public class DbDataReaderAsyncExtensions : DataReaderExtensionsBaseTests
{

    [Test]
    public async Task DbDataReaderExtensions_ToListOfAsync()
    {
        var numberOfRecords = 10;
        var testDate = DateTime.Now;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToListOf<TestDataObject>();
        Assert.That(result, Has.Count.EqualTo(numberOfRecords));
        Assert.That(numberOfRecords, Is.EqualTo(result[numberOfRecords - 1].IntCol));
        Assert.That(result.Count(x => x.DateCol < testDate), Is.EqualTo(0));
    }


    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_String()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<string>("StringCol");
        Assert.That(result, Has.Count.EqualTo(numberOfRecords));
        Assert.That(result[numberOfRecords - 1], Does.Contain(numberOfRecords.ToString()));
    }

    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_StringAsync()
    {
        var numberOfRecords = 10;
        var dataReaderTask = await GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnListAsync<string>("StringCol");
        Assert.That(result.Count, Is.EqualTo(numberOfRecords));
        Assert.True(result[numberOfRecords - 1].Contains(numberOfRecords.ToString()));
    }


    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_IntCustom()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<int>("intCol", (reader, s) => reader.AsInt(s));
        Assert.That(result.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result[numberOfRecords - 1], Is.EqualTo(numberOfRecords));
    }

    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_IntCustomAsync()
    {
        var numberOfRecords = 10;
        var dataReaderTask = await GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnListAsync<int>("intCol", (reader, s) => reader.AsInt(s));
        Assert.That(result.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result[numberOfRecords - 1], Is.EqualTo(numberOfRecords));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet2()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(2, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject>();
        Assert.That(result.Results1, Has.Count.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet2Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(2, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));
    }


    [Test]
    public async Task DataReaderExtensions_ToResultsSet3()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(3, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet3Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(3, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet4()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(4, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

        Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet4Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(4, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

        Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet5()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(5, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

        Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));

        Assert.That(result.Results5.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results5[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 5));
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet5Async()
    {
        var numberOfRecords = 10;
        var dataReader = await GenerateTestDataSetReaderAsync(5, 10);

        var result = await dataReader.ToResultsSetAsync<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

        Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));

        Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

        Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));

        Assert.That(result.Results5.Count, Is.EqualTo(numberOfRecords));
        Assert.That(result.Results5[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 5));
    }


}