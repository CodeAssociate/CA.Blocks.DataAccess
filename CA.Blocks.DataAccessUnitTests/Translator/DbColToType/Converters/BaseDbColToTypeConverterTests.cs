using System.Data;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    public class BaseDbColToTypeConverterTests
    {
        protected DataTable CreateTestTable(Type dbType, object? testData)
        {
            DataTable result = new DataTable();
            DataColumn dcKey = new DataColumn("key", typeof(int));
            result.Columns.Add(dcKey);
            DataColumn dc = new DataColumn("col", dbType);
            result.Columns.Add(dc);
            result.AcceptChanges();
            result.Rows.Add(1, null);
            result.Rows.Add(2, testData);
            result.AcceptChanges();
            return result;
        }

        protected DataRow GetDataRow(int rowNumber, DataTable sourceDataTable)
        {
            return sourceDataTable.Rows[rowNumber];
        }

        protected IDataReader GetDataReader(int rowNumber, DataTable sourceDataTable)
        {
            var datareader = sourceDataTable.CreateDataReader();
            for (int i = 0; i <= rowNumber; i++)
            {
                datareader.Read();
            }
            return datareader;
        }
    }


}
