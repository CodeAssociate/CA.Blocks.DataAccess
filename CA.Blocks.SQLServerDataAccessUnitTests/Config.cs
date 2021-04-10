/*using System.Configuration;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{
    [SetUpFixture]
    public class Config
    {
        [OneTimeSetUp] // [OneTimeSetUp] for NUnit 3.0 and up
        public void SetUp()
        {
            // we do this here as different test frameworks use and merge the app.config depending on the run time.
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            var csSection =  config.ConnectionStrings;
            csSection.ConnectionStrings.Clear();
            //csSection.ConnectionStrings.Add(new ConnectionStringSettings( "localsqlserverhost", "Server=(local);Database=tempdb;Integrated Security=SSPI", "System.Data.SqlClient"));
            csSection.ConnectionStrings.Add(new ConnectionStringSettings("localsqlserverhost", "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true", "System.Data.SqlClient"));
            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}*/


