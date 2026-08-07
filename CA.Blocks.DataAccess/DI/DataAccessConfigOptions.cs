namespace CA.Blocks.DataAccess.DI
{

    /// <summary>
    /// provides the interfaces and some common implementations for configuring the connection strings  
    /// </summary>
    public interface IDataAccessConfigOptions
    {
        string ConnectionStringKey {get;}
        bool DebugTrace { get;  }
        bool TraceExceptions { get; }
        int TransientErrorRetryTotalNumberOfTimesToTry { get; }
        int TransientErrorRetryRetryIntervalSeconds { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataAccessConfigOptions : IDataAccessConfigOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public DataAccessConfigOptions()
        {
            // Defaults
            ConnectionStringKey = string.Empty;
            DebugTrace = false;
            TraceExceptions = true;
            TransientErrorRetryTotalNumberOfTimesToTry = 3;
            TransientErrorRetryRetryIntervalSeconds = 10;
        }

#if NET6_0_OR_GREATER
        public string ConnectionStringKey {get; init;}
        public bool DebugTrace { get; init; }
        public bool TraceExceptions { get; init; }
        public int TransientErrorRetryTotalNumberOfTimesToTry { get; init; }
        public int TransientErrorRetryRetryIntervalSeconds { get; init; }
#else
        public string ConnectionStringKey {get; set;}
        public bool DebugTrace { get; set; }
        public bool TraceExceptions { get; set; }
        public int TransientErrorRetryTotalNumberOfTimesToTry { get; set; }
        public int TransientErrorRetryRetryIntervalSeconds { get; set; }
#endif
    }
}

