using System;
using System.Linq;
using System.Threading.Tasks;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.Extensions;

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
        Assert.AreEqual(numberOfRecords, result.Count);
        Assert.AreEqual(result[numberOfRecords - 1].IntCol, numberOfRecords);
        Assert.AreEqual(0, result.Count(x => x.DateCol < testDate));
    }


    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_String()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<string>("StringCol");
        Assert.AreEqual(numberOfRecords, result.Count);
        Assert.True(result[numberOfRecords - 1].Contains(numberOfRecords.ToString()));
    }

    [Test]
    public async Task DbDataReaderExtensions_ToSingleNamedColumnListAsync_IntCustom()
    {
        var numberOfRecords = 10;
        var dataReaderTask = GenerateTestDataReaderAsync(numberOfRecords);

        var result = await dataReaderTask.ToSingleNamedColumnList<int>("intCol", (reader, s) => reader.AsInt(s));
        Assert.AreEqual(numberOfRecords, result.Count);
        Assert.AreEqual(numberOfRecords, result[numberOfRecords - 1]);
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet2()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(2, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject>();
        Assert.AreEqual(numberOfRecords, result.Results1.Count);
        Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results2.Count);
        Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet3()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(3, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
        Assert.AreEqual(numberOfRecords, result.Results1.Count);
        Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results2.Count);
        Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results3.Count);
        Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet4()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(4, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.AreEqual(numberOfRecords, result.Results1.Count);
        Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results2.Count);
        Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results3.Count);
        Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results4.Count);
        Assert.AreEqual(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);
    }

    [Test]
    public async Task DataReaderExtensions_ToResultsSet5()
    {
        var numberOfRecords = 10;
        var dataReader = GenerateTestDataSetReaderAsync(5, 10);

        var result = await dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
        Assert.AreEqual(numberOfRecords, result.Results1.Count);
        Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results2.Count);
        Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results3.Count);
        Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results4.Count);
        Assert.AreEqual(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);

        Assert.AreEqual(numberOfRecords, result.Results5.Count);
        Assert.AreEqual(numberOfRecords * 5, result.Results5[numberOfRecords - 1].IntCol);
    }


}