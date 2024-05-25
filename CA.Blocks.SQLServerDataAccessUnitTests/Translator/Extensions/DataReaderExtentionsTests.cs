using System;
using System.Diagnostics;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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

            var result = Execute(cmd).ToSingleNamedColumnList<string>("Name");

            ClassicAssert.IsTrue(result.Count > 0);

            foreach (var item in result)
            {
                ClassicAssert.IsTrue(item.StartsWith("sys", StringComparison.CurrentCultureIgnoreCase));
            }
        }

    

        public class sysobject
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        [Test]
        public void ExeucteDataReaderToResultsSet()
        {
            SqlCommand cmd = CreateTextCommand("Select id, name from sysobjects; Select * from sysindexes;");

            var result = Execute(cmd).ToResultsSet<sysobject, sysobject>();

            ClassicAssert.IsTrue(result.Results1.Count > 0);

            ClassicAssert.IsTrue(result.Results2.Count > 0);
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
            ClassicAssert.IsTrue(result.Results1.Count != result.Results2.Count);
        }

    }
}
