using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class ShortListDbColToTypeConverter : BaseDbColToTypeConverter<IList<short>>
    {
        private readonly char _delimiter;
        public ShortListDbColToTypeConverter() : this(',')
        {

        }

        public ShortListDbColToTypeConverter(char delimiter)
        {
            _delimiter = delimiter;
        }

        private IList<short> ToList(string input)
        {
            var result = new List<short>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select short.Parse(s.Trim()));
            }
            return result;
        }

        public override IList<short> GetDataValue(DataRow dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<short> GetDataValue(IDataReader dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<short> GetDataValue(DataRow dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }

        public override IList<short> GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }
    }
}