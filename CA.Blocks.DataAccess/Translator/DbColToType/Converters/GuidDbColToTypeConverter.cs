using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class GuidDbColToTypeConverter : BaseDbColToTypeConverter<Guid>
    {
        public override Guid GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsGuid(columnName);
        }

        public override Guid GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsGuid(columnName);
        }

        public override Guid GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsGuid(columnIndex);
        }

        public override Guid GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsGuid(columnIndex);
        }
    }

    public class NullGuidDbColToTypeConverter : BaseDbColToTypeConverter<Guid?>
    {
        public override Guid? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullGuid(columnName);
        }

        public override Guid? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullGuid(columnName);
        }

        public override Guid? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullGuid(columnIndex);
        }

        public override Guid? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullGuid(columnIndex);
        }
    }
}
