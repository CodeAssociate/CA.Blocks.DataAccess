
using System.Collections.Generic;
using System.Linq;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUlid;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeUlidAsStringTests : UnitTestDataAccess
    {
        private IList<Ulid> _testData = new List<Ulid>();


        private class UlidDataType
        {
            public Ulid Col { get; set; }
        }

        private void InsertTestDataSQL(Ulid data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data.ToString()}'"));
        }


        private void LoadTestData()
        {
            _testData.Clear();
            _testData.Add(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V")); // data for 17/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H5M4AVZGAM9FS8TAQEY6CH7R")); // data for 18/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR")); // data for 20/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H610AEZG2A3E3NRS3V5QH477")); // data for 23/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H6B9XAZGW2RDKHWHTB15JQ9W")); // data for 27/07/2023 9:12:08 AM +00:00 

        }

        [OneTimeSetUp]
        public void RegisterTypeConverter()
        {
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());
        }

        [SetUp]
        public void Setup()
        {
     

            LoadTestData();

            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("char(26) not null"));

            foreach (var item in _testData)
            {
                InsertTestDataSQL(item);
            }

        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllData()
        {
            //Setup 
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UlidDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<UlidDataType>();
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V"), data[0].Col);
        }




        [Test]
        public void SelectAllDataWithFilter()
        {
            //setup
            var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.AsString().ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToListOf<UlidDataType>();

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }

        [Test]
        public void SelectSingleWithFilter()
        {
            var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.AsString().ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToFirstOrDefault<UlidDataType>();

            //Asert
            ClassicAssert.AreEqual(testvalue, data.Col);
        }
    }
    ////
    ///
    ///
    ///

    [TestFixture]
    public class DbTypeUlidAsBinaryTests : UnitTestDataAccess
    {
        private IList<Ulid> _testData = new List<Ulid>();


        private class UlidDataType
        {
            public Ulid Col { get; set; }
        }


        private string ToBinaryString(Ulid data)
        {
            return "0x" + string.Join("", data.ToByteArray().Select(b => b.ToString("x2")));
        }

        private void InsertTestDataSQL(Ulid data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"{ToBinaryString(data)}"));
        }


        private void LoadTestData()
        {
            _testData.Clear();
            _testData.Add(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V")); // data for 17/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H5M4AVZGAM9FS8TAQEY6CH7R")); // data for 18/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR")); // data for 20/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H610AEZG2A3E3NRS3V5QH477")); // data for 23/07/2023 9:12:08 AM +00:00 
            _testData.Add(new Ulid("01H6B9XAZGW2RDKHWHTB15JQ9W")); // data for 27/07/2023 9:12:08 AM +00:00 

        }

        [OneTimeSetUp]
        public void RegisterTypeConverter()
        {
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());
        }

        [SetUp]
        public void Setup()
        {
         

            LoadTestData();

            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("binary(16) not null"));

            foreach (var item in _testData)
            {
                InsertTestDataSQL(item);
            }

        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllData()
        {
            //Setup 
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UlidDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<UlidDataType>();
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V"), data[0].Col);
        }




        [Test]
        public void SelectAllDataWithFilter()
        {
            //setup
            var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.AsByteArray().ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToListOf<UlidDataType>();

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }

        [Test]
        public void SelectSingleWithFilter()
        {
            var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.AsByteArray().ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToFirstOrDefault<UlidDataType>();

            //Asert
            ClassicAssert.AreEqual(testvalue, data.Col);
        }
    }
}
