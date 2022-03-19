using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ShortDbColToTypeConverter : BaseDbColToTypeConverter<short>
    {
        public override short GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsShort(columnName);
        }

        public override short GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsShort(columnName);
        }

        public override short GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsShort(columnIndex);
        }

        public override short GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsShort(columnIndex);
        }
    }

    public class NullShortDbColToTypeConverter : BaseDbColToTypeConverter<short?>
    {
        public override short? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullShort(columnName);
        }

        public override short? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullShort(columnName);
        }

        public override short? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullShort(columnIndex);
        }

        public override short? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullShort(columnIndex);
        }
    }
}
