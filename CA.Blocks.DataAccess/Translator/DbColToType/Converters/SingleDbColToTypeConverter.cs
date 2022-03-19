using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class SingleDbColToTypeConverter : BaseDbColToTypeConverter<Single>
    {
        public override Single GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsSingle(columnName);
        }

        public override Single GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsSingle(columnName);
        }

        public override Single GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsSingle(columnIndex);
        }

        public override Single GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsSingle(columnIndex);
        }

    }

    public class NullSingleDbColToTypeConverter : BaseDbColToTypeConverter<Single?>
    {
        public override Single? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullSingle(columnName);
        }

        public override Single? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullSingle(columnName);
        }

        public override Single? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullSingle(columnIndex);
        }

        public override Single? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullSingle(columnIndex);
        }
    }
}
