using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class LongListDbColToTypeConverter : DelimitedListDbColToTypeConverter<long>
    {
        public LongListDbColToTypeConverter() : this(',')
        {
        }

        public LongListDbColToTypeConverter(char delimiter) : base(delimiter)
        {
        }

        protected override IList<long> ToList(string input)
        {
            var result = new List<long>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select long.Parse(s.Trim()));
            }
            return result;
        }
    }
}