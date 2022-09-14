using System;
using System.Linq;
using CA.Blocks.DataAccess.Translator.Extensions;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.Extensions
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
            Assert.AreEqual(numberOfRecords, result.Count);
            Assert.AreEqual(result[numberOfRecords -1].IntCol, numberOfRecords);
            Assert.AreEqual(0, result.Count(x => x.DateCol < testDate));
        }

        [Test]
        public void DataReaderExtensions_ToSingleNamedColumnList_String()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<string>("StringCol");
            Assert.AreEqual(numberOfRecords, result.Count);
            Assert.True(result[numberOfRecords - 1].Contains(numberOfRecords.ToString()));
        }


        [Test]
        public void DataReaderExtensions_ToSingleNamedColumnList_Int()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataReader(numberOfRecords);

            var result = dataReader.ToSingleNamedColumnList<int>("intCol");
            Assert.AreEqual(numberOfRecords, result.Count);
            Assert.AreEqual(numberOfRecords, result[numberOfRecords -1]);
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet2()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(2, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject>();
            Assert.AreEqual(numberOfRecords, result.Results1.Count);
            Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results2.Count);
            Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet3()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject>();
            Assert.AreEqual(numberOfRecords, result.Results1.Count);
            Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results2.Count);
            Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.AreEqual(numberOfRecords, result.Results3.Count);
            Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet4()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(4, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.AreEqual(numberOfRecords, result.Results1.Count);
            Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results2.Count);
            Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.AreEqual(numberOfRecords, result.Results3.Count);
            Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results4.Count);
            Assert.AreEqual(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);
        }

        [Test]
        public void DataReaderExtensions_ToResultsSet5()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(5, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.AreEqual(numberOfRecords, result.Results1.Count);
            Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results2.Count);
            Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.AreEqual(numberOfRecords, result.Results3.Count);
            Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results4.Count);
            Assert.AreEqual(numberOfRecords * 4, result.Results4[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results5.Count);
            Assert.AreEqual(numberOfRecords * 5, result.Results5[numberOfRecords - 1].IntCol);
        }


        [Test]
        public void DataReaderExtensions_ToResultsSet4_MissingData()
        {
            var numberOfRecords = 10;
            var dataReader = GenerateTestDataSetReader(3, 10);

            var result = dataReader.ToResultsSet<TestDataObject, TestDataObject, TestDataObject, TestDataObject>();
            Assert.AreEqual(numberOfRecords, result.Results1.Count);
            Assert.AreEqual(numberOfRecords, result.Results1[numberOfRecords - 1].IntCol);

            Assert.AreEqual(numberOfRecords, result.Results2.Count);
            Assert.AreEqual(numberOfRecords * 2, result.Results2[numberOfRecords - 1].IntCol);


            Assert.AreEqual(numberOfRecords, result.Results3.Count);
            Assert.AreEqual(numberOfRecords * 3, result.Results3[numberOfRecords - 1].IntCol);

            Assert.AreEqual(0, result.Results4.Count);
        }
    }


}
