using System.Diagnostics;

namespace CA.Blocks.DataAccess.DI
{

    /// <summary>
    /// provides the interfaces and some common implementations for configuring the connection strings  
    /// </summary>
    public interface IDataAccessConfigOptions
    {
        string ConnectionStringKey {get; set;}
        bool DebugTrace { get; set; }
        bool TraceExceptions { get; set; }
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
            DebugTrace = false;
            TraceExceptions = true;
        }

        public string ConnectionStringKey {get; set;}
        public bool DebugTrace { get; set; }
        public bool TraceExceptions { get; set; }
    }


}
