using CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks
{
    public class AdventureWorksDataAccessTests
    {
        private readonly AdventureWorksDataAccess _adventureWorksDataAccess;
        public AdventureWorksDataAccessTests()
        {
            _adventureWorksDataAccess = new AdventureWorksDataAccess();
            Init();
        }

        private void Init()
        {
            try
            {
                if (!_adventureWorksDataAccess.DBExists())
                {
                    throw Xunit.Sdk.SkipException.ForSkip("The AdventureWorks database does not exist");
                }
            }
            catch
            {
                throw Xunit.Sdk.SkipException.ForSkip("The AdventureWorks database does not exist");
            }
        }


        [Fact]
        public void GetProductionProductCount()
        {
            var result = _adventureWorksDataAccess.GetProductionProductCount();
            Console.WriteLine(result);
        }

        [Fact]
        public async Task GetProductionProductCountAsync()
        {
            var result = await _adventureWorksDataAccess.GetProductionProductCountAsync();
            Console.WriteLine(result);
        }

        [Fact]
        public void GetValueThatMustBeConvertedToString()
        {
            var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToString();
            Console.WriteLine(result);
        }

        [Fact]
        public void GetValueThatMustBeConvertedToByte()
        {
            var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToByte();
            Console.WriteLine(result);
        }

        [Fact]
        public void GetValueThatMustBeConvertedToByte_Exception()
        {
            var exception = Assert.Throws<System.ArgumentException>(() =>
            {
                var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToByte_Exception();
            });
            Console.WriteLine(exception.Message);
        }

        [Fact]
        public void GetProductSummary()
        {
            var result = _adventureWorksDataAccess.GetProductSummary(1);
            Console.WriteLine(result.Print());
        }

        [Fact]
        public void GetProductSummaryUsingToFirstOrDefault()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToFirstOrDefault(1);
            Console.WriteLine(result.Print());
        }

        [Fact]
        public void GetProductSummaryUsingToFirst()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToFirst(1);
            Console.WriteLine(result.Print());
        }

        [Fact]
        public void GetProductSummaryUsingToSingle()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToSingle(1);
            Console.WriteLine(result.Print());
        }

        [Fact]
        public void GetProductSummaryUsingToSingleOrDefault()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToSingleOrDefault(1);
            Console.WriteLine(result.Print());
        }

        [Fact]
        public void GetAllProductSummary()
        {
            var result = _adventureWorksDataAccess.GetAllProductSummary();
            foreach (var product in result)
            {
                Console.WriteLine(product.Print());
            }
        }

        [Fact]
        public void GetAllProductSummaryWithFunc()
        {
            var result = _adventureWorksDataAccess.GetAllProductSummaryWithFunc();
            foreach (var product in result)
            {
                Console.WriteLine(product.Print());
            }
        }


        [Fact]
        public async Task GetAllProductSummaryAsync()
        {
            var result = await _adventureWorksDataAccess.GetAllProductSummaryAsync();
            foreach (var product in result)
            {
                Console.WriteLine(product.Print());
            }
        }

        [Fact]
        public void GetProductSummaryContainingName()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryContainingName("%Bike%");
            foreach (var product in result)
            {
                Console.WriteLine(product.Print());
            }
        }

        [Fact]
        public void GetProjectCategoryResultSet()
        {
            var result = _adventureWorksDataAccess.GetProjectCategoryResultSet();
            foreach (var category in result.Results1)
            {
                Console.WriteLine(category.Name);
            }
            foreach (var subCategory in result.Results2)
            {
                Console.WriteLine(subCategory.Name);
            }
            foreach (var product in result.Results3)
            {
                Console.WriteLine(product.Name);
            }
        }

        [Fact]
        public void CreateTableExample()
        {

            var result = _adventureWorksDataAccess.CreateTableExample();
            Console.WriteLine(result.ToString());
        }
    }
}



