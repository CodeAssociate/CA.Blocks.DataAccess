using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
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



    [TestFixture]
    public class SqlServerDataAccessExecuteTo : UnitTestDataAccess
    {

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
