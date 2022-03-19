using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class IntDbColToTypeConverter : BaseDbColToTypeConverter<int>
    {
        public override int GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsInt(columnName);
        }

        public override int GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsInt(columnName);
        }

        public override int GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsInt(columnIndex);
        }

        public override int GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsInt(columnIndex);
        }
    }

    public class NullIntDbColToTypeConverter : BaseDbColToTypeConverter<int?>
    {
        public override int? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullInt(columnName);
        }

        public override int? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullInt(columnName);
        }

        public override int? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullInt(columnIndex);
        }

        public override int? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullInt(columnIndex);
        }
    }
}
