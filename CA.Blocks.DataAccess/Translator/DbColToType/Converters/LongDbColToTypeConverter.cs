using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class LongDbColToTypeConverter : BaseDbColToTypeConverter<long>
    {
        public override long GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsLong(columnName);
        }

        public override long GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsLong(columnName);
        }
    }

    public class NullLongDbColToTypeConverter : BaseDbColToTypeConverter<long?>
    {
        public override long? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullLong(columnName);
        }

        public override long? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullLong(columnName);
        }
    }
}
