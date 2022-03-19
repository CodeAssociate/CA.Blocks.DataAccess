using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class UIntDbColToTypeConverter : BaseDbColToTypeConverter<uint>
    {
        public override uint GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsUInt(columnName);
        }

        public override uint GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsUInt(columnName);
        }

        public override uint GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsUInt(columnIndex);
        }

        public override uint GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsUInt(columnIndex);
        }
    }

    public class NullUIntDbColToTypeConverter : BaseDbColToTypeConverter<uint?>
    {
        public override uint? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullUInt(columnName);
        }

        public override uint? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUInt(columnName);
        }

        public override uint? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUInt(columnIndex);
        }

        public override uint? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUInt(columnIndex);
        }
    }
}
