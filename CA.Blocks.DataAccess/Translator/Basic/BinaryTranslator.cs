using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class BinaryTranslator : Db2SingleNamedColumnTranslator<byte[]>
    {
        public BinaryTranslator(string columnName) : base(new BinaryDbColToTypeConverter(), columnName)
        {

        }
    }
}
