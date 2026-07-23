using System.Data;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.AllowList
{

    public class AdventureWorksDynamicTableQueryTests
    {
        private readonly AdventureWorksDynamicTableQuery _adventureWorksDataAccess;
        public AdventureWorksDynamicTableQueryTests()
        {
            _adventureWorksDataAccess = new AdventureWorksDynamicTableQuery();
            Init();
        }


        public void Init()
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
        public void SelectDynamicTableFromSalesSchema_Valid()
        {
            var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("vSalesPerson");
            Assert.NotNull(dtResult);
            Assert.True(dtResult.Rows.Count > 0);
        }

        [Fact]
        public void SelectDynamicTableFromSalesSchema_BadTableName()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("BadTableName");
                }
            );

            Assert.Contains("BadTableName", exception.Message);
        }


        [Fact]
        public void SelectDynamicTableFromSalesSchema_WrongSchema()
        {
            var exception = Assert.Throws<DataException>(() =>
                {
                    var dtResult = _adventureWorksDataAccess.SelectDynamicTableFromSalesSchema("Person");
                }
            );

            Assert.Contains("Person", exception.Message);
        }

    }
}



