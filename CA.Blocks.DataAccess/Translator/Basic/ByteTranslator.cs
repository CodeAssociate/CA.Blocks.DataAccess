using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    //public class ByteTranslator : SimpleDbRow2ObjectTranslator<Byte>
    //{
    //    private readonly string _colName;
    //    public ByteTranslator(string colName)
    //    {
    //        _colName = colName;
    //    }

    //    protected override Byte CustomTranslate(DataRow dr)
    //    {
    //        return dr.AsByte(_colName);
    //    }
    //}

    public class ByteTranslator : Db2SingleNamedColumnTranslator<Byte>
    {
        public ByteTranslator(string columnName) : base(new ByteDbColToTypeConverter(), columnName)
        {

        }
    }
}
