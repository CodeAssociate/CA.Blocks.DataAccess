#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language. Use as example to convert

using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Mappings;
using CA.Blocks.DataAccess.Translator.DbRowToObject;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;


namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    internal class temp
    {
        public int id { get; set; }
        public string? name { get; set; }
    }

    internal class temp3
    {
        public int id { get; set; }

        //[Description("StringDbColToTypeConverter")]
        public string? name { get; set; }
    }
    public class temp2
    {
        public long Id2 { get; set; }
        public string? Name2 { get; set; }
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
        }, () => new temp2())
        {
        }
    }


    [Collection("DbIntegrationTests")]
    public class SqlServerDataAccessExecuteTo : UnitTestDataAccess
    {
        public SqlServerDataAccessExecuteTo()
        {
           if (!DefaultDbRowTranslatorProvider.DefaultInstance.HasTranslatorFor<temp2>())
           {
                ((DefaultDbRowTranslatorProvider)DefaultDbRowTranslatorProvider.DefaultInstance).Add(new Temp2CustomTranslator());
           }
        }

        [Fact]
        public void ExecuteToListOfDev()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = ExecuteToListOf<temp>(cmd);
            Assert.True(result.Count > 0);
            //foreach (var o in result)
            //{
            //    Console.WriteLine($"{o.id},{o.name}");
            //}
            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].id.ToSqlParameter("@id"));
            var singleResult  = ExecuteTo<temp>(cmd);
            Assert.Equal(singleResult.id, result[0].id);
            Assert.Equal(singleResult.name, result[0].name);
        }

        [Fact]
        public void ExecuteToListOfDevCustomTranslator()
        {

            var cmd = CreateDbCommand("Select id, name from sysobjects");
            var result = ExecuteToListOf<temp2>(cmd);
            Assert.True(result.Count > 0);
            var cmdSingle = CreateDbCommand("Select id, name from sysobjects where id=@id");

			cmdSingle.Parameters.Add(result[0].Id2.ToSqlParameter("@id")); // << we what to replace this with a generic solution.

			//cmdSingle.Parameters.Add(cmdSingle.CreateParameter().WithValueAs(result[0].Id2, "@Id"));
			//cmdSingle.CreateParameter().WithValue(result[0].Id2, "@id");
			var singleResult = ExecuteTo<temp>(cmd);
            Assert.Equal(singleResult.id, result[0].Id2);
            Assert.Equal(singleResult.name, result[0].Name2);
        }


        [Fact]
        public void ExecuteToListOfDevCustomTranslatorUsingFunc()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name  from sysobjects");
            var result = ExecuteToListOf<temp2>(cmd, (IDataReader dr) =>
            {
                var rowObj = new temp2();
                rowObj.Id2 = dr.AsInt("id");
                rowObj.Name2 = dr.AsString("name");
                return rowObj;
            });
            Assert.True(result.Count > 0);

            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].Id2.ToSqlParameter("@id"));
            var singleResult = ExecuteTo<temp>(cmd);
            Assert.Equal(singleResult.id, result[0].Id2);
            Assert.Equal(singleResult.name, result[0].Name2);
        }

        private temp2 LocalTranslate(IDataReader dr)
        {
            var rowObj = new temp2();
            rowObj.Id2 = dr.AsInt("id");
            rowObj.Name2 = dr.AsString("name");
            return rowObj;
        }


        [Fact]
        public void ExecuteToListOfDevCustomTranslatorUsingFunc1()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name  from sysobjects");
            var result = ExecuteToListOf<temp2>(cmd, LocalTranslate);
            Assert.True(result.Count > 0);

            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].Id2.ToSqlParameter("@id"));
            var singleResult = ExecuteTo<temp>(cmd);
            Assert.Equal(singleResult.id, result[0].Id2);
            Assert.Equal(singleResult.name, result[0].Name2);
        }



        [Fact]
        public void ExecuteToListOfDevEmptyList()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = ExecuteToListOf<temp>(cmd);
            Assert.Empty(result);
        }


        [Fact]
        public void ExecuteToListOfDevEmptySingle()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = ExecuteTo<temp>(cmd);
            Assert.True(result == null);
        }


        [Fact]
        public void ExecuteToListOfStrutList()
        {
            int defaulint = default;
            SqlCommand cmd = CreateTextCommand("Select id from sysobjects where id <> @id").WithParameter(defaulint.ToSqlParameter("@id"));
            var result = ExecuteToListOf<int>(cmd);
            Assert.True(result.Count > 0);
            var shouldBeEmpty = result.Where(x => x == defaulint).ToList();
            Assert.Empty(shouldBeEmpty);
        }


        #region async Tests

        [Fact]
        public async Task ExecuteToListOfDevAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects");
            var result = await  ExecuteToListOfAsync<temp>(cmd);
            Assert.True(result.Count > 0);
            //foreach (var o in result)
            //{
            //    Console.WriteLine($"{o.id},{o.name}");
            //}
            SqlCommand cmdSingle = CreateTextCommand("Select id, name from sysobjects where id=@id").WithParameter(result[0].id.ToSqlParameter("@id"));
            var singleResult = await ExecuteToAsync<temp>(cmd);
            Assert.Equal(singleResult.id, result[0].id);
            Assert.Equal(singleResult.name, result[0].name);
        }

        [Fact]
        public async Task ExecuteToListOfDevEmptyListAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = await ExecuteToListOfAsync<temp>(cmd);
            Assert.Empty(result);
        }


        [Fact]
        public async Task ExecuteToListOfDevEmptySingleAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where 1=2");
            var result = await ExecuteToAsync<temp>(cmd);
            Assert.True(result == default);
        }

        [Fact]
        public async Task  ExecuteToListOfStrutListAsync()
        {
            int defaulInt = default;
            SqlCommand cmd = CreateTextCommand("Select id from sysobjects where id <> @id").WithParameter(defaulInt.ToSqlParameter("@id"));
            var result = await ExecuteToListOfAsync<int>(cmd);
            Assert.True(result.Count > 0);
            var shouldBeEmpty = result.Where(x => x == defaulInt).ToList();
            Assert.Empty(shouldBeEmpty);
        }



        #endregion

        #region BadTranslate 
        [Fact]
        public void ExecuteToListOfBadTranslate()
        {
            SqlCommand cmd = CreateTextCommand("Select id as id2, name from sysobjects");
            Assert.Throws<ConverterColumnNotFoundException>(() => ExecuteToListOf<temp>(cmd));
        }


        [Fact]
        public void ExecuteToListOfBadTranslateInvalidDateTypes()
        {
            SqlCommand cmd = CreateTextCommand("Select id as name, name as id from sysobjects");
            Assert.Throws<ConverterColumnBadDataException>(() => ExecuteToListOf<temp>(cmd));
        }

        [Fact]
        public async Task ExecuteToListOfBadTranslateAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id as id2, name from sysobjects");
            await Assert.ThrowsAsync<ConverterColumnNotFoundException>(() => ExecuteToListOfAsync<temp>(cmd));
        }

        #endregion

    }

}



