using CA.Blocks.DataAccess.Extensions.Translators.Json.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using System.Text.Json;

namespace CA.Blocks.DataAccess.Extensions.Translators.JsonTests.Converters
{

    [TestFixture]
    public class JsonDbColToTypeConverterUnitTests : BaseDbColToTypeConverterTests
    {

        public static string TestData =
            @"[{""colour"": ""red"",""value"": ""#f00""},{""colour"": ""green"",""value"": ""#0f0""}]";


        private class ColourValueDataType
        {
            public string? Colour { get; set; }
            public string? Value { get; set; }
        }

        [Test]
        public void DbColToTypeConverterTest()
        {
            var dt = CreateTestTable(typeof(string), TestData);
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);

            //var expected = new List<ColourValueDataType>
            //{
            //    new ColourValueDataType { Colour = "red", Value = "#f00" },
            //    new ColourValueDataType { Colour = "green", Value = "#0f0" }
            //};

            var target = new JsonDbColToTypeConverter<List<ColourValueDataType>>(new JsonSerializerOptions{ PropertyNameCaseInsensitive = true});

            Assert.That(target.GetDataValue(dataRow, "col")[1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataRow, 1)[1].Colour, Is.EqualTo("green"));

            Assert.That(target.GetDataValue(dataReader, "col")[1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataReader, 1)[1].Colour, Is.EqualTo("green"));
        }


        [Test]
        public void NullDbColToTypeConverterTestWithNull()
        {

            var dt = CreateTestTable(typeof(string), TestData);
            var dataRow = GetDataRow(0, dt);
            var dataReader = GetDataReader(0, dt);


            var target = new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(target.GetDataValue(dataRow, "col"), Is.Null);
            Assert.That(target.GetDataValue(dataRow, 1), Is.Null);

            Assert.That(target.GetDataValue(dataReader, "col"), Is.Null);
            Assert.That(target.GetDataValue(dataReader, 1), Is.Null);

        }

        [Test]
        public void NullDbColToTypeConverterTestWithDataNull()
        {
            var dt = CreateTestTable(typeof(string), TestData);
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(target.GetDataValue(dataRow, "col")![1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataRow, 1)![1].Colour, Is.EqualTo("green"));

            Assert.That(target.GetDataValue(dataReader, "col")![1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataReader, 1)![1].Colour, Is.EqualTo("green"));

        }


        [Test]
        public void DbColToTypeReadOnlyConverterTest()
        {
            var dt = CreateTestTable(typeof(string), TestData);
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);

            //var expected = new List<ColourValueDataType>
            //{
            //    new ColourValueDataType { Colour = "red", Value = "#f00" },
            //    new ColourValueDataType { Colour = "green", Value = "#0f0" }
            //};

            var target = new JsonDbInterfacedColToTypeConverter<
                    IReadOnlyList<ColourValueDataType>, List<ColourValueDataType>>
                    (new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var obj = target.GetDataValue(dataRow, 1);
            Assert.That(obj, Is.InstanceOf<IReadOnlyList<ColourValueDataType>>()); 


            Assert.That(target.GetDataValue(dataRow, "col")[1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataRow, 1)[1].Colour, Is.EqualTo("green"));


            Assert.That(target.GetDataValue(dataReader, "col")[1].Colour, Is.EqualTo("green"));
            Assert.That(target.GetDataValue(dataReader, 1)[1].Colour, Is.EqualTo("green"));
        }


        [Test]
        public void DbColToTypeEmptyReadOnlyConverterTest()
        {
            var dt = CreateTestTable(typeof(string), "");
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new JsonDbInterfacedColToTypeConverter<
                    IReadOnlyList<ColourValueDataType>, List<ColourValueDataType>>
                    (new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var obj = target.GetDataValue(dataRow, 1);
            Assert.That(obj, Is.InstanceOf<IReadOnlyList<ColourValueDataType>>());
        }

        [Test]
        public void DbColToTypeEmptyNullReadOnlyConverterTest()
        {
            var dt = CreateTestTable(typeof(string), null);
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new JsonDbInterfacedColToTypeConverter<
                    IReadOnlyList<ColourValueDataType>, List<ColourValueDataType>>
                    (new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var obj = target.GetDataValue(dataRow, 1);
            Assert.That(obj, Is.InstanceOf<IReadOnlyList<ColourValueDataType>>());
        }

        [Test]
        public void DbColToTypeEmptyArrayReadOnlyConverterTest()
        {
            var dt = CreateTestTable(typeof(string), "[]");
            var dataRow = GetDataRow(1, dt);
            var dataReader = GetDataReader(1, dt);


            var target = new JsonDbInterfacedColToTypeConverter<
                    IReadOnlyList<ColourValueDataType>, List<ColourValueDataType>>
                    (new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var obj = target.GetDataValue(dataRow, 1);
            Assert.That(obj, Is.InstanceOf<IReadOnlyList<ColourValueDataType>>());
        }

    }
}
