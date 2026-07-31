#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language. Use as example to convert
// ReSharper disable InconsistentNaming

using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.Data.SqlClient;


namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.Extensions
{
    [Collection("DbIntegrationTests")]
    public class DbDataReaderExtensionsTests : UnitTestDataAccess
    {
        [Fact]
        public async Task ExeucteDataReaderAsString()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where name like @test")
                .WithParameter("sys%".ToSqlParameter("@test"));

            var result = await ExecuteAsync(cmd, TestContext.Current.CancellationToken).ToSingleNamedColumnList<string>("Name");

            Assert.True(result.Count > 0);

            foreach (var item in result)
            {
                Assert.StartsWith("sys", item, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public class sysobject
        {
            public int id { get; init; }
            public required string name { get; init; }
        }

        [Fact]
        public async Task ExeucteDataReaderToResultsSetAsync()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects; Select * from sysindexes;");

            var result = await ExecuteAsync(cmd, TestContext.Current.CancellationToken).ToResultsSet<sysobject, sysobject>();

            Assert.True(result.Results1.Count > 0);

            Assert.True(result.Results2.Count > 0);
            if (result.Results1.Count == result.Results2.Count)
            {
                for (int i = 0; i < result.Results1.Count; i++)
                {
                    if (result.Results1[i].id == result.Results1[2].id)
                    {

                    }
                    else
                    {
                        break;
                    }
                    Assert.Fail("The lists are the same, there should be two different data sets");
                }
            }
            else
            {
               // we good they not the same lists as they are different sizes
            }

            Assert.True(result.Results1.Count != result.Results2.Count);
        }
    }

}




