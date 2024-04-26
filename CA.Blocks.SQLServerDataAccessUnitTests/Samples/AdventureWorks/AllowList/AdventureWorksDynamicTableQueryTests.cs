using System.Data;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.AllowList
{

    [TestFixture]
    public class AdventureWorksDynamicTableQueryTests
    {
        private readonly AdventureWorksDynamicTableQuery _adventureWorksDataAccess;
        public AdventureWorksDynamicTableQueryTests()
        {
            _adventureWorksDataAccess = new AdventureWorksDynamicTableQuery();
        }


        [SetUp]
        public void Init()
        {
            try
            {
                if (!_adventureWorksDataAccess.DBExists())
                {
                    Assert.Ignore("The AdventureWorks database does not  exist");
                }
            }
            catch 
            {
                Assert.Ignore("The AdventureWorks database does not exist");
            }
        }


        [Test]
        public void SelectDynamicTableFromSalesSchema_Valid()
        {
            var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("vSalesPerson");
            ClassicAssert.IsNotNull(dtResult);
            ClassicAssert.Greater(dtResult.Rows.Count, 0);
        }

        [Test]
        public void SelectDynamicTableFromSalesSchema_BadTableName()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("BadTableName");
                }
            );

            ClassicAssert.True(exception.Message.Contains("BadTableName"));
        }


        [Test]
        public void SelectDynamicTableFromSalesSchema_WrongSchema()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("Person");
                }
            );

            ClassicAssert.True(exception.Message.Contains("Person"));
        }

    }
}
