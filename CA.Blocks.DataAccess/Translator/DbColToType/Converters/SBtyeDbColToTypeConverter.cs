using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class SByteDbColToTypeConverter : BaseDbColToTypeConverter<sbyte>
    {
        public override sbyte GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsSbyte(columnName);
        }

        public override sbyte GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsSbyte(columnName);
        }
    }

    public class NullSByteDbColToTypeConverter : BaseDbColToTypeConverter<sbyte?>
    {
        public override sbyte? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullSbyte(columnName);
        }

        public override sbyte? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullSbyte(columnName);
        }
    }
}
