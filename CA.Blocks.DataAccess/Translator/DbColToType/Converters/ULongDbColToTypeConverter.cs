using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

    /// <inheritdoc />
    public class ULongDbColToTypeConverter : BaseDbColToTypeConverter<ulong>
    {
        /// <inheritdoc />
        public override ulong GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsULong(columnName);
        }
        /// <inheritdoc />
        public override ulong GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsULong(columnName);
        }
        /// <inheritdoc />
        public override ulong  GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsULong(columnIndex);
        }
        /// <inheritdoc />
        public override ulong GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsULong(columnIndex);
        }
    }

    /// <inheritdoc />
    public class NullULongDbColToTypeConverter : BaseDbColToTypeConverter<ulong?>
    {
        /// <inheritdoc />
        public override ulong? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullULong(columnName);
        }

        /// <inheritdoc />
        public override ulong? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUShort(columnName);
        }

        /// <inheritdoc />
        public override ulong? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }

        /// <inheritdoc />
        public override ulong? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUShort(columnIndex);
        }
    }
}
