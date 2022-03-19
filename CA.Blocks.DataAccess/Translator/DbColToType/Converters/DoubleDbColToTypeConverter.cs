using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class DoubleDbColToTypeConverter : BaseDbColToTypeConverter<Double>
    {
        public override Double GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsDouble(columnName);
        }

        public override Double GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsDouble(columnName);
        }

        public override Double GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsDouble(columnIndex);
        }

        public override Double GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsDouble(columnIndex);
        }
    }

    public class NullDoubleDbColToTypeConverter : BaseDbColToTypeConverter<Double?>
    {
        public override Double? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullDouble(columnName);
        }

        public override Double? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullDouble(columnName);
        }
        public override Double? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullDouble(columnIndex);
        }

        public override Double? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullDouble(columnIndex);
        }
    }
}
