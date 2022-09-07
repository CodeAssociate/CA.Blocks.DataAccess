using System.ComponentModel.DataAnnotations;
using CA.Blocks.DataAccess.Translator.DbColToType.AttributeExtensions;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbRowToObject
{
    [TestFixture]
    public class DefaultDbRowTranslatorProviderTests
    {
        public class CustomClass1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int IgnoreMe  { get;}
        }

        public class CustomClass2
        {
            public int Id { get; set; }

            [DbColToSourceName("ListSource")]
            [DbColToTypeConverter(typeof(IntListDbColToTypeConverter), ',')]
            public string ListOfNumbers { get; set; }
            public int IgnoreMe { get; }
        }


        [Test]
        public void GenerateDefaultMappingsForTest()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass1>();
            Assert.AreEqual(mappingSet.MappingSet.Count, 2);
            Assert.AreEqual("Id", mappingSet.MappingSet[0].DestinationName);
            Assert.AreEqual("Name", mappingSet.MappingSet[1].DestinationName);
        }

        [Test]
        public void GenerateDefaultMappingsForTest2()
        {
            var result = new DefaultDbRowTranslatorProvider();
            var mappingSet = result.GenerateDefaultMappingsFor<CustomClass2>();
            Assert.AreEqual(2, mappingSet.MappingSet.Count);
            Assert.AreEqual("Id", mappingSet.MappingSet[0].DestinationName);
            Assert.AreEqual("ListOfNumbers", mappingSet.MappingSet[1].DestinationName);
            Assert.AreEqual("ListSource", mappingSet.MappingSet[1].SourceNameName);
            Assert.AreEqual(typeof(IntListDbColToTypeConverter).FullName, mappingSet.MappingSet[1].Converter.GetType().FullName);
        }
    }
}
