using System.Reflection;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


namespace CA.Blocks.DataAccess.Generators.Tests
{
    public class GeneratorUnitTests
    {
        [Fact]
        public void Generator_GeneratesTranslator_ForAnnotatedClass()
        {
            string source = @"
using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;

namespace TestNamespace
{
    [GenerateDbRowTranslator]
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
";

            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken) },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IDbRowTranslator<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(GenerateDbRowTranslatorAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Data.Common").Location),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new GenerateDbRowTranslatorGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics, TestContext.Current.CancellationToken);

            Assert.Empty(diagnostics);

            var runResult = driver.GetRunResult();
            Assert.Single(runResult.GeneratedTrees);

            string generatedCode = runResult.GeneratedTrees[0].ToString();
            Assert.Contains("public sealed partial class CustomerDtoDbRowTranslator : IDbRowTranslator<global::TestNamespace.CustomerDto>", generatedCode);
            Assert.Contains("val_Id = dr.AsInt(ords.IdOrdinal);", generatedCode);
            Assert.Contains("val_Name = dr.AsString(ords.NameOrdinal) ?? string.Empty;", generatedCode);
            Assert.Contains("val_CreatedAt = dr.AsDateTime(ords.CreatedAtOrdinal);", generatedCode);
            Assert.Contains("Id = val_Id,", generatedCode);
            Assert.Contains("Name = val_Name,", generatedCode);
            Assert.Contains("CreatedAt = val_CreatedAt,", generatedCode);
        }

        [Fact]
        public void Generator_Supports_InitAndRequiredProperties()
        {
            string source = @"
using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;

namespace TestNamespace
{
    [GenerateDbRowTranslator]
    public class CustomerWithInitAndRequired
    {
        public int Id { get; init; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; init; }
    }
}
";

            var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp11);
            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { CSharpSyntaxTree.ParseText(source, parseOptions, cancellationToken: TestContext.Current.CancellationToken) },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IDbRowTranslator<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(GenerateDbRowTranslatorAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Data.Common").Location),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new GenerateDbRowTranslatorGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { generator.AsSourceGenerator() }, parseOptions: parseOptions);

            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics, TestContext.Current.CancellationToken);

            Assert.Empty(diagnostics);

            var runResult = driver.GetRunResult();
            Assert.Single(runResult.GeneratedTrees);

            string generatedCode = runResult.GeneratedTrees[0].ToString();
            Assert.Contains("Id = val_Id,", generatedCode);
            Assert.Contains("Name = val_Name,", generatedCode);
            Assert.Contains("CreatedAt = val_CreatedAt,", generatedCode);
        }

        [Fact]
        public void Generator_HandlesCustomSourceNameAndConverter()
        {
            string source = @"
using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;
using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace TestNamespace
{
    [GenerateDbRowTranslator(ThrowIfColumnNotFound = false)]
    public class ProductDto
    {
        [DbColToSourceName(""prod_id"")]
        public int Id { get; set; }

        [DbColToSourceName(""list_nums"")]
        [DbColToTypeConverter(typeof(IntListDbColToTypeConverter), ',')]
        public string? ListOfNumbers { get; set; }
    }
}
";

            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken) },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IDbRowTranslator<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(GenerateDbRowTranslatorAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Data.Common").Location),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new GenerateDbRowTranslatorGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics, TestContext.Current.CancellationToken);

            Assert.Empty(diagnostics);

            var runResult = driver.GetRunResult();
            Assert.Single(runResult.GeneratedTrees);

            string generatedCode = runResult.GeneratedTrees[0].ToString();
            Assert.Contains("case \"prodid\":", generatedCode);
            Assert.Contains("case \"listnums\":", generatedCode);
            Assert.Contains("_converter_ListOfNumbers", generatedCode);
        }

        [Fact]
        public void Generator_HandlesCustomUnmappedType_ResolvingViaDefaultDbColToTypeProvider()
        {
            string source = @"
using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Attributes;

namespace TestNamespace
{
    public struct CustomId { public string Value { get; set; } }

    [GenerateDbRowTranslator]
    public class OrderDto
    {
        public int Id { get; set; }
        public CustomId OrderId { get; set; }
    }
}
";

            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken) },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IDbRowTranslator<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(GenerateDbRowTranslatorAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Data.Common").Location),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new GenerateDbRowTranslatorGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics, TestContext.Current.CancellationToken);

            Assert.Empty(diagnostics);

            var runResult = driver.GetRunResult();
            Assert.Single(runResult.GeneratedTrees);

            string generatedCode = runResult.GeneratedTrees[0].ToString();
            Assert.Contains("Lazy<IDbColToTypeConverter> _converter_OrderId", generatedCode);
            Assert.Contains("DefaultDbColToTypeProvider.DefaultInstance.Resolve(typeof(global::TestNamespace.CustomId))", generatedCode);
            Assert.Contains("val_OrderId = (global::TestNamespace.CustomId)_converter_OrderId.Value.GetData(dr, ords.OrderIdOrdinal)!;", generatedCode);
        }
    }
}
