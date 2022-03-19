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

        public override sbyte GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsSbyte(columnIndex);
        }

        public override sbyte GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsSbyte(columnIndex);
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
        public override sbyte? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullSbyte(columnIndex);
        }

        public override sbyte? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullSbyte(columnIndex);
        }
    }
}
