using System;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes
{
    /// <summary>
    /// Instructs the CA.Blocks.DataAccess.Generators source generator to generate a compile-time, AOT-friendly IDbRowTranslator implementation for this type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class GenerateDbRowTranslatorAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets whether column names with underscores should match property names without underscores (e.g., 'first_name' matches 'FirstName'). Default is true.
        /// </summary>
        public bool MatchNamesWithUnderscores { get; set; } = true;

        /// <summary>
        /// Gets or sets whether column matching should be case-insensitive. Default is true.
        /// </summary>
        public bool MatchCaseInsensitive { get; set; } = true;

        /// <summary>
        /// Optional name key under which to register the translator in DefaultDbRowTranslatorProvider. Default is empty / null (type-level registration).
        /// </summary>
        public string? ByName { get; set; }

        /// <summary>
        /// Gets or sets whether the generated translator should automatically register itself into DefaultDbRowTranslatorProvider.DefaultInstance upon module load. Default is true.
        /// </summary>
        public bool AutoRegister { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to throw a ConverterColumnNotFoundException if a mapped property's column is not found in the result set. Default is true.
        /// </summary>
        public bool ThrowIfColumnNotFound { get; set; } = true;

        public GenerateDbRowTranslatorAttribute()
        {
        }

        public GenerateDbRowTranslatorAttribute(string byName)
        {
            ByName = byName;
        }
    }
}
