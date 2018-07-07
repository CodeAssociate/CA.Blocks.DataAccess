using System;
using System.Collections.Generic;
using System.Text;

namespace CA.CoreBlocks.DataAccess.DI
{
    public interface IDataAccessCoreOptions
    {
        string ConnectionStringKey {get; set;}
        bool DebugTrace { get; set; }
    }

    public class DataAccessCoreOptions : IDataAccessCoreOptions
    {
        public string ConnectionStringKey {get; set;}
        public bool DebugTrace { get; set; }
    }


}
