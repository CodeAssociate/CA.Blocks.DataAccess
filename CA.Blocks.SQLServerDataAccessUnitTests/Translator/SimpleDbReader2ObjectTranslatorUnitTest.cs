//using Microsoft.Data.SqlClient;
//using System.Diagnostics;
//using CA.Blocks.SQLServerDataAccessUnitTests.Base;
//using NUnit.Framework;

//namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator
//{
//    [TestFixture]
//    public class SimpleDbReader2ObjectTranslatorUnitTest : UnitTestDataAccess
//    {



//        #region TestSysobjects
//        [Test]
//        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping()
//        {
//            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
//            var result = TestSysobjectsReaderTranslator.CurrentInstance.Translate(ExecuteReader(cmd));

//            ClassicAssert.IsTrue(result.Count > 0);

//            var outputformat = "{0}\t{1}\t{2}\t{3}";
//            Trace.WriteLine(string.Format(outputformat, "id", "name", "xtype", "crdate"));
//            foreach (var item in result)
//            {
//                TestContext.WriteLine(string.Format(outputformat, item.id, item.name, item.xtype, item.crdate));
//            }
//        }

//        #endregion

//        //private string benchmarkSQL = "Select id,name,xtype,crdate from sysobjects";

//        //public void ExecuteReader()
//        //{
//        //    SqlCommand cmd = CreateTextCommand(benchmarkSQL);
//        //    var result = TestSysobjectsReaderTranslator.CurrentInstance.Translate(ExecuteReader(cmd));

//        //    ClassicAssert.IsTrue(result.Count > 0);

//        //}

//        //public void ExecuteReaderByOrdinal()
//        //{
//        //    SqlCommand cmd = CreateTextCommand(benchmarkSQL);
//        //    var result = TestSysobjectsOrginalReaderTranslator.CurrentInstance.Translate(ExecuteReader(cmd));

//        //    ClassicAssert.IsTrue(result.Count > 0);

//        //}

//        //public void ExecuteDataTable()
//        //{
//        //    SqlCommand cmd = CreateTextCommand(benchmarkSQL);
//        //    var result = TestSysobjectsTranslator.CurrentInstance.Translate(ExecuteDataTable(cmd));

//        //    ClassicAssert.IsTrue(result.Count > 0);
//        //}

//        //public void ExecuteTo()
//        //{
//        //    SqlCommand cmd = CreateTextCommand(benchmarkSQL);
//        //    var result = ExecuteToListOf<TestSysobjects>(cmd);

//        //    ClassicAssert.IsTrue(result.Count > 0);
//        //}



        
//        //[Test]
//        //public void Execute2DataTableBenchMark()
//        //{
//        //    Stopwatch sw = new Stopwatch();
//        //    sw.Start();
//        //    for (int i = 0; i < 1000; i++)
//        //    {
//        //        ExecuteDataTable();
//        //    }
//        //    sw.Stop();
//        //    TestContext.WriteLine($"{sw.ElapsedMilliseconds}");
//        //}


//        //[Test]
//        //public void Execute0ReaderBenchMark()
//        //{
//        //    Stopwatch sw = new Stopwatch();
//        //    sw.Start();
//        //    for(int i = 0; i < 1000; i++)
//        //    {
//        //        ExecuteReader();
//        //    }
//        //    sw.Stop();
//        //    TestContext.WriteLine($"{sw.ElapsedMilliseconds}");
//        //}
//        //[Test]
//        //public void Execute1ReaderBenchMark()
//        //{
//        //    Stopwatch sw = new Stopwatch();
//        //    sw.Start();
//        //    for (int i = 0; i < 1000; i++)
//        //    {
//        //        ExecuteReaderByOrdinal();
//        //    }
//        //    sw.Stop();
//        //    TestContext.WriteLine($"{sw.ElapsedMilliseconds}");
//        //}


//        //[Test]
//        //public void Execute3ToBenchMark()
//        //{
//        //    Stopwatch sw = new Stopwatch();
//        //    sw.Start();
//        //    for (int i = 0; i < 1000; i++)
//        //    {
//        //        ExecuteTo();
//        //    }
//        //    sw.Stop();
//        //    TestContext.WriteLine($"{sw.ElapsedMilliseconds}");
//        //}



//    }
//}
