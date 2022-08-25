using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class IntListDbColToTypeConverter : DelimitedListDbColToTypeConverter<int>
    {
        public IntListDbColToTypeConverter() : this(',')
        {
        }

        public IntListDbColToTypeConverter(char delimiter) : base (delimiter)
        {
        }

        protected override IList<int> ToList(string input)
        {
            var result = new List<int>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select int.Parse(s.Trim()));
            }
            return result;
        }
    }
}