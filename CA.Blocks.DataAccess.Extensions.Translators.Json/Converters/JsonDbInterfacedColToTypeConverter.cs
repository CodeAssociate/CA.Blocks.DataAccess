using System.Data;
using System.Text.Json;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccess.Extensions.Translators.Json.Converters
{
    public class JsonDbInterfacedColToTypeConverter<I, T> : BaseDbColToTypeConverter<I>
           where T : I, new()
    {
        private readonly JsonSerializerOptions _options;
        public JsonDbInterfacedColToTypeConverter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public JsonDbInterfacedColToTypeConverter()
        {
            _options = null;
        }

        private I ToObject(string json)
        {
            return string.IsNullOrEmpty(json) ?
                new T() :
                JsonSerializer.Deserialize<T>(json, _options);
        }

        public override I GetDataValue(DataRow dr, string columnName)
        {
            return ToObject(dr.AsString(columnName));
        }

        public override I GetDataValue(IDataReader dr, string columnName)
        {
            return ToObject(dr.AsString(columnName));
        }

        public override I GetDataValue(DataRow dr, int columnIndex)
        {
            return ToObject(dr.AsString(columnIndex));
        }

        public override I GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToObject(dr.AsString(columnIndex));
        }
    }
}