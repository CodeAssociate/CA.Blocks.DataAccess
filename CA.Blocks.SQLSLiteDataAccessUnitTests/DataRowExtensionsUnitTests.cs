using System;
using System.Data;
using CA.Blocks.DataAccess;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests
{
    [TestFixture]
    public class DataRowExtensionsUnitTests
    {

        private DataTable CreateTestTable(Type dbType, object testData)
        {
            DataTable result = new DataTable();
            DataColumn dckey = new DataColumn("key", typeof(int));
            result.Columns.Add(dckey);
            DataColumn dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, null);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            return result;
        }

        [Test]
        public void GetValueFromRowAsDecimal_Tests()
        {
            decimal expected = (decimal)123.456;
            decimal? actual;
            var dt = CreateTestTable(typeof(decimal), expected);

            actual = dt.Rows[0].AsNullDecimal("col");
            Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[0].AsNullDecimal(1);
            Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[0].AsNullDecimal(dt.Columns["col"]);
            Assert.IsFalse(actual.HasValue);


            actual = dt.Rows[1].AsNullDecimal("col");
            Assert.AreEqual(expected, actual.Value);

            actual = dt.Rows[1].AsNullDecimal(1);
            Assert.AreEqual(expected, actual.Value);

            actual = dt.Rows[1].AsNullDecimal(dt.Columns["col"]);
            Assert.AreEqual(expected, actual.Value);
        }


        [Test]
        public void GetValueFromRowAsDouble_Tests()
        {
            Double expected = (Double)987.456;
            Double? actual;
            Double actual2;
            var dt = CreateTestTable(typeof(Double), expected);

            actual = dt.Rows[0].AsNullDouble("col");
            Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[0].AsNullDouble(1);
            Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[0].AsNullDouble(dt.Columns["col"]);
            Assert.IsFalse(actual.HasValue);


            actual = dt.Rows[1].AsNullDouble("col");
            Assert.AreEqual(expected, actual.Value);

            actual = dt.Rows[1].AsNullDouble(1);
            Assert.AreEqual(expected, actual.Value);

            actual = dt.Rows[1].AsNullDouble(dt.Columns["col"]);
            Assert.AreEqual(expected, actual.Value);

            actual2 = dt.Rows[1].AsDouble("col");
            Assert.AreEqual(expected, actual2);

            actual2 = dt.Rows[1].AsDouble(1);
            Assert.AreEqual(expected, actual2);

            actual2 = dt.Rows[1].AsDouble(dt.Columns["col"]);
            Assert.AreEqual(expected, actual2);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat_AsNullTests(int rowNumber, float? expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(float), expected);

            actual = dt.Rows[rowNumber].AsNullFloat("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullFloat(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullFloat(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (float)987.456)]
        public void GetValueFromRowAsFloat(int rowNumber, float expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(float), expected);

            actual = dt.Rows[rowNumber].AsNullFloat("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsNullFloat(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsNullFloat(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }



        [Test]
        [TestCase(0, null)]
        [TestCase(1, (long)123456789)]
        public void GetValueFromRowAsLong_AsNullTests(int rowNumber, long? expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(long), expected);

            actual = dt.Rows[rowNumber].AsNullLong("col");
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullLong(1);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);

            actual = dt.Rows[rowNumber].AsNullLong(dt.Columns["col"]);
            if (expected.HasValue)
                Assert.AreEqual(expected, actual.Value);
            else
                Assert.IsFalse(actual.HasValue);
        }

        [Test]
        [TestCase(1, (long)1234567890)]
        public void GetValueFromRowAsLong(int rowNumber, long expected)
        {
            float? actual;
            var dt = CreateTestTable(typeof(long), expected);

            actual = dt.Rows[rowNumber].AsLong("col");
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(1);
            Assert.AreEqual(expected, actual);

            actual = dt.Rows[rowNumber].AsLong(dt.Columns["col"]);
            Assert.AreEqual(expected, actual);
        }
    }
}