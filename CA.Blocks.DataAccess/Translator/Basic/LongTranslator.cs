using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class LongTranslator : Db2SingleNamedColumnTranslator<long>
    {
        public LongTranslator(string columnName) : base(new LongDbColToTypeConverter(), columnName, () => 0)
        {

        }
    }
}
