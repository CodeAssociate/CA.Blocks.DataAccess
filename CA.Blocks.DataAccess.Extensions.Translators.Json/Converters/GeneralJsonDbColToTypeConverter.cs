using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using System;
using System.Data;
using System.Text.Json;

namespace CA.Blocks.DataAccess.Extensions.Translators.Json.Converters
{
    public class GeneralJsonDbColToTypeConverter<T> : BaseDbColToTypeConverter<T>
    {
        // A factory function to create a default instance of T
        // this it to deal with the case when we have no json to Deserialize
        // removing the new() constaint there are three key usecases:
        // 1) this Deserialize to IList or like
        // 2) Deserialize to an Interface with a default implementation 
        // 3) Deserialize to t nuallable object
        private readonly Func<T> _defaultFactory;
        private readonly JsonSerializerOptions _options;
        public GeneralJsonDbColToTypeConverter(JsonSerializerOptions options, Func<T> defaultFactory)
        {
            _defaultFactory = defaultFactory;
            _options = options;
        }

        private T ToObject(string json)
        {
            return string.IsNullOrEmpty(json) ? _defaultFactory() : JsonSerializer.Deserialize<T>(json, _options);
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
}