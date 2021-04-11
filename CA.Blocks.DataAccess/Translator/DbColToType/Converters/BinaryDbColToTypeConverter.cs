using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class BinaryDbColToTypeConverter : BaseDbColToTypeConverter<byte[]>
    {
        public override byte[] GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsBinary(columnName);
        }

        public override byte[] GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsBinary(columnName);
        }
    }
}
