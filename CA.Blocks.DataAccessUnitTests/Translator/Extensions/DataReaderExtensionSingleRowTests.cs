using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

public class DataReaderExtensionSingleRowTests : DataReaderExtensionsBaseTests
{
    [Fact]
    public void DataReaderExtensions_ToSingle_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingle<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public void DataReaderExtensions_ToSingle_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            _ = dataReader.ToSingle<TestDataObject>(); 

        });
        Assert.NotNull(exception);
        Assert.Contains("Expected Single Result", exception!.Message);
        Assert.Contains("No row", exception!.Message);
    }

    [Fact]
    public void DataReaderExtensions_ToSingle_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            _ = dataReader.ToSingle<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception!.Message);
        Assert.Contains("more", exception!.Message);
    }


    [Fact]
    public void DataReaderExtensions_ToSingleOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public void DataReaderExtensions_ToSingleOrDefault_ValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public void DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToSingleOrDefault<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception!.Message);
        Assert.Contains("more", exception!.Message);
    }

    #region First 
    [Fact]
    public void DataReaderExtensions_ToFirst_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public void DataReaderExtensions_ToFirst_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToFirst<TestDataObject>();

        });
        Assert.Contains("Expected Single Result", exception!.Message);
        Assert.Contains("No row", exception!.Message);
    }

    [Fact]
    public void DataReaderExtensions_ToFirst_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }
    #endregion

    #region First Or Default 
    [Fact]
    public void DataReaderExtensions_ToFirstOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }

    [Fact]
    public void DataReaderExtensions_ToFirstOrDefaultValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.Equal(default(TestDataObject), result);
    }

    [Fact]
    public void DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.Equal(1, result.IntCol);
    }
    #endregion 
}
