using System;


namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models
{
    public class ProductSummary
    {
        public int ProductID { get; init; }
        public string Name { get; init; }
        public string ProductNumber { get; init; }
        public short ReorderPoint { get; init; }
        public decimal StandardCost { get; init; }
        public Guid rowguid { get; init; }
        public DateTime ModifiedDate { get; init; }
    }


    public static class PrintExtensions
    {
        public static string Print(this ProductSummary item)
        {
            return $"{item.ProductID},{item.Name},{item.ProductNumber}";
        }
    }



}




