using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    /// <inheritdoc />
    public class UShortDbColToTypeConverter : BaseDbColToTypeConverter<ushort>
    {
        /// <inheritdoc />
        public override ushort GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsUShort(columnName);
        }

        /// <inheritdoc />
        public override ushort GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsUShort(columnName);
        }

        /// <inheritdoc />
        public override ushort GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsUShort(columnIndex);
        }

        /// <inheritdoc />
        public override ushort GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsUShort(columnIndex);
        }
    }
    /// <inheritdoc />
    public class NullUShortDbColToTypeConverter : BaseDbColToTypeConverter<ushort?>
    {
        /// <inheritdoc />
        public override ushort? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        /// <inheritdoc />
        public override ushort? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        /// <inheritdoc />
        public override ushort? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }

        /// <inheritdoc />
        public override ushort? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }
    }
}
