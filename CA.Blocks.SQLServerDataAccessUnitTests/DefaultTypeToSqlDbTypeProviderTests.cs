using System;
using System.Data;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests;

public class DefaultTypeToSqlDbTypeProviderTests
{
    [Theory]
    [InlineData(typeof(string), SqlDbType.NVarChar, null)]
    [InlineData(typeof(string), SqlDbType.VarChar, "VarChar")]
    [InlineData(typeof(string), SqlDbType.VarChar, "varchar")]
    [InlineData(typeof(long), SqlDbType.BigInt, null)]
    [InlineData(typeof(long?), SqlDbType.BigInt, null)]
    public void DefaultMappings(Type source, SqlDbType dbtype, string? specific)
    {
        var r = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(source, specific);
        Assert.Equal(dbtype, r);
    }
}



