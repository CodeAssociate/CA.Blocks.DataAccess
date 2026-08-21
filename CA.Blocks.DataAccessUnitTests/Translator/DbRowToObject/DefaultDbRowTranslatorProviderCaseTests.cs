using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccessUnitTests.TestData;
using CA.Blocks.DataAccessUnitTests.TestData.StubObjects;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbRowToObject;

public class DefaultDbRowTranslatorProviderCaseTests
{
    [Fact]
    public void TranslateFromPascalCase()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.PascalCase);
        var dr = dt.CreateDataReader();
        var result = dr.ToListOf<TestDataClass>();
            
        // assert
        Assert.Equal("IntCol", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    [Fact]
    public void TranslateFromCamelCase()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.CamelCase);
        var dr = dt.CreateDataReader();
        var result = dr.ToListOf<TestDataClass>();
   
        // assert
        Assert.Equal("intCol", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    [Fact]
    public void TranslateFromSnakeCase()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.SnakeCase);
        var dr = dt.CreateDataReader();
        var result = dr.ToListOf<TestDataClass>();
            
        // assert
        Assert.Equal("int_col", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    [Fact]
    public void TranslateFromMadeUpCase()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.SnakeCase);
        dt.Columns[0].ColumnName = "Int_CoL";
        var dr = dt.CreateDataReader();
        
        var result = dr.ToListOf<TestDataClass>();
            
        // assert
        Assert.Equal("Int_CoL", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    [Fact]
    public void TranslateFromPascalCaseDataTable()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.PascalCase);
        var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TestDataClass>();
        var result = translator.Translate(dt);
            
        // assert
        Assert.Equal("IntCol", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    
    [Fact]
    public void TranslateFromCamelCaseDataTable()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.CamelCase);
        var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TestDataClass>();
        var result = translator.Translate(dt);
   
        // assert
        Assert.Equal("intCol", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    [Fact]
    public void TranslateFromSnakeCaseDataTable()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10, TestDataGenerator.SourceColNamingConvention.SnakeCase);
        var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TestDataClass>();
        var result = translator.Translate(dt);
            
        // assert
        Assert.Equal("int_col", dt.Columns[0].ColumnName);
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].IntCol);
    }
    
    
    
}