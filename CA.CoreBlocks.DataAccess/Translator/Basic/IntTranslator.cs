using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class IntTranslator : SimpleDbRow2ObjectTranslator<int>
    {
        private readonly string _colName;
        public IntTranslator(string colName)
        {
            _colName = colName;
        }

        protected override int CustomTranslate(DataRow dr)
        {
            return dr.AsInt(_colName);
        }
    }
}
