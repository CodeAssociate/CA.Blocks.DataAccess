using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class UShortDbColToTypeConverter : BaseDbColToTypeConverter<ushort>
    {
        public override ushort GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsUShort(columnName);
        }

        public override ushort GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsUShort(columnName);
        }

        public override ushort GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsUShort(columnIndex);
        }

        public override ushort GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsUShort(columnIndex);
        }
    }

    public class NullUShortDbColToTypeConverter : BaseDbColToTypeConverter<ushort?>
    {
        public override ushort? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        public override ushort? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        public override ushort? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }

        public override ushort? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }
    }
}
