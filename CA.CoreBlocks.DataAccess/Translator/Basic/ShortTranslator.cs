using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class ShortTranslator : SimpleDbRow2ObjectTranslator<short>
    {
        private readonly string _colName;
        public ShortTranslator(string colName)
        {
            _colName = colName;
        }

        protected override short CustomTranslate(DataRow dr)
        {
            return dr.AsShort(_colName);
        }
    }
}
