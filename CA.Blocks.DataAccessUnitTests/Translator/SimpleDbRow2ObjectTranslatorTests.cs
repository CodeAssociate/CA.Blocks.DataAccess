using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.DataAccessUnitTests.TestData;
using CA.Blocks.DataAccessUnitTests.TestData.StubObjects;

namespace CA.Blocks.DataAccessUnitTests.Translator
{


        public class SimpleDbRow2ObjectTranslatorTests
    {
        public class TestTranslator : SimpleDbRow2ObjectTranslator<TestDataClass>
        {
            protected override TestDataClass CustomTranslate(DataRow dr)
            {
                var result =  new TestDataClass
                {
                    IntCol = dr.AsInt("IntCol"),
                    StringCol = dr.AsString("StringCol")!,
                    GuidCol = dr.AsGuid("GuidCol"),
                    DateCol = dr.AsDateTime("DateCol")
                };
                return result;
            }
        }

        [Fact]
        public void Translate_DataTable()
        {
            // setup 
            var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10);
            var target = new TestTranslator();
            // act 
            var result = target.Translate(dt);
            
            // assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
            Assert.Equal(1, result[0].IntCol);
        }
        [Fact]
        public void Translate_DataRows()
        {
            // setup 
            var drows = TestDataGenerator.GenerateTestDataForTestDataClassAsDataTable(1, 10).Select();
            var target = new TestTranslator();
            // act 
            var result = target.Translate(drows);

            // assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
            Assert.Equal(1, result[0].IntCol);
        }
    }
}
