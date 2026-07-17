namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models;

public class ProductCategory
{
    public int ProductCategoryID { get; init; }
    public string Name { get; init; }
}

public class ProductSubCategory
{
    public int ProductSubCategoryID { get; init; }
    public int ProductCategoryID { get; init; }
    public string Name { get; init; }
}

public class ProductNameAndNumber
{
    public int ProductID { get; init; }
    public string Name { get; init; }
    public string ProductNumber { get; init; }
}



