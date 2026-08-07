using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    public class ByteTranslator : Db2SingleNamedColumnTranslator<Byte>
    {
        public ByteTranslator(string columnName) : base(new ByteDbColToTypeConverter(), columnName, () => 0)
        {

        }
    }
}
