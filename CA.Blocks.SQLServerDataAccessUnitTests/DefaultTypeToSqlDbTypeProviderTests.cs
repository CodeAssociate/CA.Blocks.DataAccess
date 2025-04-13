using System;
using System.Data;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests;

[TestFixture]
public class DefaultTypeToSqlDbTypeProviderTests
{

    [TestCase(typeof(string), SqlDbType.NVarChar)]
    [TestCase(typeof(string), SqlDbType.VarChar, "VarChar")]
    [TestCase(typeof(string), SqlDbType.VarChar, "varchar")]
    [TestCase(typeof(long), SqlDbType.BigInt)]
    [TestCase(typeof(long?), SqlDbType.BigInt)]
    //[TestCase(typeof(string), SqlDbType.NVarChar)]
    public void DefaultMappings(Type source, SqlDbType dbtype, string specific = null)
    {
        var r = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(source, specific);
        Assert.That(r, Is.EqualTo(dbtype));
    }

}