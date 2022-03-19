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

        public override byte GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsByte(columnIndex);
        }

        public override byte GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsByte(columnIndex);
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

        public override byte? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullByte(columnIndex);
        }

        public override byte? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullByte(columnIndex);
        }
    }
}
