using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Mappings;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Providers
{
    public class DefaultDbRowTranslatorProvider : IDbRowTranslatorProvider
    {
        private readonly IDbColToTypeProvider _colTypeConverters = DefaultDbColToTypeProvider.DefaultInstance;

        private static object _syncLock = new object();
        private readonly ConcurrentDictionary<string, object> _typeConverters = new ConcurrentDictionary<string, object>();

        public static IDbRowTranslatorProvider DefaultInstance = new DefaultDbRowTranslatorProvider();

        private string GetKey(Type targetType, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{targetType}" : $"{targetType}-{byName}";
        }

        public DbRowToObjectMappings GenerateDefaultMappingsFor<T>() where T : new()
        {
            DbRowToObjectMappings mappings = new DbRowToObjectMappings();
            var myObjectFields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in myObjectFields)
            {
                if (pi.CanWrite)
                {
                    IDbColToTypeConverter dbToTypeConverter = null;
                    if (Attribute.IsDefined(pi, typeof(DbColToTypeConverterAttribute)))
                    {
                        var customConverter = (DbColToTypeConverterAttribute)(pi.GetCustomAttributes(typeof(DbColToTypeConverterAttribute), false)).FirstOrDefault();
                        if (customConverter != default)
                        {
                            dbToTypeConverter = (IDbColToTypeConverter)Activator.CreateInstance(customConverter.ConverterType, customConverter.ConverterParameters);
                        }
                    }
                    else
                    {
                        dbToTypeConverter = _colTypeConverters.Resolve(pi.PropertyType);
                    }

                    if (dbToTypeConverter != null)
                    {
                        if (Attribute.IsDefined(pi, typeof(DbColToSourceNameAttribute)))
                        {
                            var sourceFrom = (DbColToSourceNameAttribute)(pi.GetCustomAttributes(typeof(DbColToSourceNameAttribute), false)).FirstOrDefault();
                            if (sourceFrom != default)
                            {
                                mappings.AddMapping(new DbColToTypeMapping
                                {
                                    DestinationName = pi.Name, 
                                    SourceNameName = sourceFrom.SourceName, 
                                    Converter = dbToTypeConverter
                                });
                            }
                            else
                            {
                                mappings.AddOneToOneMapping(pi.Name, dbToTypeConverter);
                            }
                        }
                        else
                        {
                            mappings.AddOneToOneMapping(pi.Name, dbToTypeConverter);
                        }
                    }
                }
            }
            return mappings;
        }

        private void TryAdd<T>(string key, IDbRowTranslator<T> translator, bool errorOnExists = true)
        {
            lock (_syncLock)
            {
                if (!_typeConverters.TryAdd(key, translator) && errorOnExists)
                {
                    throw new ApplicationException($"There is already a IDbRowTranslator Type registered for {key} they must be unique");
                }
            }
        }

        private void TryAdd<T>(IDbRowTranslator<T> translator, string byName = "", bool errorOnExists = true)
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);
            TryAdd(key, translator, errorOnExists);
        }

        public void Add<T>(IDbRowTranslator<T> translator, string byName = "")
        {
            TryAdd(translator, byName);
        }

        public IDbRowTranslator<T> Resolve<T>(string byName = "") where T : new()
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);

            if (!_typeConverters.TryGetValue(key, out var typeConverter))
            {
                if (targetType.IsClass)
                {
                    typeConverter = new Db2ObjectTranslator<T>(GenerateDefaultMappingsFor<T>());
                    TryAdd<T>(key, (IDbRowTranslator<T>)typeConverter, false);
                }
                else
                {
                    var dbToTypeConverter = _colTypeConverters.Resolve(targetType);
                    if (dbToTypeConverter != null)
                    {
                        typeConverter = new Db2SingleColumnTranslator<T>(dbToTypeConverter);
                        TryAdd<T>(key, (IDbRowTranslator<T>)typeConverter, false);
                    }
                    else
                    {
                        throw new KeyNotFoundException($"No DbRow To Object Provider registered for {key}");
                    }
                }
            }
            return typeConverter as IDbRowTranslator<T>;
        }

        public bool HasTranslatorFor<T>(string byName = "")
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);

            return _typeConverters.ContainsKey(key);
        }
    }
}