using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ByteListDbColToTypeConverter : DelimitedListDbColToTypeConverter<byte>
    {
        public ByteListDbColToTypeConverter() : this(',')
        {
        }

        public ByteListDbColToTypeConverter(char delimiter) : base(delimiter)
        {
        }

        protected override IList<byte> ToList(string input)
        {
            var result = new List<byte>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select byte.Parse(s.Trim()));
            }
            return result;
        }
    }
}