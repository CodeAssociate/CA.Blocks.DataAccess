using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class IntListDbColToTypeConverter : BaseDbColToTypeConverter<IList<int>>
    {
        private readonly char _delimiter;
        public IntListDbColToTypeConverter() : this(',')
        {

        }

        public IntListDbColToTypeConverter(char delimiter)
        {
            _delimiter = delimiter;
        }

        private IList<int> ToList(string input)
        {
            var result = new List<int>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                var inputStringArray = input.Split(_delimiter);
                result.AddRange(from s in inputStringArray where !string.IsNullOrWhiteSpace(s) select int.Parse(s.Trim()));
            }
            return result;
        }

        public override IList<int> GetDataValue(DataRow dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<int> GetDataValue(IDataReader dr, string columnName)
        {
            return ToList(dr.AsString(columnName));
        }

        public override IList<int> GetDataValue(DataRow dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }

        public override IList<int> GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToList(dr.AsString(columnIndex));
        }
    }
}