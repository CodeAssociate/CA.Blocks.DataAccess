using System.Globalization;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Translator.Extensions;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.Extensions;

[TestFixture]
public class DbDataReaderSingleRowAsyncExtensions : DataReaderExtensionsBaseTests
{
    #region Single

    [Test]
    public async Task DataReaderExtensions_ToSingle_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingle_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingle<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingle_InvalidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            var result = await dataReader.ToSingleAsync<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }

    [Test]
    public async Task DataReaderExtensions_ToSingle_InvalidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            var result = await dataReader.ToSingle<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }


    [Test]
    public async Task DataReaderExtensions_ToSingle_InvalidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async() =>
        {
            var result = await dataReader.ToSingleAsync<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }

    [Test]
    public async Task DataReaderExtensions_ToSingle_InvalidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            var result = await dataReader.ToSingle<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }

    #endregion 

    #region SingleOrDefault

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () => 
        {
            var result = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }

    [Test]
    public async Task DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            var result = await dataReader.ToSingleOrDefault<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }

    #endregion 

    #region First 
    [Test]
    public async Task DataReaderExtensions_ToFirst_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirst_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirst_InvalidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () => 
        {
            var result = await dataReader.ToFirstAsync<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }

    [Test]
    public async Task DataReaderExtensions_ToFirst_InvalidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);

        var exception = Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            var result = await dataReader.ToFirst<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }

    [Test]
    public async Task DataReaderExtensions_ToFirst_ValidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }


    [Test]
    public async Task DataReaderExtensions_ToFirst_ValidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }
    #endregion

    #region First Or Default 
    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }


    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefaultValidNoRow()
    {
        var dataReader = await  GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefaultValidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }
    #endregion 
}