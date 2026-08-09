using System.Data;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccess.Builder;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Builder
{
    public class BatchSqlBuilderTests
    {
        [Fact]
        public void SimpleBatchSqlBuilderTests()
        {
            var builder = new BatchSqlBuilder();
            builder.AddSqlFragment("INSERT INTO TestName (Id, Name) values ");
            for (int i = 0; i < 3; i++)
            {
                builder.AddSqlOpenBracket();
                builder.AddIndexedParameter((x) => i.ToSqlParameter(x), "@id", i);
                builder.AddSqlComma();
                builder.AddIndexedParameter((x) => $"{i}_string".ToSqlParameter(x), "@name", i);
               
                builder.AddSqlCloseBracket();
                if (i < 2)
                {
                    builder.AddSqlFragment(","); 
                }
            }
            Assert.Equal("INSERT INTO TestName (Id, Name) values (@id_0, @name_0),(@id_1, @name_1),(@id_2, @name_2)", builder.GetSqlStatement());
            var parameters = builder.GetParameters();
            Assert.Equal(6, parameters.Count);
            Assert.All(parameters.Where(x => x.ParameterName.StartsWith("@id")),
                p =>
                {
                    Assert.NotNull(p.Value);
                    Assert.IsType<int>(p.Value);
                    Assert.Equal(DbType.Int32, p.DbType);
                }
            );
        }
    }
}