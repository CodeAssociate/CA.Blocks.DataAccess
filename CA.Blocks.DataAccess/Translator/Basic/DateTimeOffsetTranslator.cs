using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.Basic
{
    public class DateTimeOffsetTranslator : SimpleDbRow2ObjectTranslator<DateTimeOffset>
    {
        private readonly string _colName;
        public DateTimeOffsetTranslator(string colName)
        {
            _colName = colName;
        }

        protected override DateTimeOffset CustomTranslate(DataRow dr)
        {
            return dr.AsDateTimeOffset(_colName);
        }
    }
}