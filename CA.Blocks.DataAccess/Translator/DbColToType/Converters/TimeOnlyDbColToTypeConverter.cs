using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

#if NET6_0_OR_GREATER
    public class TimeOnlyDbColToTypeConverter : BaseDbColToTypeConverter<TimeOnly>
    {
        public override TimeOnly GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsTimeOnly(columnName);
        }

        public override TimeOnly GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsTimeOnly(columnName);
        }

        public override TimeOnly GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsTimeOnly(columnIndex);
        }

        public override TimeOnly GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsTimeOnly(columnIndex);
        }
    }

    public class NullTimeOnlyDbColToTypeConverter : BaseDbColToTypeConverter<TimeOnly?>
    {
        public override TimeOnly? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullTimeOnly(columnName);
        }

        public override TimeOnly? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullTimeOnly(columnName);
        }

        public override TimeOnly? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullTimeOnly(columnIndex);
        }

        public override TimeOnly? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullTimeOnly(columnIndex);
        }
    }
#endif
}
