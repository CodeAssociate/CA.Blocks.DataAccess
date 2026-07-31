//using Microsoft.Data.SqlClient;
//using System.Diagnostics;
//using CA.Blocks.DataAccess.Translator;
//using CA.Blocks.DataAccess.Translator.Extensions;
//using CA.Blocks.SQLServerDataAccessUnitTests.Base;
//using NUnit.Framework;

//namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
//{
//    internal class Sysobjects
//    {
//        public string name { get; set; }
//        public int id { get; set; }
//        public string xtype { get; set; }
//    }

//    // Shows how to use the ExecuteObjectList to get a list of dyanmic objects from a SQL query.
//    // This is handy for very quick development
//    //    public class SqlServerDataAccessExecuteObjectListTests : UnitTestDataAccess
//    {


//        [Fact]
//        public void GetDataAsDyanmicList()
//        {
//            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
//            var result = ExecuteObjectList(cmd);
//            foreach (var o in result)
//            {
//                string s = o.name;
//                Trace.WriteLine(s);
//            }
//        }

//        // This will use the same dynamic function except it adds a little more work in converting the 
//        // object into a known type rather than dyanmic
//        [Fact]
//        public void GetDataAsFixedTypeFromDynamic()
//        {
//              // Note we only support 1-1 mapping for now
//            var t = new BaseDb2ObjectTranslator<Sysobjects>(); // <<-- need to have the ability to inject the map. 
//            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
//            var result = t.Translate(Execute(cmd).ToDataTable()); //<< inject a filter with parameters..

//            foreach (var o in result)
//            {
//                string s = o.name;
//                Trace.WriteLine(s);
//            }
//        }
//    }


//}




