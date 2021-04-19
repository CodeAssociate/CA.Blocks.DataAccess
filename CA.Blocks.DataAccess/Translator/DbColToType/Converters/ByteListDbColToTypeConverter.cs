using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ByteListDbColToTypeConverter : BaseDbColToTypeConverter<IList<byte>>
    {
        private readonly char _delimiter;
        public ByteListDbColToTypeConverter() : this(',')
        {

        }

        public ByteListDbColToTypeConverter(char delimiter)
        {
            _delimiter = delimiter;
        }

        private IList<byte> ToList(string input)
        {
            var result = new List<byte>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select byte.Parse(s.Trim()));
            }
            return result;
        }

        public override IList<byte> GetDataValue(DataRow dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<byte> GetDataValue(IDataReader dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }
    }
}