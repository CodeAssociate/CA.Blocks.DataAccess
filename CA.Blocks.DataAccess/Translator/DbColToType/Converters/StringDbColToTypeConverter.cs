using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class StringDbColToTypeConverter : BaseDbColToTypeConverter<string>
    {
        public override string? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsString(columnName);
        }

        public override string? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsString(columnName);
        }

        public override string? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsString(columnIndex);
        }

        public override string? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsString(columnIndex);
        }
    }
}
