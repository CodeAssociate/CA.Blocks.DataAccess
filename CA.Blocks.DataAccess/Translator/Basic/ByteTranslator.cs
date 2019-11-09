using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class ByteTranslator : SimpleDbRow2ObjectTranslator<Byte>
    {
        private readonly string _colName;
        public ByteTranslator(string colName)
        {
            _colName = colName;
        }

        protected override Byte CustomTranslate(DataRow dr)
        {
            return dr.AsByte(_colName);
        }
    }
}
