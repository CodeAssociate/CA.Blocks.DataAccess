using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{


    public class StringTranslator : Db2SingleNamedColumnTranslator<string>
    {
        public StringTranslator(string columnName) : base(new StringDbColToTypeConverter(), columnName,
            () => string.Empty)
        {

        }
    }
}
