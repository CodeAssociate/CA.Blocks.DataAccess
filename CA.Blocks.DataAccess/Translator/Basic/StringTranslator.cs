using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    //public class StringTranslator : SimpleDbRow2ObjectTranslator<string>
    //{
    //    private readonly string _colName;
    //    public StringTranslator(string colName)
    //    {
    //        _colName = colName;
    //    }

    //    protected override string CustomTranslate(DataRow dr)
    //    {
    //        return dr.AsString(_colName);
    //    }
    //}

    public class StringTranslator : Db2SingleNamedColumnTranslator<string>
    {
        public StringTranslator(string columnName) : base(new StringDbColToTypeConverter(), columnName)
        {

        }
    }
}
