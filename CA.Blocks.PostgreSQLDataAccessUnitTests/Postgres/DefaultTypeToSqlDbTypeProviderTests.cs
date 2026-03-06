using CA.Blocks.PostgreSQLDataAccess;
using NpgsqlTypes;
using NUnit.Framework;


namespace CA.Blocks.PostgreSQLDataAccessUnitTests;

[TestFixture]
public class DefaultTypeToSqlDbTypeProviderTests
{

    [TestCase(typeof(long), NpgsqlDbType.Bigint)]
    [TestCase(typeof(long?), NpgsqlDbType.Bigint)]
    [TestCase(typeof(byte[]), NpgsqlDbType.Bytea)]
    [TestCase(typeof(byte), NpgsqlDbType.Smallint)]
    [TestCase(typeof(byte?), NpgsqlDbType.Smallint)]
    [TestCase(typeof(char), NpgsqlDbType.Char)]
    [TestCase(typeof(char?), NpgsqlDbType.Char)]
    [TestCase(typeof(bool), NpgsqlDbType.Boolean)]
    [TestCase(typeof(bool?), NpgsqlDbType.Boolean)]
    [TestCase(typeof(DateOnly), NpgsqlDbType.Date)]
    [TestCase(typeof(DateOnly?), NpgsqlDbType.Date)]
    [TestCase(typeof(DateTime), NpgsqlDbType.Timestamp)]
    [TestCase(typeof(DateTime?), NpgsqlDbType.Timestamp)]
    [TestCase(typeof(decimal), NpgsqlDbType.Numeric)]
    [TestCase(typeof(decimal?), NpgsqlDbType.Numeric)]
    [TestCase(typeof(double), NpgsqlDbType.Double)]
    [TestCase(typeof(double?), NpgsqlDbType.Double)]
    [TestCase(typeof(float), NpgsqlDbType.Real)]
    [TestCase(typeof(float?), NpgsqlDbType.Real)]
    [TestCase(typeof(Guid), NpgsqlDbType.Uuid)]
    [TestCase(typeof(Guid?), NpgsqlDbType.Uuid)]
    [TestCase(typeof(short), NpgsqlDbType.Smallint)]
    [TestCase(typeof(short?), NpgsqlDbType.Smallint)]
    [TestCase(typeof(int), NpgsqlDbType.Integer)]
    [TestCase(typeof(int?), NpgsqlDbType.Integer)]
    [TestCase(typeof(string), NpgsqlDbType.Varchar)] // the default for string
    [TestCase(typeof(string), NpgsqlDbType.Text, "text")]
    [TestCase(typeof(string), NpgsqlDbType.Char, "char")]
    [TestCase(typeof(decimal), NpgsqlDbType.Money, "money")]
    
    public void DefaultMappings(Type source, NpgsqlDbType dbtype, string? specific = null)
    {
        var r = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(source, specific);
        Assert.That(r, Is.EqualTo(dbtype));
    }

}