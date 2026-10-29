using System;

namespace CA.Blocks.DataAccess.Generators
{
    public enum PropertyKind
    {
        String,
        Int32,
        NullableInt32,
        Int64,
        NullableInt64,
        Int16,
        NullableInt16,
        Byte,
        NullableByte,
        SByte,
        NullableSByte,
        UInt32,
        NullableUInt32,
        UInt64,
        NullableUInt64,
        UInt16,
        NullableUInt16,
        Boolean,
        NullableBoolean,
        DateTime,
        NullableDateTime,
        DateTimeOffset,
        NullableDateTimeOffset,
        Guid,
        NullableGuid,
        Decimal,
        NullableDecimal,
        Double,
        NullableDouble,
        Single,
        NullableSingle,
        ByteArray,
        DateOnly,
        NullableDateOnly,
        TimeOnly,
        NullableTimeOnly,
        Enum,
        NullableEnum,
        CustomConverter,
        Object
    }

    public sealed class PropertyToGenerate : IEquatable<PropertyToGenerate>
    {
        public string PropertyName { get; }
        public string PropertyTypeName { get; }
        public string SourceColumnName { get; }
        public PropertyKind Kind { get; }
        public string? CustomConverterTypeName { get; }
        public EquatableArray<string> CustomConverterArguments { get; }

        public PropertyToGenerate(
            string propertyName,
            string propertyTypeName,
            string sourceColumnName,
            PropertyKind kind,
            string? customConverterTypeName = null,
            EquatableArray<string>? customConverterArguments = null)
        {
            PropertyName = propertyName;
            PropertyTypeName = propertyTypeName;
            SourceColumnName = sourceColumnName;
            Kind = kind;
            CustomConverterTypeName = customConverterTypeName;
            CustomConverterArguments = customConverterArguments ?? EquatableArray<string>.Empty;
        }

        public bool Equals(PropertyToGenerate? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return PropertyName == other.PropertyName &&
                   PropertyTypeName == other.PropertyTypeName &&
                   SourceColumnName == other.SourceColumnName &&
                   Kind == other.Kind &&
                   CustomConverterTypeName == other.CustomConverterTypeName &&
                   CustomConverterArguments.Equals(other.CustomConverterArguments);
        }

        public override bool Equals(object? obj) => Equals(obj as PropertyToGenerate);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = PropertyName.GetHashCode();
                hash = (hash * 397) ^ PropertyTypeName.GetHashCode();
                hash = (hash * 397) ^ SourceColumnName.GetHashCode();
                hash = (hash * 397) ^ (int)Kind;
                hash = (hash * 397) ^ (CustomConverterTypeName?.GetHashCode() ?? 0);
                hash = (hash * 397) ^ CustomConverterArguments.GetHashCode();
                return hash;
            }
        }
    }

    public sealed class TypeToGenerate : IEquatable<TypeToGenerate>
    {
        public string TargetNamespace { get; }
        public string TargetTypeName { get; }
        public string FullTypeName { get; }
        public string TranslatorClassName { get; }
        public string Accessibility { get; }
        public bool IsValueType { get; }
        public bool MatchNamesWithUnderscores { get; }
        public bool MatchCaseInsensitive { get; }
        public string? ByName { get; }
        public bool AutoRegister { get; }
        public bool ThrowIfColumnNotFound { get; }
        public EquatableArray<PropertyToGenerate> Properties { get; }

        public TypeToGenerate(
            string targetNamespace,
            string targetTypeName,
            string fullTypeName,
            string translatorClassName,
            string accessibility,
            bool isValueType,
            bool matchNamesWithUnderscores,
            bool matchCaseInsensitive,
            string? byName,
            bool autoRegister,
            bool throwIfColumnNotFound,
            EquatableArray<PropertyToGenerate> properties)
        {
            TargetNamespace = targetNamespace;
            TargetTypeName = targetTypeName;
            FullTypeName = fullTypeName;
            TranslatorClassName = translatorClassName;
            Accessibility = accessibility;
            IsValueType = isValueType;
            MatchNamesWithUnderscores = matchNamesWithUnderscores;
            MatchCaseInsensitive = matchCaseInsensitive;
            ByName = byName;
            AutoRegister = autoRegister;
            ThrowIfColumnNotFound = throwIfColumnNotFound;
            Properties = properties;
        }

        public bool Equals(TypeToGenerate? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return TargetNamespace == other.TargetNamespace &&
                   TargetTypeName == other.TargetTypeName &&
                   FullTypeName == other.FullTypeName &&
                   TranslatorClassName == other.TranslatorClassName &&
                   Accessibility == other.Accessibility &&
                   IsValueType == other.IsValueType &&
                   MatchNamesWithUnderscores == other.MatchNamesWithUnderscores &&
                   MatchCaseInsensitive == other.MatchCaseInsensitive &&
                   ByName == other.ByName &&
                   AutoRegister == other.AutoRegister &&
                   ThrowIfColumnNotFound == other.ThrowIfColumnNotFound &&
                   Properties.Equals(other.Properties);
        }

        public override bool Equals(object? obj) => Equals(obj as TypeToGenerate);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = TargetNamespace.GetHashCode();
                hash = (hash * 397) ^ TargetTypeName.GetHashCode();
                hash = (hash * 397) ^ FullTypeName.GetHashCode();
                hash = (hash * 397) ^ TranslatorClassName.GetHashCode();
                hash = (hash * 397) ^ Accessibility.GetHashCode();
                hash = (hash * 397) ^ IsValueType.GetHashCode();
                hash = (hash * 397) ^ MatchNamesWithUnderscores.GetHashCode();
                hash = (hash * 397) ^ MatchCaseInsensitive.GetHashCode();
                hash = (hash * 397) ^ (ByName?.GetHashCode() ?? 0);
                hash = (hash * 397) ^ AutoRegister.GetHashCode();
                hash = (hash * 397) ^ ThrowIfColumnNotFound.GetHashCode();
                hash = (hash * 397) ^ Properties.GetHashCode();
                return hash;
            }
        }
    }
}
