using System;
using System.Collections.Generic;
using System.Reflection;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Providers
{
    public class DefaultDbRowToObjectProviderProvider : IDbRowToObjectProvider
    {

        public IDbColToTypeProvider _colTypeConverters;

        public DefaultDbRowToObjectProviderProvider(IDbColToTypeProvider colTypeConverters)
        {
            _colTypeConverters = colTypeConverters;
        }


        private readonly IDictionary<string, object> _typeConverters = new Dictionary<string, object>();


        private string GetKey(Type targetType, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{targetType}" : $"{targetType}-{byName}";
        }


        private IDb2ObjectTranslator<T> GenerateDefaultMappingsFor<T>() where T : new()
        {

            DbRowToObjectMappings mappings = new DbRowToObjectMappings();
            var myObjectFields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in myObjectFields)
            {
                if (pi.CanWrite)
                {
                    var dbToTypeConverter = _colTypeConverters.Resolve(pi.PropertyType);
                    if (dbToTypeConverter != null)
                    {
                        mappings.AddOneToOneMapping(pi.Name, dbToTypeConverter);
                    }
                }
            }
            return new Db2ObjectTranslator<T>(mappings);
        }


        public IDb2ObjectTranslator<T> Resolve<T>(string byName = "") where T : new()
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);
            object typeConverter = null;

            if (!_typeConverters.TryGetValue(key, out typeConverter))
            {
                typeConverter = GenerateDefaultMappingsFor<T>();
                /// bugger.
                _typeConverters.Add(key, typeConverter);
               // let try register the default version with 100% 1-1 mapping. might fail but no worse that not having one registered
               //throw new KeyNotFoundException($"No DbRow To Object Provider registered for {key}");
            }

            return typeConverter as IDb2ObjectTranslator<T>;
        }
    }


}