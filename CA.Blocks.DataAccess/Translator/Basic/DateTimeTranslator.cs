using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{

    public class DateTimeTranslator : SimpleDbRow2ObjectTranslator<DateTime>
    {
        private readonly string _colName;
        public DateTimeTranslator(string colName)
        {
            _colName = colName;
        }

        protected override DateTime CustomTranslate(DataRow dr)
        {
            return dr.AsDateTime(_colName);
        }
    }
}
