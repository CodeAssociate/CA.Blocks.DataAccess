using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Providers
{
    public class DefaultDbColToTypeProvider : IDbColToTypeProvider
    {
        private static readonly object _syncLock = new object();
        private readonly ConcurrentDictionary<string, object> _typeConverters;

        public static IDbColToTypeProvider DefaultInstance = new DefaultDbColToTypeProvider();
        

        private string GetKey(Type targetType, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{targetType}" : $"{targetType}-{byName}";
        }

        public DefaultDbColToTypeProvider()
        {
            _typeConverters = new ConcurrentDictionary<string, object>();
            // string
            Add(new StringDbColToTypeConverter());

            // bool
            Add(new BoolDbColToTypeConverter());
            Add(new NullBoolDbColToTypeConverter());
            // byte
            Add(new ByteDbColToTypeConverter());
            Add(new NullByteDbColToTypeConverter());
            // sbyte
            Add(new SByteDbColToTypeConverter());
            Add(new NullSByteDbColToTypeConverter());
            // short
            Add(new ShortDbColToTypeConverter());
            Add(new NullShortDbColToTypeConverter());
            // int
            Add(new IntDbColToTypeConverter());
            Add(new NullIntDbColToTypeConverter());
            Add(new IntListDbColToTypeConverter(','));
            // long
            Add(new LongDbColToTypeConverter());
            Add(new NullLongDbColToTypeConverter());
            // Guid
            Add(new GuidDbColToTypeConverter());
            Add(new NullGuidDbColToTypeConverter());
            // Char
            Add(new CharDbColToTypeConverter());
            Add(new NullCharDbColToTypeConverter());
            // Ushort
            Add(new UShortDbColToTypeConverter());
            Add(new NullUShortDbColToTypeConverter());
            // Uint
            Add(new UIntDbColToTypeConverter());
            Add(new NullUIntDbColToTypeConverter());
            // ULong
            Add(new ULongDbColToTypeConverter());
            Add(new NullULongDbColToTypeConverter());
            // single 
            Add(new SingleDbColToTypeConverter());
            Add(new NullSingleDbColToTypeConverter());
            // Double 
            Add(new DoubleDbColToTypeConverter());
            Add(new NullDoubleDbColToTypeConverter());
            // Decimal 
            Add(new DecimalDbColToTypeConverter());
            Add(new NullDecimalDbColToTypeConverter());
            // Binary
            Add(new BinaryDbColToTypeConverter());

            //DateTime
            Add(new DateTimeDbColToTypeConverter());
            Add(new NullDateTimeDbColToTypeConverter());

            //DateTimeOffSet
            Add(new DateTimeOffSetDbColToTypeConverter());
            Add(new NullDateTimeOffSetDbColToTypeConverter());

            // 
            Add(new VersionDbColToTypeConverter());

#if NET6_0_OR_GREATER
            Add(new DateOnlyDbColToTypeConverter());
            Add(new NullDateOnlyDbColToTypeConverter());
            Add(new TimeOnlyDbColToTypeConverter());
            Add(new NullTimeOnlyDbColToTypeConverter());
#endif
        }

        public void TryAdd<T>(IDbColToTypeConverter<T> typeConverter, string byName = "", bool errorOnExists = false)
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);

            lock (_syncLock)
            {
                if (!_typeConverters.TryAdd(key, typeConverter) && errorOnExists)
                {
                    throw new ApplicationException($"There is already a ITypeConverter Type registered for {key} they must be unique");
                }
            }
        }

        private void TryAddGeneratedType(IDbColToTypeConverter typeConverter, Type targetType, string byName = "", bool errorOnExists = false)
        {
            var key = GetKey(targetType, byName);

            lock (_syncLock)
            {
                if (!_typeConverters.TryAdd(key, typeConverter) && errorOnExists)
                {
                    throw new ApplicationException($"There is already a ITypeConverter Type registered for {key} they must be unique");
                }
            }
        }


        public void Add<T>(IDbColToTypeConverter<T> typeConverter, string byName = "")
        {
            TryAdd(typeConverter, byName, true);
        }


        public IDbColToTypeConverter Resolve<T>(string byName = "")
        {
            var targetType = typeof(T);
            return Resolve(targetType, byName);
        }


        private IDbColToTypeConverter CreateEnumTypeConverterFor(Type targetType)
        {
            Type concreteConverter;
            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                Type genericConverter = typeof(NullEnumDbColToTypeConverter<>);
                concreteConverter = genericConverter.MakeGenericType(Nullable.GetUnderlyingType(targetType));
            }
            else
            {
                Type genericConverter = typeof(EnumDbColToTypeConverter<>);
                concreteConverter = genericConverter.MakeGenericType(targetType);
            }
            var result = (IDbColToTypeConverter)Activator.CreateInstance(concreteConverter, true);

            return result;

        }

        public IDbColToTypeConverter Resolve(Type targetType,  string byName = "")
        {

            var key = GetKey(targetType, byName);

            if (!_typeConverters.TryGetValue(key, out var typeConverter))
            {

                // if it is a enum we can create converter and register
                if (targetType.IsEnum || (targetType.IsGenericType 
                                          && targetType.GenericTypeArguments != null 
                                          && targetType.GenericTypeArguments.Length == 1 
                                          && targetType.GenericTypeArguments[0].IsEnum))
                {
                    var newTypeConverter = CreateEnumTypeConverterFor(targetType);
                    if (newTypeConverter != null)
                    {
                        TryAddGeneratedType(newTypeConverter, targetType);
                        typeConverter = newTypeConverter;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"No DbCol To Type Converter registered for {key}");
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"No DbCol To Type Converter registered for {key}");
                }
            }

            return typeConverter as IDbColToTypeConverter;
        }
    }
}