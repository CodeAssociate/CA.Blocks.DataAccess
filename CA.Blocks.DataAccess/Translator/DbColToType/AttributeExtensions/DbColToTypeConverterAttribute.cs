using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using System;

namespace CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColToTypeConverterAttribute : Attribute
    {

        private readonly Type _converterType;

        public Type ConverterType => this._converterType;

        public object[] ConverterParameters { get; private set; }


        public DbColToTypeConverterAttribute(Type converterType)
        {
            this.ConverterParameters = Array.Empty<object>();
            this._converterType = converterType ?? throw new ArgumentNullException(nameof(converterType));
            var fullName = typeof(IDbColToTypeConverter).FullName;
            if (fullName != null)
            {
                var hasInterface = converterType.GetInterface(fullName);
                if (hasInterface == null)
                {
                    throw new ArgumentException("The Converter must be of type IDbColToTypeConverter", nameof(converterType));
                }
            }
        }

        public DbColToTypeConverterAttribute(Type converterType, params object[] converterParameters)
            : this(converterType)
        {
            this.ConverterParameters = converterParameters;
        }
    }
}
