using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    // Shows how to pass a Context to SQL on each execute  for unit testing this is simply a random guid
    // in most cases this will be the security context like user name. This is useful in case you want to 
    // do auditing, however the using the 

    internal class TransientErrorUnitTestDataAccess : UnitTestDataAccess
    {
        public int dbErrorCount {get; private set; }
     
        #region SQL scripts
        private string SQLCleanupSQL = @"
If Exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CABLOCKS_MockTransientError')
BEGIN 
	Drop table CABLOCKS_MockTransientError
END";
        private string SQLSetupSQL = @"
Create Table CABLOCKS_MockTransientError(col int)";
        
        private string SQLMOCKTransientErrorSQL = @"
insert into CABLOCKS_MockTransientError values  (1)
Declare @count int
select @count = count(*) from CABLOCKS_MockTransientError

If @count < 3
BEGIN
	THROW 51234, 'mock Transient error', 1; 
END
ELSE
BEGIN
	Select @count as CountValue
END
";
        #endregion
        
        protected override List<int> TransientErrorNumbers()
        {
            return new List<int> { 51234 };
        }

        protected override void TraceTransientErrorDbError(IDbCommand cmd, DbException ex)
        {
            dbErrorCount++;
            TestContext.WriteLine($"Try number {dbErrorCount} and fail with - {cmd.CommandText}");
            base.TraceDbError(cmd, ex);
        }

        public void CleanUp()
        {
            var cmd = CreateTextCommand(SQLCleanupSQL);
            ExecuteNonQuery(cmd); 
        }

        public void PrepTest()
        {
            CleanUp();
            var cmd = CreateTextCommand(SQLSetupSQL);
            ExecuteNonQuery(cmd);
        }
        

        public int ExecuteTestScriptAsScalar()
        {
            dbErrorCount = 0;
            var cmd = CreateTextCommand(SQLMOCKTransientErrorSQL);
            return ExecuteScalarAs<int>(cmd);
        }

    }



    [TestFixture]
    public class SqlServerDataAccessTransientErrorTests
    {
        private TransientErrorUnitTestDataAccess _targetDal = new TransientErrorUnitTestDataAccess();

        [SetUp]
        public void SetUpFixture()
        {
            _targetDal.PrepTest();

        }

        [TearDown]
        public void TearDownFixture()
        {
            _targetDal.CleanUp();
        }


        [Test]
        public void BasicTestTransientUsingScalar()
        {
            var result = _targetDal.ExecuteTestScriptAsScalar();
            Assert.AreEqual(2, _targetDal.dbErrorCount);
            Assert.AreEqual(3, result);
        }
    }


}
