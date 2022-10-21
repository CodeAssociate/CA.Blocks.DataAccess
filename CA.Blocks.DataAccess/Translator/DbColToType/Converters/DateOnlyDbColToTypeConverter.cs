using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

#if NET6_0_OR_GREATER
    public class DateOnlyDbColToTypeConverter : BaseDbColToTypeConverter<DateOnly>
    {
        public override DateOnly GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsDateOnly(columnName);
        }

        public override DateOnly GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsDateOnly(columnName);
        }

        public override DateOnly GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsDateOnly(columnIndex);
        }

        public override DateOnly GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsDateOnly(columnIndex);
        }
    }

    public class NullDateOnlyDbColToTypeConverter : BaseDbColToTypeConverter<DateOnly?>
    {
        public override DateOnly? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullDateOnly(columnName);
        }

        public override DateOnly? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullDateOnly(columnName);
        }

        public override DateOnly? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullDateOnly(columnIndex);
        }

        public override DateOnly? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullDateOnly(columnIndex);
        }
    }
#endif
}
