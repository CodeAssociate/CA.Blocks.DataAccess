using System.Collections.Generic;
using System.Threading.Tasks;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Model.Results;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks
{
    public class AdventureWorksDataAccess : SqlServerDataAccess
    {
        public AdventureWorksDataAccess() :
            base(new SimpleConnectionStringDataAccessConfig(
                "Server=(local);Database=AdventureWorks2019;Integrated Security=SSPI;TrustServerCertificate=True"))
        {

        }

        public bool DBExists()
        {
            var cmd = CreateTextCommand("Select name from master..sysdatabases where name = 'AdventureWorks2019'");
            return ExecuteScalarAs<string>(cmd) == "AdventureWorks2019";
        }


        public int GetProductionProductCount()
        {
            var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
            return ExecuteScalarAs<int>(cmd);
        }

        public Task<int> GetProductionProductCountAsync()
        {
            var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
            return ExecuteScalarAsAsync<int>(cmd);
        }

        public string GetValueThatMustBeConvertedToString()
        {
            // Here we getting a values as a byte from the server but returning the values as a string
            var cmd = CreateTextCommand("Select Cast(123 as tinyint) as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<string>(cmd);
        }

        public byte GetValueThatMustBeConvertedToByte()
        {
            // Here we getting a values as a string from the server but returning the values as a byte
            var cmd = CreateTextCommand("Select '123' as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<byte>(cmd);
        }

        public byte GetValueThatMustBeConvertedToByte_Exception()
        {
            var cmd = CreateTextCommand("Select '1234' as ExampleOfConvert");
            // this will raise a System.ArgumentException with message '1234 is not a valid value for Byte.'
            return ExecuteScalarWithConvertAs<byte>(cmd);
        }

        private string ProductSummarySQL(string filter = "")
        {
            return @$"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
{filter}";
        }

        public ProductSummary GetProductSummary(int productId)
        {
            var sql = ProductSummarySQL("where ProductID = @productId");
            var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
            return ExecuteTo<ProductSummary>(cmd);
        }

        public ProductSummary GetProductSummaryUsingToFirstOrDefault(int productId)
        {
            var sql = ProductSummarySQL("where ProductID = @productId");
            var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
            return Execute(cmd).ToFirstOrDefault<ProductSummary>();
        }

        public ProductSummary GetProductSummaryUsingToSingle(int productId)
        {
            var sql = ProductSummarySQL("where ProductID = @productId");
            var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
            return Execute(cmd).ToSingle<ProductSummary>();
        }

        public ProductSummary GetProductSummaryUsingToSingleOrDefault(int productId)
        {
            var sql = ProductSummarySQL("where ProductID = @productId");
            var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
            return Execute(cmd).ToSingleOrDefault<ProductSummary>();
        }

        public ProductSummary GetProductSummaryUsingToFirst(int productId)
        {
            var sql = ProductSummarySQL("where ProductID = @productId");
            var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
            return Execute(cmd).ToFirst<ProductSummary>();
        }


        public IList<ProductSummary> GetAllProductSummary()
        {
            var sql = ProductSummarySQL();
            var cmd = CreateTextCommand(sql);
            return Execute(cmd).ToListOf<ProductSummary>();
        }


        public IList<ProductSummary> GetAllProductSummaryWithFunc()
        {
            var sql = ProductSummarySQL();
            var cmd = CreateTextCommand(sql);
            return Execute(cmd).ToListOf<ProductSummary>(reader => new ProductSummary
            {
                ProductID = reader.AsInt("ProductID"),
                Name = reader.AsString("Name"),
                ProductNumber = reader.AsString("ProductNumber"),
                ReorderPoint = reader.AsShort("ReorderPoint"),
                StandardCost = reader.AsDecimal("StandardCost"),
                rowguid = reader.AsGuid("rowguid"),
                ModifiedDate = reader.AsDateTime("ModifiedDate"),
            });
        }


        public Task<IList<ProductSummary>> GetAllProductSummaryAsync()
        {
            var sql = ProductSummarySQL();
            var cmd = CreateTextCommand(sql);
            return ExecuteAsync(cmd).ToListOf<ProductSummary>();
        }


        public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
        {
            var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
            var cmd = CreateTextCommand(sql).WithParameter(searchTerm.ToSqlParameter("@searchTerm"));
            return Execute(cmd).ToListOf<ProductSummary>();
        }

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


        public int CreateNewProductCategory(string name)
        {
            var sql = "Insert into [Production].[ProductCategory] (Name, rowguid, ModifiedDate) values (@name, NEWID(), GetDate())";
            var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
            return ExecuteNonQuery(cmd);
        }


        public int DeleteProductCategory(string name)
        {
            var sql = "Delete from [Production].[ProductCategory] where Name = @name";
            var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
            return ExecuteNonQuery(cmd);
        }

        public bool CreateTableExample()
        {
            var sql = @"
If exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MyTable')
BEGIN
	drop  table MyTable 
END;
Create Table MyTable (Id int not null, Name varchar(10) not null);";
            var cmd = CreateTextCommand(sql);
            return ExecuteNonQuery(cmd) == -1;
        }

    }
}




