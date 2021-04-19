using System.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// This is a hard coded Connection String Resolver.  This class is useful for providing examples and quick prototype code.  Typically you will not use this once rolling out the application.
    /// The Configuration used is a Hosting app concern. See examples at : https://www.codeassociate.com/Blocks/DataAccess/Samples/Connection/Index.html
    /// </summary>
    public class HardCodedConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        private readonly string _connectionString;

        /// <summary>
        /// Creates a new instance of HardCodedConnectionStringsResolver that implements IDataAccessKeyToConnectionStringResolver. The connectionString passed in will be given back at run time. The connectionStringKey is ignored with this class.
        /// </summary>
        /// <param name="connectionString"></param>
        public HardCodedConnectionStringsResolver(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// returns the _connectionString given at creation time.
        /// </summary>
        /// <param name="connectionStringKey">This parameter is ignored in this implementation</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return _connectionString;
        }
    }
}