using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions
{
    [TestFixture]
    public class DataReaderExtensionTests : DataReaderExtensionsBaseTests
    {

        [Test]
        public void DataReaderExtensions_ToListOf()
        {
            var numberOfRecords = 10;
            var testDate = DateTime.Now;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToListOf<TestDataObject>();
            Assert.That(result.Count, Is.EqualTo(numberOfRecords));
            Assert.That(numberOfRecords, Is.EqualTo(result[numberOfRecords -1].IntCol));
            Assert.That(result.Count(x => x.DateCol < testDate), Is.EqualTo(0));
        }



        [Test]
        public void DataReaderExtensions_ToListOf2()
        {
            var numberOfRecords = 10;
            var testDate = DateTime.Now;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToListOf<TestDataObjectRequired>();
            Assert.That(result.Count, Is.EqualTo(numberOfRecords));
            Assert.That(numberOfRecords, Is.EqualTo(result[numberOfRecords - 1].IntCol));
            Assert.That(result.Count(x => x.DateCol < testDate), Is.EqualTo(0));
        }


        [Test]
        public void DataReaderExtensions_ToDictionary()
        {
	        var numberOfRecords = 10;
	        var testDate = DateTime.Now;
	        var dataReader = GenerateTestDataReader(numberOfRecords);

	        var result = dataReader.ToDictionary<int, TestDataObject>(x => x.IntCol);
	        Assert.That(result.Count, Is.EqualTo(numberOfRecords));
	        Assert.That(numberOfRecords, Is.EqualTo(result[numberOfRecords].IntCol));
        }


		[Test]
        public void DataReaderExtensions_ToSingleNamedColumnList_String()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<string>("StringCol");
            Assert.That(result.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result[numberOfRecords - 1].Contains(numberOfRecords.ToString()), Is.True);
        }


        [Test]
        public void DataReaderExtensions_ToSingleNamedColumnList_Int()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<int>("intCol");
            Assert.That(result.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result[numberOfRecords -1], Is.EqualTo(numberOfRecords));
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet2()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(2, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject>();
            Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

            Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet3()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
            Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

            Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));


            Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet4()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(4, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

            Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));


            Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

            Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));
        }

        [Test]
        public void DataReaderExtensions_ToResultsSet5()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(5, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

            Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));


            Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

            Assert.That(result.Results4.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results4[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 4));

            Assert.That(result.Results5.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results5[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 5));
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet4_MissingData()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.That(result.Results1.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results1[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords));

            Assert.That(result.Results2.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results2[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 2));


            Assert.That(result.Results3.Count, Is.EqualTo(numberOfRecords));
            Assert.That(result.Results3[numberOfRecords - 1].IntCol, Is.EqualTo(numberOfRecords * 3));

            Assert.That(result.Results4.Count, Is.EqualTo(0));
        }
    }


}
