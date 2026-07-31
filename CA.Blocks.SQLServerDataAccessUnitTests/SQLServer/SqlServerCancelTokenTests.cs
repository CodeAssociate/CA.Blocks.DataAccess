
using System.Diagnostics;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    // Shows how to pass a Context to SQL on each execute  for unit testing this is simply a random guid
    // in most cases this will be the security context like user name. This is useful in case you want to 
    // do auditing, however the using the 

    internal class CancelTokenTestsDataAccess() : UnitTestDataAccess(new DataAccessConfigOptions
    {
        ConnectionStringKey = "ignored", 
        TransientErrorRetryRetryIntervalSeconds = 10,
        TransientErrorRetryTotalNumberOfTimesToTry = 3
    })
    {
        public async Task ExecuteWithCancelToken(TimeSpan dbExecuteTime, int cmdTimeOut, CancellationToken cancellationToken)
        {
            // here we all a set with three cases:
            // 1 the execute time ino the db
            // 2 the configured .net sql Command Timeout
            // 3 a cancel token
            string tsAsString = dbExecuteTime.ToString(@"hh\:mm\:ss");
            var cmd = CreateDbCommand($"WAITFOR DELAY '{tsAsString}';");
            cmd.CommandTimeout = cmdTimeOut;
            await ExecuteNonQueryAsync(cmd, cancellationToken);
        }
    }



    [Collection("DbIntegrationTests")]
    public class SqlServerCancelTokenTests
    {

        [Fact]
        public async Task CaseCleanExecuteNoTimeout()
        {
            var target = new CancelTokenTestsDataAccess();
            // we take 1 second to execute the statement when Command Timeout is 5 seconds and CancellationTokenSource is 10 seconds
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await target.ExecuteWithCancelToken(new TimeSpan(0, 0, 1), 5, timeoutCts.Token);
        }

        [Fact]
        public async Task CaseCleanExecuteDbTriggerTimeout()
        {
            var stopWatch = new Stopwatch();
            var target = new CancelTokenTestsDataAccess();
            // we take 10 seconds to execute the statement when Command Timeout is 2 seconds and CancellationTokenSource is 10 seconds
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            stopWatch.Start();
            await Assert.ThrowsAsync<SqlException>(() =>
                target.ExecuteWithCancelToken(new TimeSpan(0, 0, 10), 2, timeoutCts.Token));
            stopWatch.Stop();
            // make sure we hit the timeout from the db
            Assert.True(stopWatch.ElapsedMilliseconds > 2000);
            Assert.True(stopWatch.ElapsedMilliseconds < 3000);
        }
        
        [Fact]
        public async Task CaseCleanExecuteUserTriggerTimeout()
        {
            var stopWatch = new Stopwatch();
            var target = new CancelTokenTestsDataAccess();
            // we take 10 seconds to execute the statement with Command Timeout is 30 seconds and CancellationTokenSource is 2 seconds
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            stopWatch.Start();
            await Assert.ThrowsAsync<SqlException>(() =>
                target.ExecuteWithCancelToken(new TimeSpan(0, 0, 10), 30, timeoutCts.Token));
            stopWatch.Stop();
            // make sure we hit the timeout from the CancellationTokenSource 
            Assert.True(stopWatch.ElapsedMilliseconds > 2000);
            Assert.True(stopWatch.ElapsedMilliseconds < 3000);
        }
    }
}



