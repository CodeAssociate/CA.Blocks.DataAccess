using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Mappings;
using CA.Blocks.DataAccess.Translator.DbRowToObject;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    internal class temp
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class temp2
    {
        public long Id2 { get; set; }
        public string Name2 { get; set; }
    }

    public class Temp2CustomTranslator : Db2ObjectTranslator<temp2>
    {

        public Temp2CustomTranslator() : base (new DbRowToObjectMappings
        {
            MappingSet = new List<IDbColToTypeMapping>
            {
                new DbColToTypeMapping{DestinationName = "Id2", SourceNameName = "id", Converter  = new LongDbColToTypeConverter()},
                new DbColToTypeMapping{DestinationName = "Name2", SourceNameName = "name", Converter  = new StringDbColToTypeConverter()},
            }
        })
        {
        }
    }


    [TestFixture]
    public class SqlServerDataAccessExecuteTo : UnitTestDataAccess
    {
        [SetUp]
        public void Setup()
        {
           if (!DefaultDbRowTranslatorProvider.DefaultInstance.HasTranslatorFor<temp2>())
           {
                ((DefaultDbRowTranslatorProvider)DefaultDbRowTranslatorProvider.DefaultInstance).Add(new Temp2CustomTranslator());
           }
        }

        [Test]
        public void ExecuteToListOfDev()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = ExecuteToListOf<temp>(cmd);
            Assert.Greater(result.Count, 0);
            //foreach (var o in result)
            //{
            //    TestContext.WriteLine($"{o.id},{o.name}");
            //}
            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].id.ToSqlParameter("@id"));
            var singleResult  = ExecuteTo<temp>(cmd);
            Assert.AreEqual(singleResult.id, result[0].id);
            Assert.AreEqual(singleResult.name, result[0].name);
        }


        [Test]
        public void ExecuteToListOfDevCustomTranslator()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = ExecuteToListOf<temp2>(cmd);
            Assert.Greater(result.Count, 0);
            //foreach (var o in result)
            //{
            //    TestContext.WriteLine($"{o.id},{o.name}");
            //}
            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].Id2.ToSqlParameter("@id"));
            var singleResult = ExecuteTo<temp>(cmd);
            Assert.AreEqual(singleResult.id, result[0].Id2);
            Assert.AreEqual(singleResult.name, result[0].Name2);
        }



        [Test]
        public void ExecuteToListOfDevEmptyList()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = ExecuteToListOf<temp>(cmd);
            Assert.AreEqual(result.Count, 0);
        }


        [Test]
        public void ExecuteToListOfDevEmptySingle()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = ExecuteTo<temp>(cmd);
            Assert.True(result == default);
        }


        [Test]
        public void ExecuteToListOfStrutList()
        {
            int defaulint = default;
            SqlCommand cmd = CreateTextCommand("Select id from sysobjects where id <> @id").WithParameter(defaulint.ToSqlParameter("@id"));
            var result = ExecuteToListOf<int>(cmd);
            Assert.Greater(result.Count, 0);
            var shouldBeEmpty = result.Where(x => x == defaulint).ToList();
            Assert.AreEqual(0 ,shouldBeEmpty.Count);
        }


        #region async Tests

        [Test]
        public async Task ExecuteToListOfDevAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = await  ExecuteToListOfAsync<temp>(cmd);
            Assert.Greater(result.Count, 0);
            //foreach (var o in result)
            //{
            //    TestContext.WriteLine($"{o.id},{o.name}");
            //}
            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].id.ToSqlParameter("@id"));
            var singleResult = await ExecuteToAsync<temp>(cmd);
            Assert.AreEqual(singleResult.id, result[0].id);
            Assert.AreEqual(singleResult.name, result[0].name);
        }

        [Test]
        public async Task ExecuteToListOfDevEmptyListAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = await ExecuteToListOfAsync<temp>(cmd);
            Assert.AreEqual(result.Count, 0);
        }


        [Test]
        public async Task ExecuteToListOfDevEmptySingleAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = await ExecuteToAsync<temp>(cmd);
            Assert.True(result == default);
        }

        [Test]
        public async Task  ExecuteToListOfStrutListAsync()
        {
            int defaulInt = default;
            SqlCommand cmd = CreateTextCommand("Select id from sysobjects where id <> @id").WithParameter(defaulInt.ToSqlParameter("@id"));
            var result = await ExecuteToListOfAsync<int>(cmd);
            Assert.Greater(result.Count, 0);
            var shouldBeEmpty = result.Where(x => x == defaulInt).ToList();
            Assert.AreEqual(0, shouldBeEmpty.Count);
        }




        #endregion

        #region BadTranslate 
        [Test]
        public void ExecuteToListOfBadTranslate()
        {
            SqlCommand cmd = CreateTextCommand("Select id as id2, name from sysobjects");
            Assert.Throws<ConverterColumnNotFoundException>(() => ExecuteToListOf<temp>(cmd));
        }


        [Test]
        public void ExecuteToListOfBadTranslateInvalidDateTypes()
        {
            SqlCommand cmd = CreateTextCommand("Select id as name, name as id from sysobjects");
            Assert.Throws<ConverterColumnBadDataException>(() => ExecuteToListOf<temp>(cmd));
        }

        [Test]
        public async Task ExecuteToListOfBadTranslateAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id as id2, name from sysobjects");
            Assert.ThrowsAsync<ConverterColumnNotFoundException>(() => ExecuteToListOfAsync<temp>(cmd));
        }

        #endregion

    }

}
