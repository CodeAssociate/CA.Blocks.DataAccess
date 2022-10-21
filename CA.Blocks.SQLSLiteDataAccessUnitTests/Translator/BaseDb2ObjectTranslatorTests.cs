//using System;
//using System.Collections.Generic;
//using System.Data;
//using CA.Blocks.DataAccess.Translator;
//using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
//using NUnit.Framework;

//namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator
//{
//    [TestFixture]
//    public class BaseDb2ObjectTranslatorTests
//    {

//        private DataTable CreateTestTable(Type dbType, IList<object> testData)
//        {
//            DataTable result = new DataTable();
//            DataColumn dckey = new DataColumn("Key", typeof(int));
//            result.Columns.Add(dckey);
//            DataColumn dc = new DataColumn("Value", dbType);
//            result.Columns.Add(dc);
//            result.AcceptChanges();
//            for (int i = 1; i <= testData.Count; i++)
//            {
//                result.Rows.Add(i, testData[i -1]);

//            }
//            result.AcceptChanges();
//            return result;
//        }

//        [Test]
//        public void BaseDb2ObjectTranslator_StringTest()
//        {
//            // Setup
//            var testData = CreateTestTable(typeof(String), new List<object> {"Test1", "Test2", "", null});
//            var target = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TestStringClass>();
//            //var target = new BaseDb2ObjectTranslator<TestStringClass>();
//            // Act
//            var result = target.Translate(testData);
//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(4, result.Count);
//            Assert.AreEqual("Test1", result[0].Value);
//            Assert.AreEqual("Test2", result[1].Value);
//            Assert.AreEqual("", result[2].Value);
//            Assert.AreEqual(null, result[3].Value);
//        }
//    }


//    public class TestBaseClass
//    {
//        public int Key { get; set; }
//    }

//    public class DbTranslateTestClass<T> : TestBaseClass where T : struct
//    {
//        public T Value { get; set; }
//    }

//    public class DbTranslateTestNullClass<T> : TestBaseClass where T : struct
//    {
//        public T? Value { get; set; }
//    }

//    // Test Classes for string // string is not a struct
//    public class TestStringClass : TestBaseClass
//    {
//        public string Value { get; set; }
//    }
//}
