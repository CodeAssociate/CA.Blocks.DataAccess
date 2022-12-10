using CA.Blocks.DataAccess.DataTableHelpers;

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
    }
}
