using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    // Shows how to pass a Context to SQL on each execute  for unit testing this is simply a random guid
    // in most cases this will be the security context like user name. This is useful in case you want to 
    // do auditing, however the using the 

    internal class TransientErrorUnitTestDataAccess : UnitTestDataAccess
    {

        public TransientErrorUnitTestDataAccess() : base(new DataAccessConfigOptions
            { ConnectionStringKey = "localsqlserverhost", TransientErrorRetryRetryIntervalSeconds = 1, TransientErrorRetryTotalNumberOfTimesToTry = 3})
        {
        }

        public int DbTransientErrorDbErrorCount { get; private set; }
        public int DbErrorDbErrorCount { get; private set; }


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

If @count < @WorkOnStatementNumber
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
            DbTransientErrorDbErrorCount++;
            TestContext.WriteLine($"Try number {DbTransientErrorDbErrorCount} and fail with - {cmd.CommandText}");
            base.TraceTransientErrorDbError(cmd, ex);
        }

        protected override void TraceDbError(IDbCommand cmd, DbException ex)
        {
            DbErrorDbErrorCount++;
            TestContext.WriteLine($"Try number {DbTransientErrorDbErrorCount} and fail with - {cmd.CommandText}");
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
            DbTransientErrorDbErrorCount = 0;
            DbErrorDbErrorCount = 0;
        }
        

        public int ExecuteTestScriptAsScalar(int workOnStatementNumber)
        { 
            var cmd = CreateTextCommand(SQLMOCKTransientErrorSQL).WithParameter(workOnStatementNumber.ToSqlParameter("@WorkOnStatementNumber"));
            return ExecuteScalarAs<int>(cmd);
        }

        public int ExecuteTestScriptAsDataRow(int workOnStatementNumber)
        {
            var cmd = CreateTextCommand(SQLMOCKTransientErrorSQL).WithParameter(workOnStatementNumber.ToSqlParameter("@WorkOnStatementNumber"));
            var dr =  ExecuteDataRow(cmd);
            return dr.AsInt(0);
        }

        public int ExecuteTestScriptAsScalarAsync(int workOnStatementNumber)
        {
            var cmd = CreateTextCommand(SQLMOCKTransientErrorSQL).WithParameter(workOnStatementNumber.ToSqlParameter("@WorkOnStatementNumber"));
            var task = ExecuteScalarAsAsync<int>(cmd);
            task.Wait();
            return task.Result;
        }

    }



    [TestFixture]
    public class SqlServerDataAccessTransientErrorTests
    {
        private TransientErrorUnitTestDataAccess _targetDal = new TransientErrorUnitTestDataAccess();

        [SetUp]
        public void Init()
        {
            _targetDal.PrepTest();

        }

        [TearDown]
        public void Cleanup()
        {
            _targetDal.CleanUp();
        }


        [Test]
        public void BasicTestTransientUsingScalarOneError()
        {
            var result = _targetDal.ExecuteTestScriptAsScalar(2);
            Assert.AreEqual(1, _targetDal.DbTransientErrorDbErrorCount);
            Assert.AreEqual(2, result);
        }


        [Test]
        public void BasicTestTransientUsingScalarTwoErrors()
        {
            var result = _targetDal.ExecuteTestScriptAsScalar(3);
            Assert.AreEqual(2, _targetDal.DbTransientErrorDbErrorCount);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void BasicTestTransientUsingScalarThreeErrors()
        {
            try
            {
                var result = _targetDal.ExecuteTestScriptAsScalar(4);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf(typeof(SqlException), ex);
                Assert.AreEqual(51234, ((SqlException) ex).Number);
                Assert.AreEqual(2, _targetDal.DbTransientErrorDbErrorCount);
                Assert.AreEqual(1, _targetDal.DbErrorDbErrorCount);
            }
        }

        [Test]
        public void BasicTestTransientUsingScalar()
        {
            var result = _targetDal.ExecuteTestScriptAsScalar(3);
            Assert.AreEqual(2, _targetDal.DbTransientErrorDbErrorCount);
            Assert.AreEqual(3, result);
        }


        [Test]
        public void BasicTestTransientUsingDataRow()
        {
            var result = _targetDal.ExecuteTestScriptAsDataRow(3);
            Assert.AreEqual(2, _targetDal.DbTransientErrorDbErrorCount);
            Assert.AreEqual(3, result);
        }


        [Test]
        public void ExecuteTestScriptAsScalarAsync()
        {
            var result = _targetDal.ExecuteTestScriptAsScalarAsync(3);
            Assert.AreEqual(2, _targetDal.DbTransientErrorDbErrorCount);
            Assert.AreEqual(3, result);
        }

    }


}
