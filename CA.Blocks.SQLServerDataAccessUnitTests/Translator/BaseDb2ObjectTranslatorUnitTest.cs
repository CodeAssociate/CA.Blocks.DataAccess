//using System;
//using Microsoft.Data.SqlClient;
//using System.Diagnostics;
//using CA.Blocks.SQLServerDataAccessUnitTests.Base;

//using NUnit.Framework;

//namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator
//{
//    public class TestClassChar
//    {
//        public char Col { get; set; }
//    }

//    public class TestClassTimeSpan
//    {
//        public TimeSpan Col { get; set; }
//    }



//    //    public class BaseDb2ObjectTranslatorUnitTest : UnitTestDataAccess
//    {
//        //private const string unitTestTableName = "CA_BLOCKS_UNITTEST_TEMP_TESTTABLE";

//        #region TestSysobjects
//        [Fact]
//        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping()
//        {
//            var t = new BaseDb2ObjectTranslator<TestSysobjects>();
//            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
//            var result = t.Translate(ExecuteDataTable(cmd));

//            Assert.True(result.Count > 0);

//            var outputFormat = "{0}\t{1}\t{2}\t{3}";
//            Trace.WriteLine(string.Format(outputFormat, "id", "name", "xtype", "crdate"));
//            foreach (var item in result)
//            {
//                Trace.WriteLine(string.Format(outputFormat, item.id, item.name, item.xtype, item.crdate));
//            }
//        }


//        [Fact]
//        public void BaseDb2ObjectTranslatorTestTestSysobjectsMapping2()
//        {
//            var t = new CustomTestSysobjectsTranslator();
//            SqlCommand cmd = CreateTextCommand("Select * from sysobjects");
//            var result = t.Translate(ExecuteDataTable(cmd));

//            Assert.True(result.Count > 0);

//            var outputFormat = "{0}\t{1}\t{2}\t{3}\t{4}";
//            Console.WriteLine(string.Format(outputFormat, "id", "name", "xtype", "crdate", "CrazyNameForRefDate"));
//            foreach (var item in result)
//            {
//                Console.WriteLine(string.Format(outputFormat, item.id, item.name, item.xtype, item.crdate.Date, item.CrazyNameForRefDate));
//            }
//        }
//        #endregion




//        #region Char
//        [Fact]
//        public void BaseDb2ObjectTranslatorTestCharMapping()
//        {
//            var da = new UnitTestDataAccess();
//            da.ExecuteNonQuery(DropTestTableSQL());
//            da.ExecuteNonQuery(CreateTestTable("char(1)"));
//            da.ExecuteNonQuery(InsertTestDataSQL("'T'"));

//            var target = new BaseDb2ObjectTranslator<TestClassChar>();
//            SqlCommand cmd = CreateTextCommand(SelectTestDataSQL());
//            var result = target.Translate(ExecuteDataRow(cmd));
            
//            Assert.Equal('T', result.Col);

//            da.ExecuteNonQuery(DropTestTableSQL());
//        }

//        #endregion 

//        #region timespan

//        [Fact]
//        public void BaseDb2ObjectTranslatorTestTimeSpanMapping()
//        {
//            var da = new UnitTestDataAccess();
//            da.ExecuteNonQuery(DropTestTableSQL());
//            da.ExecuteNonQuery(CreateTestTable("time(0)"));
//            da.ExecuteNonQuery(InsertTestDataSQL("'01:02:03'"));

//            var target = new BaseDb2ObjectTranslator<TestClassTimeSpan>();
//            var cmd = CreateTextCommand(SelectTestDataSQL());
//            var result = target.Translate(ExecuteDataRow(cmd));

//            Assert.Equal(new TimeSpan(1,2,3), result.Col);

//            da.ExecuteNonQuery(DropTestTableSQL());
//        }


//        #endregion

//        #region more to come when i get time

//        #endregion 
//    }
//}




