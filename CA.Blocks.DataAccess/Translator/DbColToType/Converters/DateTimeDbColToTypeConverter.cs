using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class DateTimeDbColToTypeConverter : BaseDbColToTypeConverter<DateTime>
    {
        public override DateTime GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsDateTime(columnName);
        }

        public override DateTime GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsDateTime(columnName);
        }
    }

    public class NullDateTimeDbColToTypeConverter : BaseDbColToTypeConverter<DateTime?>
    {
        public override DateTime? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullDateTime(columnName);
        }

        public override DateTime? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullDateTime(columnName);
        }
    }
}
