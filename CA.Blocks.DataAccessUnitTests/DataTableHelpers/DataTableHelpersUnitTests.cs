using CA.Blocks.DataAccess.DataTableHelpers;
using System.Data;

namespace CA.Blocks.DataAccessUnitTests.DataTableHelpers
{
        public class DataTableHelpersUnitTests
    {
        [Fact]
        public void ToValueDataTable_Int()
        {
            var testList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var result = testList.ToValueDataTable();

            Assert.NotNull(result);
            Assert.Equal(1, result.Columns.Count);
            Assert.Equal("Value", result.Columns[0].ColumnName);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal(9, result.Rows.Count);
        }
        
        
        [Fact]
        public void ToValueDataTable_NullInt()
        {
            var testList = new List<int?> { null, 1 };

            var result = testList.ToValueDataTable();

            Assert.NotNull(result);
            Assert.Equal(1, result.Columns.Count);
            Assert.Equal("Value", result.Columns[0].ColumnName);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal(2, result.Rows.Count);
        }
        
        [Fact]
        public void ToValueDataTable_String()
        {
            var testList = new List<string> { "a", "b", "c", "d"};

            var result = testList.ToValueDataTable();

            Assert.NotNull(result);
            Assert.Equal(1, result.Columns.Count);
            Assert.Equal("Value", result.Columns[0].ColumnName);
            Assert.Equal(typeof(string), result.Columns[0].DataType);
            Assert.Equal(4, result.Rows.Count);
        }


        public class TestComplexObject
        {
            public int Id { get; set; }
            public string Value { get; set; } = null!;
        }
        
        [Fact]
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

            Assert.NotNull(result);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Id", result.Columns[0].ColumnName);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal("Value", result.Columns[1].ColumnName);
            Assert.Equal(typeof(string), result.Columns[1].DataType);
            Assert.Equal(4, result.Rows.Count);
            Assert.Equal(1, result.Rows[0][0]);
            Assert.Equal("a", result.Rows[0][1]);
        }

        public class TestComplexObjectWithNull
        {
            public int? Id { get; set; }
            public string? Value { get; set; } = null!;
        }
        
        [Fact]
        public void ToDataTable_ObjectWithNull()
        {
            var testList = new List<TestComplexObjectWithNull>
            {
                new TestComplexObjectWithNull{Id = 1, Value = null},
                new TestComplexObjectWithNull{Id = null, Value = "b"},
                new TestComplexObjectWithNull{Id = 3, Value = "c"},
                new TestComplexObjectWithNull{Id = null, Value = null },
            };

            var result = testList.ToObjectDataTable();

            Assert.NotNull(result);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Id", result.Columns[0].ColumnName);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal("Value", result.Columns[1].ColumnName);
            Assert.Equal(typeof(string), result.Columns[1].DataType);
            Assert.Equal(4, result.Rows.Count);
            Assert.Equal(1, result.Rows[0][0]);
            Assert.Equal(DBNull.Value, result.Rows[0][1]);
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

        [Fact]
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

            Assert.NotNull(result);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Id", result.Columns[0].ColumnName);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal("Value", result.Columns[1].ColumnName);
            Assert.Equal(typeof(string), result.Columns[1].DataType);
            Assert.Equal(4, result.Rows.Count);
            Assert.Equal(1, result.Rows[0][0]);
            Assert.Equal("a", result.Rows[0][1]);
        }

    }
}
