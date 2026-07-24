### Multiple sets


| Method      | Description |
| ----------- | ----------- |
| Execute(cmd).ToResultsSet<T1, T2>   | Returns an object derived from the data reader |


### Execute(cmd).ToResultsSet<T1, T2>

The blocks support executing multiple ResultSets within a single query.
Given the example where we would like to get the ProductCategory  the ProductSubCategory and the ProductNameAndNumber we could execute this as three methods:

```C#
    public IList<ProductCategory> GetProductCategory()
    {
        var sql = @"Select ProductCategoryID, Name from [Production].[ProductCategory]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductCategory>();
    }

    public IList<ProductSubCategory> GetProductSubCategory()
    {
        var sql = @"Select ProductCategoryID, ProductSubCategoryID, Name  from [Production].[ProductSubcategory]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductSubCategory>();
    }
    public IList<ProductNameAndNumber> GetProductNameAndNumber()
    {
        var sql = @"Select ProductID, ProductSubCategoryID, Name, ProductNumber from [Production].[Product] where ProductSubcategoryID is not null;";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductNameAndNumber>();
    }

```

If we know we always want to work with all three sets of data, we can wrap the execution up into a single `ToResultsSet`. The key advantage is reduced latency, as all three sets can be fetched from the database in a single round trip.

To do this, simply execute the result into `ToResultsSet<T1, T2, ... T5>`; the blocks have support for up to 5 result sets in a single statement. 

```C#
        public ResultsSet<ProductCategory, ProductSubCategory, ProductNameAndNumber> GetProjectCategoryResultSet()
        {
            var sql = @"
Select ProductCategoryID, Name from [Production].[ProductCategory];
Select ProductCategoryID, ProductSubCategoryID, Name  from [Production].[ProductSubcategory];
Select ProductID, ProductSubCategoryID, Name, ProductNumber from [Production].[Product] where ProductSubcategoryID is not null;
";
            var cmd = CreateTextCommand(sql);
            return Execute(cmd).ToResultsSet<ProductCategory, ProductSubCategory, ProductNameAndNumber>();
        }
```


Note there is no automatic aggregation of results.  