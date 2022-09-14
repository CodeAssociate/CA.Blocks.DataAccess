using System.Collections.Generic;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
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

        public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
        {
            var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
            var cmd = CreateTextCommand(sql).WithParameter(searchTerm.ToSqlParameter("@searchTerm"));
            return Execute(cmd).ToListOf<ProductSummary>();
        }



    }
}
