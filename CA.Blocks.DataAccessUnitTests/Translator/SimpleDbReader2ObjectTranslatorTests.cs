using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.DataAccessUnitTests.TestData;
using CA.Blocks.DataAccessUnitTests.TestData.StubObjects;

namespace CA.Blocks.DataAccessUnitTests.Translator;

[TestFixture]
public class SimpleDbReader2ObjectTranslatorTests
{
    public class TestTranslator : SimpleDbReader2ObjectTranslator<TestDataClass>
    {
        protected override TestDataClass CustomTranslate(IDataReader dr)
        {
            var result = new TestDataClass
            {
                IntCol = dr.AsInt("IntCol"),
                StringCol = dr.AsString("StringCol"),
                GuidCol = dr.AsGuid("GuidCol"),
                DateCol = dr.AsDateTime("DateCol")
            };
            return result;
        }
    }

    [Test]
    public void Translate_DataTable()
    {
        // setup 
        var dt = TestDataGenerator.GenerateTestDataForTestDataClassAsDataReader(1, 10);
        var target = new TestTranslator();
        // act 
        var result = target.Translate(dt);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(10));
        Assert.That(result[0].IntCol, Is.EqualTo(1));
    }
}