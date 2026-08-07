using Microsoft.Data.SqlClient;
using System.Diagnostics;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator
{
    [Collection("DbIntegrationTests")]
    public class DynamicDbRow2ObjectTranslatorTests : UnitTestDataAccess
    {
        
        #region TestSysobjects
        [Fact]
        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping()
        {
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            var result = DynamicDbRow2ObjectTranslator.CurrentInstance.Translate(Execute(cmd).ToDataTable());

            Assert.True(result.Count > 0);

            var outputformat = "{0}\t{1}\t{2}\t{3}";
            Trace.WriteLine(string.Format(outputformat, "id", "name", "xtype", "crdate"));
            foreach (var item in result)
            {
                Assert.NotNull(item);
                Console.WriteLine($"{item!.id}\t{item.name}\t{item.xtype}\t{item.crdate}");
            }
        }
        #endregion 
    }
}




