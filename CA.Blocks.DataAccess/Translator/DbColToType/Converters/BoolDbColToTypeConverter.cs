using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class BoolDbColToTypeConverter : BaseDbColToTypeConverter<bool>
    {
        public override bool GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsBool(columnName);
        }

        public override bool GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsBool(columnName);
        }
    }

    public class NullBoolDbColToTypeConverter : BaseDbColToTypeConverter<bool?>
    {
        public override bool? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullBool(columnName);
        }

        public override bool? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullBool(columnName);
        }
    }
}
