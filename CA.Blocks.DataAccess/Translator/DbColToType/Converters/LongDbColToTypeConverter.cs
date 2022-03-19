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

        public override long GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsLong(columnIndex);
        }

        public override long GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsLong(columnIndex);
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
        public override long? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullLong(columnIndex);
        }

        public override long? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullLong(columnIndex);
        }
    }
}
