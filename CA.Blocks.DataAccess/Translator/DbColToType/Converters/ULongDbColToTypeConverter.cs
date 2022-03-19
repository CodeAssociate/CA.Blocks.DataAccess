using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ULongDbColToTypeConverter : BaseDbColToTypeConverter<ulong>
    {
        public override ulong GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsULong(columnName);
        }

        public override ulong GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsULong(columnName);
        }

        public override ulong  GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsULong(columnIndex);
        }

        public override ulong GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsULong(columnIndex);
        }

    }

    public class NullULongDbColToTypeConverter : BaseDbColToTypeConverter<ulong?>
    {
        public override ulong? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullULong(columnName);
        }

        public override ulong? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        public override ulong? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }

        public override ulong? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }
    }
}
