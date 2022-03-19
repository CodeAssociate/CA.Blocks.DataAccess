using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class ShortTranslator : Db2SingleNamedColumnTranslator<short>
    {
        public ShortTranslator(string columnName) : base(new ShortDbColToTypeConverter(), columnName)
        {

        }
    }
}
