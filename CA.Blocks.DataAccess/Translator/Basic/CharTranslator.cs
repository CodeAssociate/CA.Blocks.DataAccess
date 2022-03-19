using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    //public class CharTranslator : SimpleDbRow2ObjectTranslator<char>
    //{
    //    private readonly string _colName;
    //    public CharTranslator(string colName)
    //    {
    //        _colName = colName;
    //    }

    //    protected override Char CustomTranslate(DataRow dr)
    //    {
    //        return dr.AsChar(_colName);
    //    }
    //}

    public class CharTranslator : Db2SingleNamedColumnTranslator<char>
    {
        public CharTranslator(string columnName) : base(new CharDbColToTypeConverter(), columnName)
        {

        }
    }
}
