using System.Data.SqlClient;
using System.Diagnostics;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator
{
    [TestFixture]
    public class DynamicDbRow2ObjectTranslatorTests : UnitTestDataAccess
    {
        
        #region TestSysobjects
        [Test]
        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping()
        {
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            var result = DynamicDbRow2ObjectTranslator.CurrentInstance.Translate(ExecuteDataTable(cmd));

            Assert.IsTrue(result.Count > 0);

            var outputformat = "{0}\t{1}\t{2}\t{3}";
            Trace.WriteLine(string.Format(outputformat, "id", "name", "xtype", "crdate"));
            foreach (var item in result)
            {
                TestContext.WriteLine($"{item.id}\t{item.name}\t{item.xtype}\t{item.crdate}");
            }
        }
        #endregion 
    }
}
