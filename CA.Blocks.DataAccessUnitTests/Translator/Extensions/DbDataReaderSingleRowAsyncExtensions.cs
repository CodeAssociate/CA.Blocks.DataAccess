using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

public class DbDataReaderSingleRowAsyncExtensions : DataReaderExtensionsBaseTests
{
    #region Single

    [Fact]
    public async Task DataReaderExtensions_ToSingle_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingle_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingle<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingle_InvalidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            _ = await dataReader.ToSingleAsync<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("No row", exception.Message);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingle_InvalidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            _ = await dataReader.ToSingle<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("No row", exception.Message);
    }


    [Fact]
    public async Task DataReaderExtensions_ToSingle_InvalidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async() =>
        {
            _ = await dataReader.ToSingleAsync<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("more", exception.Message);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingle_InvalidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            _ = await dataReader.ToSingle<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("more", exception.Message);
    }

    #endregion 

    #region SingleOrDefault

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_ValidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () => 
        {
            _ = await dataReader.ToSingleOrDefaultAsync<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("more", exception.Message);
    }

    [Fact]
    public async Task DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            _ = await dataReader.ToSingleOrDefault<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception.Message);
        Assert.Contains("more", exception.Message);
    }

    #endregion 

    #region First 
    [Fact]
    public async Task DataReaderExtensions_ToFirst_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirst_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirst_InvalidNoRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(0);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () => 
        {
            _ = await dataReader.ToFirstAsync<TestDataObject>();

        });
        Assert.Contains("Expected At least one result, but no row was found", exception.Message);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirst_InvalidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);

        var exception = await Assert.ThrowsAsync<System.Data.DataException>(async () =>
        {
            _ = await dataReader.ToFirst<TestDataObject>();

        });
        Assert.Contains("Expected At least one result, but no row was found", exception.Message);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirst_ValidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }


    [Fact]
    public async Task DataReaderExtensions_ToFirst_ValidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }
    #endregion

    #region First Or Default 
    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(1);
        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }


    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefaultValidNoRow()
    {
        var dataReader = await  GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefaultValidNoRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(0);
        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRow()
    {
        var dataReader = await GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstOrDefaultAsync<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public async Task DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRowTask()
    {
        var dataReader = GenerateTestDataReaderAsync(2);

        var result = await dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }
    #endregion 
}
