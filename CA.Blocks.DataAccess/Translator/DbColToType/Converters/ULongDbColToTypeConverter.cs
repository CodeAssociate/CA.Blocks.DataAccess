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
    }
}
