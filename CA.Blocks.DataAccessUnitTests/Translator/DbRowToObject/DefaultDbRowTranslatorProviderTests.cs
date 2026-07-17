using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using System.Diagnostics.CodeAnalysis;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbRowToObject
{
        public class DefaultDbRowTranslatorProviderTests
    {
        //We only need the structure to generate the mapping
        [ExcludeFromCodeCoverage()]
        public class CustomClass1
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            // ReSharper disable once UnusedMember.Global We are testing this case
            public int IgnoreMe
            {
                get { return 1; }
            } 
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


        [Fact]
        public void GenerateDefaultMappingsForTest()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass1>();
            Assert.Equal(2, mappingSet.MappingSet.Count);
            Assert.Equal("Id", mappingSet.MappingSet[0].DestinationName);
            Assert.Equal("Name", mappingSet.MappingSet[1].DestinationName);
        }

        [Fact]
        public void GenerateDefaultMappingsForTest2()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass2>();
            Assert.Equal(2, mappingSet.MappingSet.Count);
            Assert.Equal("Id", mappingSet.MappingSet[0].DestinationName);
            Assert.Equal("ListOfNumbers", mappingSet.MappingSet[1].DestinationName);
            Assert.Equal("ListSource", mappingSet.MappingSet[1].SourceNameName);
            Assert.Equal(typeof(IntListDbColToTypeConverter).FullName, mappingSet.MappingSet[1].Converter.GetType().FullName);
        }
    }
}
