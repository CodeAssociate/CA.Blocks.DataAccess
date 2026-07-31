using CA.Blocks.PostgresDataAccess;
using NpgsqlTypes;

namespace CA.Blocks.PostgresDataAccessTests.Postgres
{


    public class DefaultTypeToSqlDbTypeProviderTests
    {

        [Theory]
        [InlineData(typeof(long), NpgsqlDbType.Bigint)]
        [InlineData(typeof(long?), NpgsqlDbType.Bigint)]
        [InlineData(typeof(byte[]), NpgsqlDbType.Bytea)]
        [InlineData(typeof(byte), NpgsqlDbType.Smallint)]
        [InlineData(typeof(byte?), NpgsqlDbType.Smallint)]
        [InlineData(typeof(char), NpgsqlDbType.Char)]
        [InlineData(typeof(char?), NpgsqlDbType.Char)]
        [InlineData(typeof(bool), NpgsqlDbType.Boolean)]
        [InlineData(typeof(bool?), NpgsqlDbType.Boolean)]
        [InlineData(typeof(DateOnly), NpgsqlDbType.Date)]
        [InlineData(typeof(DateOnly?), NpgsqlDbType.Date)]
        [InlineData(typeof(DateTime), NpgsqlDbType.Timestamp)]
        [InlineData(typeof(DateTime?), NpgsqlDbType.Timestamp)]
        [InlineData(typeof(decimal), NpgsqlDbType.Numeric)]
        [InlineData(typeof(decimal?), NpgsqlDbType.Numeric)]
        [InlineData(typeof(double), NpgsqlDbType.Double)]
        [InlineData(typeof(double?), NpgsqlDbType.Double)]
        [InlineData(typeof(float), NpgsqlDbType.Real)]
        [InlineData(typeof(float?), NpgsqlDbType.Real)]
        [InlineData(typeof(Guid), NpgsqlDbType.Uuid)]
        [InlineData(typeof(Guid?), NpgsqlDbType.Uuid)]
        [InlineData(typeof(short), NpgsqlDbType.Smallint)]
        [InlineData(typeof(short?), NpgsqlDbType.Smallint)]
        [InlineData(typeof(int), NpgsqlDbType.Integer)]
        [InlineData(typeof(int?), NpgsqlDbType.Integer)]
        [InlineData(typeof(string), NpgsqlDbType.Varchar)] // the default for string
        [InlineData(typeof(string), NpgsqlDbType.Text, "text")]
        [InlineData(typeof(string), NpgsqlDbType.Char, "char")]
        [InlineData(typeof(decimal), NpgsqlDbType.Money, "money")]

        public void DefaultMappings(Type source, NpgsqlDbType dbtype, string? specific = null)
        {
            var r = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(source, specific);
            Assert.Equal(dbtype, r);
        }
    }

}