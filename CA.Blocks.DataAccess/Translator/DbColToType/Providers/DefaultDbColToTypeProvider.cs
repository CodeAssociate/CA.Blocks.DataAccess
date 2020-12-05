using System;
using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Providers
{
    public class DefaultDbColToTypeProvider : IDbColToTypeProvider
    {
        private readonly IDictionary<string, object> _typeConverters;


        private string GetKey(Type targetType, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{targetType}" : $"{targetType}-{byName}";
        }

        public DefaultDbColToTypeProvider()
        {
            _typeConverters = new Dictionary<string, object>();
            // string
            Add(new StringDbColToTypeConverter());

            // bool
            Add(new BoolDbColToTypeConverter());
            Add(new NullBoolDbColToTypeConverter());
            // byte
            Add(new ByteDbColToTypeConverter());
            Add(new NullByteDbColToTypeConverter());
            // short
            Add(new ShortDbColToTypeConverter());
            Add(new NullShortDbColToTypeConverter());
            // int
            Add(new IntDbColToTypeConverter());
            Add(new NullIntDbColToTypeConverter());
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


            Add(new DateTimeDbColToTypeConverter());
            Add(new NullDateTimeDbColToTypeConverter());

        }

        public void Add<T>(IDbColToTypeConverter<T> typeConverter, string byName = "")
        {
            var targetType = typeof(T);
            var key = GetKey(targetType, byName);
            if (_typeConverters.ContainsKey(key))
            {
                throw new ApplicationException($"There is already a ITypeConverter Type registered for {key} they must be unique");
            }
            _typeConverters[key] = typeConverter;

        }

        public IDbColToTypeConverter Resolve<T>(string byName = "")
        {

            var targetType = typeof(T);
            return Resolve(targetType, byName);
        }

        public IDbColToTypeConverter Resolve(Type targetType,  string byName = "")
        {

            var key = GetKey(targetType, byName);

            if (!_typeConverters.TryGetValue(key, out var typeConverter))
            {

                throw new KeyNotFoundException($"No DbCol To Type Converter registered for {key}");
            }

            return typeConverter as IDbColToTypeConverter;
        }
    }
}