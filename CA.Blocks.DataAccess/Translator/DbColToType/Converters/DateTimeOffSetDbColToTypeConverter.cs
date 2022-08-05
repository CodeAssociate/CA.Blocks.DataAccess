using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

    public class DateTimeOffSetDbColToTypeConverter : BaseDbColToTypeConverter<DateTimeOffset>
    {
        public override DateTimeOffset GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsDateTimeOffset(columnName);
        }

        public override DateTimeOffset GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsDateTimeOffset(columnName);
        }

        public override DateTimeOffset GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsDateTimeOffset(columnIndex);
        }

        public override DateTimeOffset GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsDateTimeOffset(columnIndex);
        }
    }

    public class NullDateTimeOffSetDbColToTypeConverter : BaseDbColToTypeConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullDateTimeOffset(columnName);
        }

        public override DateTimeOffset? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullDateTimeOffset(columnName);
        }

        public override DateTimeOffset? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullDateTimeOffset(columnIndex);
        }

        public override DateTimeOffset? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullDateTimeOffset(columnIndex);
        }
    }
}