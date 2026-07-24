// ReSharper disable InconsistentNaming


namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models;

public class ProductCategory
{
    public int ProductCategoryID { get; init; }
    public required string Name { get; init; }
}

public class ProductSubCategory
{
    public int ProductSubCategoryID { get; init; }
    public int ProductCategoryID { get; init; }
    public required string Name { get; init; }
}

public class ProductNameAndNumber
{
    public int ProductID { get; init; }
    public required string Name { get; init; }
    public required string ProductNumber { get; init; }
}



