using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions
{
        public class DataReaderExtensionTests : DataReaderExtensionsBaseTests
    {

        [Fact]
        public void DataReaderExtensions_ToListOf()
        {
            var numberOfRecords = 10;
            var testDate = DateTime.Now;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToListOf<TestDataObject>();
            Assert.Equal(numberOfRecords, result.Count);
            Assert.Equal(result[numberOfRecords -1].IntCol, numberOfRecords);
            Assert.Equal(0, result.Count(x => x.DateCol < testDate));
        }



        [Fact]
        public void DataReaderExtensions_ToListOf2()
        {
            var numberOfRecords = 10;
            var testDate = DateTime.Now;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToListOf<TestDataObjectRequired>();
            Assert.Equal(numberOfRecords, result.Count);
            Assert.Equal(result[numberOfRecords - 1].IntCol, numberOfRecords);
            Assert.Equal(0, result.Count(x => x.DateCol < testDate));
        }


        [Fact]
        public void DataReaderExtensions_ToDictionary()
        {
	        var numberOfRecords = 10;
	        var testDate = DateTime.Now;
	        var dataReader = GenerateTestDataReader(numberOfRecords);

	        var result = dataReader.ToDictionary<int, TestDataObject>(x => x.IntCol);
	        Assert.Equal(numberOfRecords, result.Count);
	        Assert.Equal(result[numberOfRecords].IntCol, numberOfRecords);
        }


		[Fact]
        public void DataReaderExtensions_ToSingleNamedColumnList_String()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<string>("StringCol");
            Assert.Equal(numberOfRecords, result.Count);
            Assert.Contains(numberOfRecords.ToString(), result[numberOfRecords - 1]);
        }


        [Fact]
        public void DataReaderExtensions_ToSingleNamedColumnList_Int()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<int>("intCol");
            Assert.Equal(numberOfRecords, result.Count);
            Assert.Equal(numberOfRecords, result[numberOfRecords -1]);
        }


        [Fact]
        public void DataReaderExtensions_ToResultsSet2()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(2, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject>();
            Assert.Equal(numberOfRecords, result.Results1.Count);
            Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results2.Count);
            Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);
        }


        [Fact]
        public void DataReaderExtensions_ToResultsSet3()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
            Assert.Equal(numberOfRecords, result.Results1.Count);
            Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results2.Count);
            Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.Equal(numberOfRecords, result.Results3.Count);
            Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);
        }


        [Fact]
        public void DataReaderExtensions_ToResultsSet4()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(4, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.Equal(numberOfRecords, result.Results1.Count);
            Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results2.Count);
            Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.Equal(numberOfRecords, result.Results3.Count);
            Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results4.Count);
            Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);
        }

        [Fact]
        public void DataReaderExtensions_ToResultsSet5()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(5, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.Equal(numberOfRecords, result.Results1.Count);
            Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results2.Count);
            Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.Equal(numberOfRecords, result.Results3.Count);
            Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results4.Count);
            Assert.Equal(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results5.Count);
            Assert.Equal(numberOfRecords * 5, result.Results5[numberOfRecords - 1].IntCol);
        }


        [Fact]
        public void DataReaderExtensions_ToResultsSet4_MissingData()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.Equal(numberOfRecords, result.Results1.Count);
            Assert.Equal(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.Equal(numberOfRecords, result.Results2.Count);
            Assert.Equal(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.Equal(numberOfRecords, result.Results3.Count);
            Assert.Equal(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.Equal(0, result.Results4.Count);
        }
    }


}
