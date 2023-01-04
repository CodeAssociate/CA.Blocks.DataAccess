using CA.Blocks.DataAccess.DataTableHelpers;
using System.Data;
using System.Reflection;

namespace CA.Blocks.DataAccessUnitTests.DataTableHelpers
{
    [TestFixture]
    public class DataTableHelpersUnitTests
    {
        [TestCase]
        public void ToValueDataTable_Int()
        {
            var testList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var result = testList.ToValueDataTable();

            Assert.IsNotNull(result);
            Assert.That(result.Columns.Count, Is.EqualTo(1));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("Value"));
            Assert.That(result.Columns[0].DataType, Is.EqualTo(typeof(int)));
            Assert.That(result.Rows.Count, Is.EqualTo(9));
        }

        [TestCase]
        public void ToValueDataTable_String()
        {
            var testList = new List<string> { "a", "b", "c", "d"};

            var result = testList.ToValueDataTable();

            Assert.IsNotNull(result);
            Assert.That(result.Columns.Count, Is.EqualTo(1));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("Value"));
            Assert.That(result.Columns[0].DataType, Is.EqualTo(typeof(string)));
            Assert.That(result.Rows.Count, Is.EqualTo(4));
        }


        public class TestComplexObject
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }



        [TestCase]
        public void ToDataTable_Object()
        {
            var testList = new List<TestComplexObject>
            {
                new TestComplexObject{Id = 1, Value = "a"},
                new TestComplexObject{Id = 2, Value = "b"},
                new TestComplexObject{Id = 3, Value = "c"},
                new TestComplexObject{Id = 4, Value = "d"},
            };

            var result = testList.ToObjectDataTable();

            Assert.IsNotNull(result);
            Assert.That(result.Columns.Count, Is.EqualTo(2));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("Id"));
            Assert.That(result.Columns[0].DataType, Is.EqualTo(typeof(int)));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("Value"));
            Assert.That(result.Columns[1].DataType, Is.EqualTo(typeof(string)));
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows[0][0], Is.EqualTo(1));
            Assert.That(result.Rows[0][1], Is.EqualTo("a"));
        }


        private static void CustomPopulateObject(DataRow target, TestComplexObject source)
        {
            target["Id"] = source.Id;
            target["Value"] = source.Value;
        }

        private static void SetupCustomObjectDataTable(DataTable target)
        {
            target.Columns.Add("Id", typeof(int));
            target.Columns.Add("Value", typeof(string));
        }

        [TestCase]
        public void ToDataTable_Object_Custom()
        {
            var testList = new List<TestComplexObject>
            {
                new TestComplexObject{Id = 1, Value = "a"},
                new TestComplexObject{Id = 2, Value = "b"},
                new TestComplexObject{Id = 3, Value = "c"},
                new TestComplexObject{Id = 4, Value = "d"},
            };

            var result = testList.ToObjectDataTable(SetupCustomObjectDataTable, CustomPopulateObject);

            Assert.IsNotNull(result);
            Assert.That(result.Columns.Count, Is.EqualTo(2));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("Id"));
            Assert.That(result.Columns[0].DataType, Is.EqualTo(typeof(int)));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("Value"));
            Assert.That(result.Columns[1].DataType, Is.EqualTo(typeof(string)));
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows[0][0], Is.EqualTo(1));
            Assert.That(result.Rows[0][1], Is.EqualTo("a"));
        }

    }
}
