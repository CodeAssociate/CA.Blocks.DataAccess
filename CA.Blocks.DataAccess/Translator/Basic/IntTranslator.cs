using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class IntTranslator : Db2SingleNamedColumnTranslator<int>
    {
        public IntTranslator(string columnName) : base(new IntDbColToTypeConverter(), columnName)
        {

        }

    }
}
