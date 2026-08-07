using System.Collections.Generic;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public abstract class DelimitedListDbColToTypeConverter<T> : BaseDbColToTypeConverter<IList<T>>
    {
        protected readonly char _delimiter;

        protected DelimitedListDbColToTypeConverter(char delimiter)
        {
            _delimiter = delimiter;
        }

        protected abstract IList<T> ToList(string? input);
       
        public override IList<T> GetDataValue(DataRow dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<T> GetDataValue(IDataReader dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<T> GetDataValue(DataRow dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }

        public override IList<T> GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }
    }
}