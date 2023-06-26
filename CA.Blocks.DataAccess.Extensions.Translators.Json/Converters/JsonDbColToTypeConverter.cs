using System.Data;
using System.Text.Json;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccess.Extensions.Translators.Json.Converters
{
    public class JsonDbColToTypeConverter<T> : BaseDbColToTypeConverter<T> where T : new()
    {
        private readonly JsonSerializerOptions _options;
        public JsonDbColToTypeConverter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public JsonDbColToTypeConverter()
        {
            _options = null;
        }

        private T ToObject(string json)
        {
            return string.IsNullOrEmpty(json) ? new T() : JsonSerializer.Deserialize<T>(json, _options);
        }

        public override T GetDataValue(DataRow dr, string columnName)
        {
            return ToObject(dr.AsString(columnName));
        }

        public override T GetDataValue(IDataReader dr, string columnName)
        {
            return ToObject(dr.AsString(columnName));
        }

        public override T GetDataValue(DataRow dr, int columnIndex)
        {
            return ToObject(dr.AsString(columnIndex));
        }

        public override T GetDataValue(IDataReader dr, int columnIndex)
        {
            return ToObject(dr.AsString(columnIndex));
        }
    }

#if NET6_0_OR_GREATER
#nullable enable
        // We have to be using C# 7.3 + to use nullable reference types 
        public class NullJsonDbColToTypeConverter<T> : BaseDbColToTypeConverter<T?>
        {
            private readonly JsonSerializerOptions? _options;
            public NullJsonDbColToTypeConverter(JsonSerializerOptions options)
            {
                _options = options;
            }

            public NullJsonDbColToTypeConverter()
            {
                _options = null;
            }

            private T? ToObject(string json)
            {
                return string.IsNullOrEmpty(json) ? default(T?) : JsonSerializer.Deserialize<T>(json, _options);
            }

            public override T? GetDataValue(DataRow dr, string columnName)
            {
                return ToObject(dr.AsString(columnName));
            }

            public override T? GetDataValue(IDataReader dr, string columnName)
            {
                return ToObject(dr.AsString(columnName));
            }

            public override T? GetDataValue(DataRow dr, int columnIndex)
            {
                return ToObject(dr.AsString(columnIndex));
            }

            public override T? GetDataValue(IDataReader dr, int columnIndex)
            {
                return ToObject(dr.AsString(columnIndex));
            }
        }
#nullable restore
#endif
}