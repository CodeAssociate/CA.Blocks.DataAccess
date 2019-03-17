using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class StringTranslator : SimpleDbRow2ObjectTranslator<string>
    {
        private readonly string _colName;
        public StringTranslator(string colName)
        {
            _colName = colName;
        }

        protected override string CustomTranslate(DataRow dr)
        {
            return dr.AsString(_colName);
        }
    }
}
