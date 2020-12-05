using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ByteDbColToTypeConverter : BaseDbColToTypeConverter<byte>
    {
        public override byte GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsByte(columnName);
        }

        public override byte GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsByte(columnName);
        }
    }

    public class NullByteDbColToTypeConverter : BaseDbColToTypeConverter<byte?>
    {
        public override byte? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullByte(columnName);
        }

        public override byte? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullByte(columnName);
        }
    }
}
