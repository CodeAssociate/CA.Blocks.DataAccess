using CA.Blocks.DataAccess.Translator.Extensions;
using NUnit.Framework.Legacy;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

[TestFixture]
public class DataReaderExtensionSingleRowTests : DataReaderExtensionsBaseTests
{
    [Test]
    public void DataReaderExtensions_ToSingle_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingle<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }

    [Test]
    public void DataReaderExtensions_ToSingle_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            _ = dataReader.ToSingle<TestDataObject>(); 

        });
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception!.Message, Does.Contain("Expected Single Result"));
        Assert.That(exception!.Message, Does.Contain("No row"));
    }

    [Test]
    public void DataReaderExtensions_ToSingle_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            _ = dataReader.ToSingle<TestDataObject>();

        });
        Assert.That(exception!.Message, Does.Contain("Expected Single Result"));
        Assert.That(exception!.Message, Does.Contain("more"));
    }


    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }

    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_ValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToSingleOrDefault<TestDataObject>();
        Assert.That(result, Is.EqualTo(default(TestDataObject)));
    }

    [Test]
    public void DataReaderExtensions_ToSingleOrDefault_InvalidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToSingleOrDefault<TestDataObject>();

        });
        Assert.That(exception!.Message, Does.Contain("Expected Single Result"));
        Assert.That(exception!.Message, Does.Contain("more"));
    }

    #region First 
    [Test]
    public void DataReaderExtensions_ToFirst_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirst<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }

    [Test]
    public void DataReaderExtensions_ToFirst_InvalidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);

        var exception = Assert.Throws<System.Data.DataException>(() =>
        {
            var result = dataReader.ToFirst<TestDataObject>();

        });
        Assert.That(exception!.Message, Does.Contain("Expected Single Result"));
        Assert.That(exception!.Message, Does.Contain("No row"));
    }

    [Test]
    public void DataReaderExtensions_ToFirst_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirst<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }
    #endregion

    #region First Or Default 
    [Test]
    public void DataReaderExtensions_ToFirstOrDefault_ValidRow()
    {
        var dataReader = GenerateTestDataReader(1);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }

    [Test]
    public void DataReaderExtensions_ToFirstOrDefaultValidNoRow()
    {
        var dataReader = GenerateTestDataReader(0);
        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        Assert.That(result, Is.EqualTo(default(TestDataObject)));
    }

    [Test]
    public void DataReaderExtensions_ToFirstOrDefault_ValidMoreThanOneRow()
    {
        var dataReader = GenerateTestDataReader(2);

        var result = dataReader.ToFirstOrDefault<TestDataObject>();
        ClassicAssert.NotNull(result);
        Assert.That(result.IntCol, Is.EqualTo(1));
    }
    #endregion 
}