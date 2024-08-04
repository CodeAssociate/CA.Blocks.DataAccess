using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using System.Diagnostics.CodeAnalysis;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbRowToObject
{
    [TestFixture]
    public class DefaultDbRowTranslatorProviderTests
    {
        //We only need the structure to generate the mapping
        [ExcludeFromCodeCoverage()]
        public class CustomClass1
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            // ReSharper disable once UnusedMember.Global We are testing this case
            public int IgnoreMe  { get;}
        }

        //We only need the structure to generate the mapping
        [ExcludeFromCodeCoverage()]
        public class CustomClass2
        {
            [DbColToTypeConverter(typeof(IntDbColToTypeConverter))]
            public int Id { get; set; }

            [DbColToSourceName("ListSource")]
            [DbColToTypeConverter(typeof(IntListDbColToTypeConverter), ',')]
            public string? ListOfNumbers { get; set; }
            // ReSharper disable once UnassignedGetOnlyAutoProperty Testing this case
            public int IgnoreMe { get; }
        }


        [Test]
        public void GenerateDefaultMappingsForTest()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass1>();
            Assert.That(mappingSet.MappingSet.Count, Is.EqualTo(2));
            Assert.That(mappingSet.MappingSet[0].DestinationName, Is.EqualTo("Id"));
            Assert.That(mappingSet.MappingSet[1].DestinationName, Is.EqualTo("Name"));
        }

        [Test]
        public void GenerateDefaultMappingsForTest2()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass2>();
            Assert.That(mappingSet.MappingSet.Count, Is.EqualTo(2));
            Assert.That(mappingSet.MappingSet[0].DestinationName, Is.EqualTo("Id"));
            Assert.That(mappingSet.MappingSet[1].DestinationName, Is.EqualTo("ListOfNumbers"));
            Assert.That(mappingSet.MappingSet[1].SourceNameName, Is.EqualTo("ListSource"));
            Assert.That(mappingSet.MappingSet[1].Converter.GetType().FullName, Is.EqualTo(typeof(IntListDbColToTypeConverter).FullName));
        }
    }
}
