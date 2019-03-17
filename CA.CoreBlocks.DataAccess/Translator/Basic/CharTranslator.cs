using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    public class CharTranslator : SimpleDbRow2ObjectTranslator<char>
    {
        private readonly string _colName;
        public CharTranslator(string colName)
        {
            _colName = colName;
        }

        protected override Char CustomTranslate(DataRow dr)
        {
            return dr.AsChar(_colName);
        }
    }
}
