using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    public class LongTranslator : SimpleDbRow2ObjectTranslator<long>
    {
        private readonly string _colName;
        public LongTranslator(string colName)
        {
            _colName = colName;
        }

        protected override long CustomTranslate(DataRow dr)
        {
            return dr.AsLong(_colName);
        }
    }
}
