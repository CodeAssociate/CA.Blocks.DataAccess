using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CA.Blocks.DataAccess.Generators
{
    [Generator(LanguageNames.CSharp)]
    public sealed class GenerateDbRowTranslatorGenerator : IIncrementalGenerator
    {
        private const string AttributeShortName = "GenerateDbRowTranslator";
        private const string AttributeFullName = "CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes.GenerateDbRowTranslatorAttribute";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register syntax provider for classes, structs, records with attributes
            IncrementalValuesProvider<TypeToGenerate?> typesToGenerate = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => IsTargetSyntax(node),
                    transform: static (ctx, ct) => GetTargetType(ctx, ct))
                .Where(static t => t is not null);

            // Register output generation
            context.RegisterSourceOutput(typesToGenerate, static (spc, type) =>
            {
                if (type is null) return;
                string source = TranslatorEmitter.Emit(type);
                spc.AddSource($"{type.TranslatorClassName}.g.cs", SourceText.From(source, Encoding.UTF8));
            });
        }

        private static bool IsTargetSyntax(SyntaxNode node)
        {
            return node is TypeDeclarationSyntax typeDecl && typeDecl.AttributeLists.Count > 0;
        }

        private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat
            .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

        private static TypeToGenerate? GetTargetType(GeneratorSyntaxContext context, CancellationToken ct)
        {
            if (context.Node is not TypeDeclarationSyntax typeDecl)
                return null;

            if (context.SemanticModel.GetDeclaredSymbol(typeDecl, ct) is not INamedTypeSymbol typeSymbol)
                return null;

            AttributeData? targetAttr = null;
            foreach (var attr in typeSymbol.GetAttributes())
            {
                if (attr.AttributeClass is null) continue;
                string attrName = attr.AttributeClass.Name;
                string attrFull = attr.AttributeClass.ToDisplayString();

                if (attrName == AttributeShortName ||
                    attrName == AttributeShortName + "Attribute" ||
                    attrFull == AttributeFullName)
                {
                    targetAttr = attr;
                    break;
                }
            }

            if (targetAttr is null)
                return null;

            // Extract attribute settings
            bool matchNamesWithUnderscores = true;
            bool matchCaseInsensitive = true;
            string? byName = null;
            bool autoRegister = true;
            bool throwIfColumnNotFound = true;

            // Constructor arguments
            if (targetAttr.ConstructorArguments.Length > 0)
            {
                var firstArg = targetAttr.ConstructorArguments[0];
                if (firstArg.Value is string s)
                {
                    byName = s;
                }
            }

            // Named arguments
            foreach (var namedArg in targetAttr.NamedArguments)
            {
                switch (namedArg.Key)
                {
                    case "MatchNamesWithUnderscores" when namedArg.Value.Value is bool b:
                        matchNamesWithUnderscores = b;
                        break;
                    case "MatchCaseInsensitive" when namedArg.Value.Value is bool b:
                        matchCaseInsensitive = b;
                        break;
                    case "ByName" when namedArg.Value.Value is string s:
                        byName = s;
                        break;
                    case "AutoRegister" when namedArg.Value.Value is bool b:
                        autoRegister = b;
                        break;
                    case "ThrowIfColumnNotFound" when namedArg.Value.Value is bool b:
                        throwIfColumnNotFound = b;
                        break;
                }
            }

            string targetNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : typeSymbol.ContainingNamespace.ToDisplayString();

            string targetTypeName = typeSymbol.Name;
            string fullTypeName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            string translatorClassName = $"{targetTypeName}DbRowTranslator";
            string accessibility = typeSymbol.DeclaredAccessibility == Accessibility.Public ? "public" : "internal";
            bool isValueType = typeSymbol.IsValueType;

            var properties = new List<PropertyToGenerate>();

            // Inspect properties
            var members = typeSymbol.GetMembers().OfType<IPropertySymbol>();
            foreach (var prop in members)
            {
                if (prop.IsStatic || prop.DeclaredAccessibility != Accessibility.Public)
                    continue;

                // Needs a setter (set or init)
                if (prop.SetMethod is null)
                    continue;

                string propName = prop.Name;
                string propTypeName = prop.Type.ToDisplayString(TypeDisplayFormat);
                if (prop.Type.NullableAnnotation == NullableAnnotation.Annotated && !propTypeName.EndsWith("?"))
                {
                    propTypeName += "?";
                }
                string sourceColumnName = propName;
                string? customConverterTypeName = null;
                var customConverterArgs = new List<string>();

                // Check attributes on property
                foreach (var attr in prop.GetAttributes())
                {
                    if (attr.AttributeClass is null) continue;
                    string aName = attr.AttributeClass.Name;

                    if (aName == "DbColToSourceNameAttribute" || aName == "DbColToSourceName")
                    {
                        if (attr.ConstructorArguments.Length > 0 && attr.ConstructorArguments[0].Value is string srcName)
                        {
                            sourceColumnName = srcName;
                        }
                    }
                    else if (aName == "DbColToTypeConverterAttribute" || aName == "DbColToTypeConverter")
                    {
                        if (attr.ConstructorArguments.Length > 0 && attr.ConstructorArguments[0].Value is INamedTypeSymbol convType)
                        {
                            customConverterTypeName = convType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        }

                        if (attr.ConstructorArguments.Length > 1)
                        {
                            var secondArg = attr.ConstructorArguments[1];
                            if (secondArg.Kind == TypedConstantKind.Array)
                            {
                                foreach (var elem in secondArg.Values)
                                {
                                    customConverterArgs.Add(FormatTypedConstant(elem));
                                }
                            }
                            else
                            {
                                customConverterArgs.Add(FormatTypedConstant(secondArg));
                            }
                        }
                    }
                }

                PropertyKind kind = DeterminePropertyKind(prop.Type, customConverterTypeName);

                properties.Add(new PropertyToGenerate(
                    propName,
                    propTypeName,
                    sourceColumnName,
                    kind,
                    customConverterTypeName,
                    new EquatableArray<string>(customConverterArgs)
                ));
            }

            return new TypeToGenerate(
                targetNamespace,
                targetTypeName,
                fullTypeName,
                translatorClassName,
                accessibility,
                isValueType,
                matchNamesWithUnderscores,
                matchCaseInsensitive,
                byName,
                autoRegister,
                throwIfColumnNotFound,
                new EquatableArray<PropertyToGenerate>(properties)
            );
        }

        private static string FormatTypedConstant(TypedConstant tc)
        {
            if (tc.IsNull) return "null";
            if (tc.Type is null) return tc.Value?.ToString() ?? "null";

            if (tc.Type.SpecialType == SpecialType.System_String)
            {
                return $"\"{tc.Value?.ToString()?.Replace("\"", "\\\"")}\"";
            }
            if (tc.Type.SpecialType == SpecialType.System_Char)
            {
                char c = (char)tc.Value!;
                return c == '\'' ? "'\\''" : $"'{c}'";
            }
            if (tc.Type.SpecialType == SpecialType.System_Boolean)
            {
                return (bool)tc.Value! ? "true" : "false";
            }
            return tc.Value?.ToString() ?? "null";
        }

        private static PropertyKind DeterminePropertyKind(ITypeSymbol type, string? customConverterTypeName)
        {
            if (!string.IsNullOrWhiteSpace(customConverterTypeName))
                return PropertyKind.CustomConverter;

            // Check SpecialTypes
            switch (type.SpecialType)
            {
                case SpecialType.System_String:
                    return PropertyKind.String;
                case SpecialType.System_Int32:
                    return PropertyKind.Int32;
                case SpecialType.System_Int64:
                    return PropertyKind.Int64;
                case SpecialType.System_Int16:
                    return PropertyKind.Int16;
                case SpecialType.System_Byte:
                    return PropertyKind.Byte;
                case SpecialType.System_SByte:
                    return PropertyKind.SByte;
                case SpecialType.System_UInt32:
                    return PropertyKind.UInt32;
                case SpecialType.System_UInt64:
                    return PropertyKind.UInt64;
                case SpecialType.System_UInt16:
                    return PropertyKind.UInt16;
                case SpecialType.System_Boolean:
                    return PropertyKind.Boolean;
                case SpecialType.System_DateTime:
                    return PropertyKind.DateTime;
                case SpecialType.System_Decimal:
                    return PropertyKind.Decimal;
                case SpecialType.System_Double:
                    return PropertyKind.Double;
                case SpecialType.System_Single:
                    return PropertyKind.Single;
            }

            // Check byte[]
            if (type is IArrayTypeSymbol arrayType && arrayType.ElementType.SpecialType == SpecialType.System_Byte)
            {
                return PropertyKind.ByteArray;
            }

            // Check Nullable<T>
            if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullableType)
            {
                var underlying = nullableType.TypeArguments[0];
                switch (underlying.SpecialType)
                {
                    case SpecialType.System_Int32:
                        return PropertyKind.NullableInt32;
                    case SpecialType.System_Int64:
                        return PropertyKind.NullableInt64;
                    case SpecialType.System_Int16:
                        return PropertyKind.NullableInt16;
                    case SpecialType.System_Byte:
                        return PropertyKind.NullableByte;
                    case SpecialType.System_SByte:
                        return PropertyKind.NullableSByte;
                    case SpecialType.System_UInt32:
                        return PropertyKind.NullableUInt32;
                    case SpecialType.System_UInt64:
                        return PropertyKind.NullableUInt64;
                    case SpecialType.System_UInt16:
                        return PropertyKind.NullableUInt16;
                    case SpecialType.System_Boolean:
                        return PropertyKind.NullableBoolean;
                    case SpecialType.System_DateTime:
                        return PropertyKind.NullableDateTime;
                    case SpecialType.System_Decimal:
                        return PropertyKind.NullableDecimal;
                    case SpecialType.System_Double:
                        return PropertyKind.NullableDouble;
                    case SpecialType.System_Single:
                        return PropertyKind.NullableSingle;
                }

                string underFull = underlying.ToDisplayString();
                if (underFull == "System.Guid") return PropertyKind.NullableGuid;
                if (underFull == "System.DateTimeOffset") return PropertyKind.NullableDateTimeOffset;
                if (underFull == "System.DateOnly") return PropertyKind.NullableDateOnly;
                if (underFull == "System.TimeOnly") return PropertyKind.NullableTimeOnly;

                if (underlying.TypeKind == TypeKind.Enum)
                    return PropertyKind.NullableEnum;
            }

            // Check non-nullable custom value types
            string full = type.ToDisplayString();
            if (full == "System.Guid") return PropertyKind.Guid;
            if (full == "System.DateTimeOffset") return PropertyKind.DateTimeOffset;
            if (full == "System.DateOnly") return PropertyKind.DateOnly;
            if (full == "System.TimeOnly") return PropertyKind.TimeOnly;

            if (type.TypeKind == TypeKind.Enum)
                return PropertyKind.Enum;

            return PropertyKind.Object;
        }
    }
}
