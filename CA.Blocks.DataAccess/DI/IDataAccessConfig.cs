namespace CA.Blocks.DataAccess.DI
{
    public interface IDataAccessConfig
    {
        // This can use used for multiple DB configs in the same app. for example connection to 2 different databases or 2 different components
        // The Other way is using reflection
        string ConfigName { get; }
        IDataAccessKeyToConnectionStringResolver Resolver { get; }
        IDataAccessConfigOptions Options { get; }

    }

    public class DataAccessConfig : IDataAccessConfig
    {
        public DataAccessConfig(string configName, IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver)
        {
            ConfigName = configName;
            Options = options;
            Resolver = resolver;
        }

        public string ConfigName { get; }
        public IDataAccessKeyToConnectionStringResolver Resolver { get; }
        public IDataAccessConfigOptions Options { get; }
    }

    public class SimpleConnectionStringDataAccessConfig : DataAccessConfig
    {
        public SimpleConnectionStringDataAccessConfig(string connectionString) : 
            base("NotUsed", new DataAccessConfigOptions { ConnectionStringKey = "NotUsed" },
                new HardCodedConnectionStringsResolver(connectionString))
        {

            
        }
    }
}
