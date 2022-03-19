using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class CharDbColToTypeConverter : BaseDbColToTypeConverter<char>
    {
        public override char GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsChar(columnName);
        }

        public override char GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsChar(columnName);
        }

        public override char GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsChar(columnIndex);
        }

        public override char GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsChar(columnIndex);
        }
    }

    public class NullCharDbColToTypeConverter : BaseDbColToTypeConverter<char?>
    {
        public override char? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullChar(columnName);
        }

        public override char? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullChar(columnName);
        }

        public override char? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullChar(columnIndex);
        }

        public override char? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullChar(columnIndex);
        }
    }
}
