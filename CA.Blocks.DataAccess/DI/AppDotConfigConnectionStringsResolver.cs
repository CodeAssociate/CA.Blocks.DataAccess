//using System.Configuration;

//namespace CA.Blocks.DataAccess.DI
//{
    // Use the IDataAccessKeyToConnectionStringResolver and pull in the Nuget package
    /*/// <summary>
    /// Uses the App.Config ConnectionStrings, this is common on .NET 1-4.8 frameworks. It will be using the App.config or web.config ConnectionStrings setting
    /// </summary>
    [System.Obsolete("We are going remove class to reduce the number of dependencies taken on the CA.Blocks.DataAccess. The Configuration used is a Hosting app concern see https://www.codeassociate.com/Blocks/DataAccess/Samples/Connection/UsingAppDotConfigConnectionStringsResolver.html for a fix")]
    public class AppDotConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        /// <summary>
        /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
        /// </summary>
        /// <param name="connectionStringKey">The Connection string known to the code</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }
    }*/
//}