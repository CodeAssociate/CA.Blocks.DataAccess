using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class BinaryTranslator : SimpleDbRow2ObjectTranslator<byte[]>
    {
        private readonly string _colName;
        public BinaryTranslator(string colName)
        {
            _colName = colName;
        }

        protected override byte[] CustomTranslate(DataRow dr)
        {
            return dr.AsBinary(_colName);
        }
    }
}
