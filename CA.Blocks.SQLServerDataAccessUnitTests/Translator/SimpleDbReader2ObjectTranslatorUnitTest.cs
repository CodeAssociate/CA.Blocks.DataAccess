using System.Data.SqlClient;
using System.Diagnostics;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccessUnitTests.Translator.TestObjects;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator
{
    [TestFixture]
    public class SimpleDbReader2ObjectTranslatorUnitTest : UnitTestDataAccess
    {


        #region TestSysobjects
        [Test]
        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping()
        {
            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
            var result = TestSysobjectsReaderTranslator.CurrentInstance.Translate(ExecuteReader(cmd));

            Assert.IsTrue(result.Count > 0);

            var outputformat = "{0}\t{1}\t{2}\t{3}";
            Trace.WriteLine(string.Format(outputformat, "id", "name", "xtype", "crdate"));
            foreach (var item in result)
            {
                TestContext.WriteLine(string.Format(outputformat, item.id, item.name, item.xtype, item.crdate));
            }
        }

        #endregion 
    }
}
