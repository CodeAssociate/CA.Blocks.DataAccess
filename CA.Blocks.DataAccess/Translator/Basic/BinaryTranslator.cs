using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    //public class BinaryTranslator : SimpleDbRow2ObjectTranslator<byte[]>
    //{
    //    private readonly string _colName;
    //    public BinaryTranslator(string colName)
    //    {
    //        _colName = colName;
    //    }

    //    protected override byte[] CustomTranslate(DataRow dr)
    //    {
    //        return dr.AsBinary(_colName);
    //    }
    //}

    public class BinaryTranslator : Db2SingleNamedColumnTranslator<byte[]>
    {
        public BinaryTranslator(string columnName) : base(new BinaryDbColToTypeConverter(), columnName)
        {

        }
    }
}
