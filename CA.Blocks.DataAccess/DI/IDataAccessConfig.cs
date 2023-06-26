using System;

namespace CA.Blocks.DataAccess.DI
{
    public interface IDataAccessConfig
    {
        IDataAccessKeyToConnectionStringResolver Resolver { get; }
        IDataAccessConfigOptions Options { get; }
    }

    public class DataAccessConfig : IDataAccessConfig
    {

        [System.Obsolete(@"The parameter for configName is no longer used and can safely be removed, 
The intent was to have the ability to has a single database component to multiple databases, this was never implemented fully as the there are to may combinations
like sharding, split schemas, horizontal segmentation     
we delegate this responsibility to the delegate to the IDataAccessKeyToConnectionStringResolver and assume each Data layer is connecting to a 
single repository as it is easy to create multiple repository classes")]
        public DataAccessConfig(string configName, IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver) : this (options, resolver)
        {
        }

        public DataAccessConfig(IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver)
        {
            Options = options;
            Resolver = resolver;
        }

        public IDataAccessConfigOptions Options { get; }
        public IDataAccessKeyToConnectionStringResolver Resolver { get; }
   
    }
}
