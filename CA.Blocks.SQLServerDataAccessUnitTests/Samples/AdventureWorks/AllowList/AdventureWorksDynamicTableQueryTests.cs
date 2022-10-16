using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.AllowList
{

    [TestFixture]
    public class AdventureWorksDynamicTableQueryTests
    {
        private AdventureWorksDynamicTableQuery _adventureWorksDataAccess;
        public AdventureWorksDynamicTableQueryTests()
        {
            _adventureWorksDataAccess = new AdventureWorksDynamicTableQuery();
        }


        [Test]
        public void SelectDynamicTableFromSalesSchema_Valid()
        {
            var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("vSalesPerson");
            Assert.IsNotNull(dtResult);
            Assert.Greater(dtResult.Rows.Count, 0);
        }

        [Test]
        public void SelectDynamicTableFromSalesSchema_BadTableName()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("BadTableName");
                }
            );

            Assert.True(exception.Message.Contains("BadTableName"));
        }


        [Test]
        public void SelectDynamicTableFromSalesSchema_WrongSchema()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("Person");
                }
            );

            Assert.True(exception.Message.Contains("Person"));
        }

    }
}
