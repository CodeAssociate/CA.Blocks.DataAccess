using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Providers
{
    public class DefaultDbRowTranslatorProvider : IDbRowTranslatorProvider
    {
        private readonly IDbColToTypeProvider _colTypeConverters;

        private static object _syncLock = new object();
        private readonly IDictionary<string, object> _typeConverters;

        public static IDbRowTranslatorProvider DefaultInstance = new DefaultDbRowTranslatorProvider();


        public DefaultDbRowTranslatorProvider()
        {
            _colTypeConverters = DefaultDbColToTypeProvider.DefaultInstance;
            _typeConverters = new ConcurrentDictionary<string, object>();
        }
        

        private string GetKey(Type targetType, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{targetType}" : $"{targetType}-{byName}";
        }


        private IDbRowTranslator<T> GenerateDefaultMappingsFor<T>() where T : new()
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

        private void Add<T>(string key, IDbRowTranslator<T> translator, string byName = "")
        {
            lock (_syncLock)
            {
                if (_typeConverters.ContainsKey(key))
                {
                    throw new ApplicationException($"There is already a IDbRowTranslator Type registered for {key} they must be unique");
                }
                _typeConverters.Add(key, translator);
            }
        }

        public void Add<T>(IDbRowTranslator<T> translator, string byName = "")
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);
            Add(key, translator, byName);
        }


        public IDbRowTranslator<T> Resolve<T>(string byName = "") where T : new()
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);
            object typeConverter = null;

            if (!_typeConverters.TryGetValue(key, out typeConverter))
            {
                typeConverter = GenerateDefaultMappingsFor<T>();
                Add<T>(key, (IDbRowTranslator<T>)typeConverter);

               // let try register the default version with 100% 1-1 mapping. might fail but no worse that not having one registered
               //throw new KeyNotFoundException($"No DbRow To Object Provider registered for {key}");
            }

            return typeConverter as IDbRowTranslator<T>;
        }
    }
}