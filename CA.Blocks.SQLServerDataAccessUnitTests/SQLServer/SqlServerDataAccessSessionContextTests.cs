using System;
using System.Collections.Generic;
using CA.Blocks.SQLServerDataAccess.Model;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    // Shows how to pass a Context to SQL on each execute  for unit testing this is simply a random guid
    // in most cases this will be the security context like user name. This is useful in case you want to 
    // do auditing, however the using the 

    internal class SessionContextUnitTestDataAccess : UnitTestDataAccess
    {
        public readonly string RandomStringContextValue = Guid.NewGuid().ToString();
        public readonly Guid RandomGuidContextValue = Guid.NewGuid();
#pragma warning disable SCS0005 // Weak random number generator. only used in testing
        public readonly int RandomIntContextValue = new Random().Next(10000);
#pragma warning restore SCS0005 // Weak random number generator.


        // To pass the context simply override the GetConnectionContext and return the string value you what to pass in.
        // from within SQL you can then use Select Cast(CONTEXT_INFO() as varchar(100)) to get the value backout
        protected override IList<SqlServerSessionContext> GetSessionContext()
        {
            var result = new List<SqlServerSessionContext>();
            result.Add(new SqlServerStringSessionContext
                { Key = "StringContext", Value = RandomStringContextValue, ReadOnly = true });
            result.Add(new SqlServerIntSessionContext { Key = "IntContext", Value = RandomIntContextValue, ReadOnly = true });
            result.Add(new SqlServerGuidSessionContext
                { Key = "GuidContext", Value = RandomGuidContextValue, ReadOnly = true });
            return result;
        }

        public string? GetStringContextDataBase()
        {
            var cmd = CreateTextCommand("SELECT SESSION_CONTEXT(N'StringContext') AS CONTEXTINFO");
            return ExecuteScalarAs<string>(cmd);
        }

        public int GetIntContextDataBase()
        {
            var cmd = CreateTextCommand("SELECT SESSION_CONTEXT(N'IntContext') AS CONTEXTINFO");
            return ExecuteScalarAs<int>(cmd);
        }

        public Guid GetGuidContextDataBase()
        {
            var cmd = CreateTextCommand("SELECT SESSION_CONTEXT(N'GuidContext') AS CONTEXTINFO");
            return ExecuteScalarAs<Guid>(cmd);
        }

    }



    public partial class SqlServerDataAccessContextTests
    {
        [Fact]
        public void GetSessionContextTests()
        {
            var target = new SessionContextUnitTestDataAccess();
            var stringContext = target.GetStringContextDataBase();
            Assert.Equal(target.RandomStringContextValue, stringContext);

            var intContext = target.GetIntContextDataBase();
            Assert.Equal(target.RandomIntContextValue, intContext);

            var GuidContext = target.GetGuidContextDataBase();
            Assert.Equal(target.RandomGuidContextValue, GuidContext);
        }

    }


}




