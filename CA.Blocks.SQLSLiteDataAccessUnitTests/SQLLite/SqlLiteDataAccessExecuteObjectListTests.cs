using System.Diagnostics;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.SQLLiteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.SQLLite
{
    internal class Sysobjects
    {
        public string name { get; set; }
        public string  type { get; set; }
        public string rootpage { get; set; }
    }

    // Shows how to use the ExecuteObjectList to get a list of dyanmic objects from a SQL query.
    // This is handy for very quick development
    [TestFixture]
    public class SqlLiteDataAccessExecuteObjectListTests : UnitTestDataAccess
    {


        [Test]
        public void GetDataAsDyanmicList()
        {
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = ExecuteObjectList(cmd);
            foreach (var o in result)
            {
                string s = o.name;
                Trace.WriteLine(s);
            }
        }

        // This will use the same dynamic function except it adds a little more work in converting the 
        // object into a known type rather than dyanmic
        [Test]
        public void GetDataAsFixedTypeFromDynamic()
        {
              // Note we only support 1-1 mapping for now
            var t = new BaseDb2ObjectTranslator<Sysobjects>(); // 
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = t.Translate(ExecuteDataTable(cmd)); //<< inject a filter with parameters..

            foreach (var o in result)
            {
                string s = o.name;
                Trace.WriteLine(s);
            }
        }
    }


}
