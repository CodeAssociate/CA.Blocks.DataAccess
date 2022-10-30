using System.Data;

namespace CA.Blocks.DataAccessUnitTests.TestData
{
    public static class TestDataGenerator
    {
        public static DataTable GenerateTestDataForTestDataClassAsDataTable(int start, int count)
        {
            var testData = new DataTable();
            testData.Columns.Add("IntCol", typeof(int));
            testData.Columns.Add("StringCol", typeof(string));
            testData.Columns.Add("GuidCol", typeof(Guid));
            testData.Columns.Add("DateCol", typeof(DateTime));
            testData.AcceptChanges();
            for (var i = start; i <= (start + count - 1); i++)
            {
                testData.Rows.Add(i, $"row#{i}", Guid.NewGuid(), DateTime.Now.AddMinutes(i));
            }
            testData.AcceptChanges();
            return testData;
        }
        public static IDataReader GenerateTestDataForTestDataClassAsDataReader(int start, int count)
        {
            return GenerateTestDataForTestDataClassAsDataTable(start, count).CreateDataReader();
        }

    }
}
