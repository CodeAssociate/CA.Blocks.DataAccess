using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using NUnit.Framework;

namespace CA.CoreBlocks.SQLServerDataAccessUnitTests
{
    [SetUpFixture]
    public class Config
    {
        [OneTimeSetUp] // [OneTimeSetUp] for NUnit 3.0 and up; see http://bartwullems.blogspot.com/2015/12/upgrading-to-nunit-30-onetimesetup.html
        public void SetUp()
        {
            // we do this here as diffrent test frameworks use and merge the app.config depending on the run time.
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
            
            ConnectionStringsSection csSection =
                config.ConnectionStrings;
            csSection.ConnectionStrings.Add(
                new ConnectionStringSettings( "localsqlserverhost", "Server=(local);Database=tempdb;Integrated Security=SSPI", "System.Data.SqlClient"));

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            //We cannot do this as this is read only at this stage 
                /*
            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(
                "localsqlserverhost", "Server=(local);Database=tempdb;Integrated Security=SSPI", "System.Data.SqlClient"));
                */
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
