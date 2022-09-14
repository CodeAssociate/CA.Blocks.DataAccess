using System.Diagnostics;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.Models;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks
{
    [TestFixture]
    public class AdventureWorksDataAccessTests
    {
        private AdventureWorksDataAccess _adventureWorksDataAccess;
        public AdventureWorksDataAccessTests()
        {
            _adventureWorksDataAccess = new AdventureWorksDataAccess();
        }


        [Test]
        public void GetProductionProductCount()
        {
            var result = _adventureWorksDataAccess.GetProductionProductCount();
            TestContext.WriteLine(result);
        }

        [Test]
        public async Task GetProductionProductCountAsync()
        {
            var result = await _adventureWorksDataAccess.GetProductionProductCountAsync();
            TestContext.WriteLine(result);
        }

        [Test]
        public void GetValueThatMustBeConvertedToString()
        {
            var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToString();
            TestContext.WriteLine(result);
        }

        [Test]
        public void GetValueThatMustBeConvertedToByte()
        {
            var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToByte();
            TestContext.WriteLine(result);
        }

        [Test]
        public void GetValueThatMustBeConvertedToByte_Exception()
        {
            var exception = Assert.Throws<System.ArgumentException>(() =>
            {
                var result = _adventureWorksDataAccess.GetValueThatMustBeConvertedToByte_Exception();
            });
            TestContext.WriteLine(exception.Message);
        }

        [Test]
        public void GetProductSummary()
        {
            var result = _adventureWorksDataAccess.GetProductSummary(1);
            TestContext.WriteLine(result.Print());
        }

        [Test]
        public void GetProductSummaryUsingToFirstOrDefault()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToFirstOrDefault(1);
            TestContext.WriteLine(result.Print());
        }

        [Test]
        public void GetProductSummaryUsingToFirst()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToFirst(1);
            TestContext.WriteLine(result.Print());
        }

        [Test]
        public void GetProductSummaryUsingToSingle()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToSingle(1);
            TestContext.WriteLine(result.Print());
        }

        [Test]
        public void GetProductSummaryUsingToSingleOrDefault()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryUsingToSingleOrDefault(1);
            TestContext.WriteLine(result.Print());
        }

        [Test]
        public void GetAllProductSummary()
        {
            var result = _adventureWorksDataAccess.GetAllProductSummary();
            foreach (var product in result)
            {
                TestContext.WriteLine(product.Print());
            }
        }

        [Test]
        public void GetProductSummaryContainingName()
        {
            var result = _adventureWorksDataAccess.GetProductSummaryContainingName("%Bike%");
            foreach (var product in result)
            {
                TestContext.WriteLine(product.Print());
            }
        }
    }
}
