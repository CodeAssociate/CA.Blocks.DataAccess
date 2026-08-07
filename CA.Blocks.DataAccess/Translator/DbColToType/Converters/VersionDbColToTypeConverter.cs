using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class VersionDbColToTypeConverter : BaseDbColToTypeConverter<Version>
    {
        private Version? ToVersion(string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? null : new Version(input);
        }

        public override Version? GetDataValue(DataRow dr, string columnName)
        {
            return ToVersion(dr.AsString(columnName));
        }

        public override Version? GetDataValue(IDataReader dr, string columnName)
        {
            return ToVersion(dr.AsString(columnName));
        }

        public override Version? GetDataValue(DataRow dr, int columnIndex)
        {
            return ToVersion(dr.AsString(columnIndex));
        }

        public override Version? GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToVersion(dr.AsString(columnIndex));
        }
    }
}