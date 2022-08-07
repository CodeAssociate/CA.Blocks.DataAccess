using System;
using System.Diagnostics;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.Extensions
{
    [TestFixture]
    public class DataReaderExtentionsTests : UnitTestDataAccess
    {
        [Test]
        public void ExeucteDataReaderAsString()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects where name like @test")
                .WithParameter("sys%".ToSqlParameter("@test"));

            var result = ExecuteReader(cmd).ToSingleNamedColumnList<string>("Name");

            Assert.IsTrue(result.Count > 0);

            foreach (var item in result)
            {
                Assert.IsTrue(item.StartsWith("sys", StringComparison.CurrentCultureIgnoreCase));
            }
        }

    }


    [TestFixture]
    public class DataReaderExtentionsTests2 : UnitTestDataAccess
    {

        public class sysobject
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        [Test]
        public void ExeucteDataReaderAsString()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects; Select * from sysindexes;");

            var result = ExecuteReader(cmd).ToResultsSet<sysobject, sysobject>();

            Assert.IsTrue(result.Results1.Count > 0);

            Assert.IsTrue(result.Results2.Count > 0);
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
                    Assert.Fail("The lists are the same, there should be two different datasets");
                }

            }
            else
            {
               // we good they not the same lists as they are different sizes
            }

            Assert.IsTrue(result.Results1.Count != result.Results2.Count);

        }

    }
}
