using System.Globalization;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccessUnitTests.Translator.Extensions;
using NUnit.Framework;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

[TestFixture]
public class DataReaderExtensionSingleRowTests : DataReaderExtensionsBaseTests
{
    [Test]
    public void DataReaderExtensions_ToSingle_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingle<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public void DataReaderExtensions_ToSingle_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToSingle<TestDataObject>(); 

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }

    [Test]
    public void DataReaderExtensions_ToSingle_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToSingle<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }


    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_ValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToSingleOrDefault<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("more"));
    }

    #region First 
    [Test]
    public void DataReaderExtensions_ToFirst_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public void DataReaderExtensions_ToFirst_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToFirst<TestDataObject>();

        });
        Assert.True(exception.Message.Contains("Expected Single Result"));
        Assert.True(exception.Message.Contains("No row"));
    }

    [Test]
    public void DataReaderExtensions_ToFirst_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirst<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }
    #endregion

    #region First Or Default 
    [Test]
    public void DataReaderExtensions_ToFirstOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }

    [Test]
    public void DataReaderExtensions_ToFirstOrDefaultValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.AreEqual(default(TestDataObject), result);
    }

    [Test]
    public void DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.NotNull(result);
        Assert.AreEqual(result.IntCol, 1);
    }
    #endregion 
}