using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class DecimalDbColToTypeConverter : BaseDbColToTypeConverter<Decimal>
    {
        public override Decimal GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsDecimal(columnName);
        }

        public override Decimal GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsDecimal(columnName);
        }

        public override Decimal GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsDecimal(columnIndex);
        }

        public override Decimal GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsDecimal(columnIndex);
        }
    }

    public class NullDecimalDbColToTypeConverter : BaseDbColToTypeConverter<Decimal?>
    {
        public override Decimal? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullDecimal(columnName);
        }

        public override Decimal? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullDecimal(columnName);
        }

        public override Decimal? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullDecimal(columnIndex);
        }

        public override Decimal? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullDecimal(columnIndex);
        }
    }
}
