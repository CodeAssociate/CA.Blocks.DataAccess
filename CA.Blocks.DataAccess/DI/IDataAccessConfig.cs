using System;

namespace CA.Blocks.DataAccess.DI
{
    public interface IDataAccessConfig
    {
        IDataAccessKeyToConnectionStringResolver Resolver { get; }
        IDataAccessConfigOptions Options { get; }
        
        DependencyInjection.IConnectionTokenResolver ConnectionTokenResolver { get; }
    }

    public class DataAccessConfig : IDataAccessConfig
    {
        public DataAccessConfig(IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver) 
            : this (options, resolver, new DependencyInjection.NullConnectionTokenResolver())
        {
        }
        
        public DataAccessConfig(IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver, 
            DependencyInjection.IConnectionTokenResolver connectionTokenResolver)
        {
            Options = options;
            Resolver = resolver;
            ConnectionTokenResolver = connectionTokenResolver;
        }

        public IDataAccessConfigOptions Options { get; }
        public DependencyInjection.IConnectionTokenResolver ConnectionTokenResolver { get; }
        public IDataAccessKeyToConnectionStringResolver Resolver { get; }
   
    }
}
