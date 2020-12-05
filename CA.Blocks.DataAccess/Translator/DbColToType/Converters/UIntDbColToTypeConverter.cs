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
            return dr.AsUShort(columnName);
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
    }
}
