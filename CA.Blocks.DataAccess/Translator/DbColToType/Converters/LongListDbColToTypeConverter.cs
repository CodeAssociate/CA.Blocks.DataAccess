using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class LongListDbColToTypeConverter : BaseDbColToTypeConverter<IList<long>>
    {
        private readonly char _delimiter;
        public LongListDbColToTypeConverter() : this(',')
        {

        }

        public LongListDbColToTypeConverter(char delimiter)
        {
            _delimiter = delimiter;
        }

        private IList<long> ToList(string input)
        {
            var result = new List<long>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select long.Parse(s.Trim()));
            }
            return result;
        }

        public override IList<long> GetDataValue(DataRow dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<long> GetDataValue(IDataReader dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }
    }
}