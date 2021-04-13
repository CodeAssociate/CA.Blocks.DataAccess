using System;
using System.Collections.Generic;
using System.Data;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator
{
    [TestFixture]
    public class BaseDb2ObjectTranslatorTests
    {

        private DataTable CreateTestTable(Type dbType, IList<object> testData)
        {
            DataTable result = new DataTable();
            DataColumn dckey = new DataColumn("Key", typeof(int));
            result.Columns.Add(dckey);
            DataColumn dc = new DataColumn("Value", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            for (int i = 1; i <= testData.Count; i++)
            {
                result.Rows.Add(i, testData[i -1]);

            }
            result.AcceptChanges();
            return result;
        }

        [Test]
        public void BaseDb2ObjectTranslator_StringTest()
        {
            // Setup
            var testData = CreateTestTable(typeof(String), new List<object> {"Test1", "Test2", "", null});
            var target = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TestStringClass>();
            //var target = new BaseDb2ObjectTranslator<TestStringClass>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Test1", result[0].Value);
            Assert.AreEqual("Test2", result[1].Value);
            Assert.AreEqual("", result[2].Value);
            Assert.AreEqual(null, result[3].Value);
        }



        [Test]
        public void BaseDb2ObjectTranslator_DateTime()
        {
            // Setup
            var dt1 = System.DateTime.Now;
            var dt2 = new DateTime(1999, 01, 02);
            var testData = CreateTestTable(typeof(DateTime), new List<object> { dt1, dt2 });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<DateTime>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(dt1, result[0].Value);
            Assert.AreEqual(dt2, result[1].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_NullDateTime()
        {
            // Setup
            var dt1 = System.DateTime.Now;
            var dt2 = new DateTime(1999, 01, 02);
            var testData = CreateTestTable(typeof(DateTime), new List<object> { dt1, dt2, null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<DateTime>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(dt1, result[0].Value);
            Assert.AreEqual(dt2, result[1].Value);
            Assert.AreEqual(null, result[2].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_Long()
        {
            // Setup
            var testData = CreateTestTable(typeof(long), new List<object> {0,  1, long.MaxValue, long.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<long>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(long.MaxValue, result[2].Value);
            Assert.AreEqual(long.MinValue, result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullLong()
        {
            // Setup
            var testData = CreateTestTable(typeof(long), new List<object> { 0, 1, long.MaxValue, long.MinValue, null });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<long>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(long.MaxValue, result[2].Value);
            Assert.AreEqual(long.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_Int()
        {
            // Setup
            var testData = CreateTestTable(typeof(int), new List<object> { 0, 1, int.MaxValue, int.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<int>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(int.MaxValue, result[2].Value);
            Assert.AreEqual(int.MinValue, result[3].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_NullInt()
        {
            // Setup
            var testData = CreateTestTable(typeof(int), new List<object> { 0, 1, int.MaxValue, int.MinValue, null });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<int>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(int.MaxValue, result[2].Value);
            Assert.AreEqual(int.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }



        [Test]
        public void BaseDb2ObjectTranslator_short()
        {
            // Setup
            var testData = CreateTestTable(typeof(short), new List<object> { 0, 1, short.MaxValue, short.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<short>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(short.MaxValue, result[2].Value);
            Assert.AreEqual(short.MinValue, result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullShort()
        {
            // Setup
            var testData = CreateTestTable(typeof(short), new List<object> { 0, 1, short.MaxValue, short.MinValue, null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<short>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(short.MaxValue, result[2].Value);
            Assert.AreEqual(short.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }



        [Test]
        public void BaseDb2ObjectTranslator_Byte()
        {
            // Setup
            var testData = CreateTestTable(typeof(byte), new List<object> { 0, 1, byte.MaxValue, byte.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<byte>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(byte.MaxValue, result[2].Value);
            Assert.AreEqual(byte.MinValue, result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullByte()
        {
            // Setup
            var testData = CreateTestTable(typeof(byte), new List<object> { 0, 1, byte.MaxValue, byte.MinValue, null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<byte>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(byte.MaxValue, result[2].Value);
            Assert.AreEqual(byte.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }



        [Test]
        public void BaseDb2ObjectTranslator_Bool()
        {
            // Setup
            var testData = CreateTestTable(typeof(bool), new List<object> {true, false});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<bool>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(true, result[0].Value);
            Assert.AreEqual(false, result[1].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullBool()
        {
            // Setup
            var testData = CreateTestTable(typeof(bool), new List<object> { true, false, null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<bool>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(true, result[0].Value);
            Assert.AreEqual(false, result[1].Value);
            Assert.AreEqual(null, result[2].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_char()
        {
            // Setup
            var testData = CreateTestTable(typeof(char), new List<object> { 'a', 'Z', '*', '~'});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<char>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual('a', result[0].Value);
            Assert.AreEqual('Z', result[1].Value);
            Assert.AreEqual('*', result[2].Value);
            Assert.AreEqual('~', result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_Nullchar()
        {
            // Setup
            var testData = CreateTestTable(typeof(char), new List<object> { 'a', 'Z', '*', '~', null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<char>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual('a', result[0].Value);
            Assert.AreEqual('Z', result[1].Value);
            Assert.AreEqual('*', result[2].Value);
            Assert.AreEqual('~', result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_TimeSpan()
        {
            // Setup
            var ts1 = new TimeSpan(1, 2, 3);
            var ts2 = new TimeSpan(1, 2, 3, 4, 5);
            var testData = CreateTestTable(typeof(TimeSpan), new List<object> { ts1, ts2 });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<TimeSpan>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(ts1, result[0].Value);
            Assert.AreEqual(ts2, result[1].Value);
        }


        [Test]
        public void BaseDb2ObjectTranslator_NullTimeSpan()
        {
            // Setup
            var ts1 = new TimeSpan(1, 2, 3);
            var ts2 = new TimeSpan(1, 2, 3, 4, 5);
            var testData = CreateTestTable(typeof(TimeSpan), new List<object> { ts1, ts2 , null});
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<TimeSpan>>();
            // Act
            var result = target.Translate(testData);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(ts1, result[0].Value);
            Assert.AreEqual(ts2, result[1].Value);
            Assert.AreEqual(null, result[2].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_Double()
        {
            // Setup
            var testData = CreateTestTable(typeof(double), new List<object> { 0, 1, double.MaxValue, double.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<double>>();
            // Act
            var result = target.Translate(testData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(double.MaxValue, result[2].Value);
            Assert.AreEqual(double.MinValue, result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullDouble()
        {
            // Setup
            var testData = CreateTestTable(typeof(double), new List<object> { 0, 1, double.MaxValue, double.MinValue, null });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<double>>();
            // Act
            var result = target.Translate(testData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(double.MaxValue, result[2].Value);
            Assert.AreEqual(double.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_Decimal()
        {
            // Setup
            var testData = CreateTestTable(typeof(decimal), new List<object> { 0, 1, decimal.MaxValue, decimal.MinValue });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestClass<decimal>>();
            // Act
            var result = target.Translate(testData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(decimal.MaxValue, result[2].Value);
            Assert.AreEqual(decimal.MinValue, result[3].Value);
        }

        [Test]
        public void BaseDb2ObjectTranslator_NullDecimal()
        {
            // Setup
            var testData = CreateTestTable(typeof(decimal), new List<object> { 0, 1, decimal.MaxValue, decimal.MinValue, null });
            var target = new BaseDb2ObjectTranslator<DbTranslateTestNullClass<decimal>>();
            // Act
            var result = target.Translate(testData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(0, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual(decimal.MaxValue, result[2].Value);
            Assert.AreEqual(decimal.MinValue, result[3].Value);
            Assert.AreEqual(null, result[4].Value);
        }
    }


    public class TestBaseClass
    {
        public int Key { get; set; }
    }

    public class DbTranslateTestClass<T> : TestBaseClass where T : struct
    {
        public T Value { get; set; }
    }

    public class DbTranslateTestNullClass<T> : TestBaseClass where T : struct
    {
        public T? Value { get; set; }
    }

    // Test Classes for string // string is not a struct
    public class TestStringClass : TestBaseClass
    {
        public string Value { get; set; }
    }
}
