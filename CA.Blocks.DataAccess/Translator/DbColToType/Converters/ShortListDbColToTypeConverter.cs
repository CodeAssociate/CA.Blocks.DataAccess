using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ShortListDbColToTypeConverter : DelimitedListDbColToTypeConverter<short>
    {
        public ShortListDbColToTypeConverter() : this(',')
        {
        }

        public ShortListDbColToTypeConverter(char delimiter) : base(delimiter)
        {
        }

        protected override IList<short> ToList(string? input)
        {
            var result = new List<short>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input!.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select short.Parse(s.Trim()));
            }
            return result;
        }
    }
}