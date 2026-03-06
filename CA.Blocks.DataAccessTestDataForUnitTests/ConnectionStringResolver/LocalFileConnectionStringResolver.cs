using CA.Blocks.DataAccess.DI;
using System;
using System.IO;


namespace CA.Blocks.DataAccessTestDataForUnitTests.ConnectionStringResolver
{
    public class LocalFileConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    {
        private string _secretConnectionString = null;
        private static object _lockObj = new object();

        private string _connectionStringSecretFile = null;
        public LocalFileConnectionStringResolver(string connectionStringSecretFile)
        {
            _connectionStringSecretFile = connectionStringSecretFile;
        }

        private string GetSecretsFilename()
        {
            var secretsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CA.Secrets");
            return Path.Combine(secretsPath, _connectionStringSecretFile);
        }

        public string GetConnectionString(string connectionStringKey)
        {
            if (_secretConnectionString == null)
            {
                lock (_lockObj)
                {
                    if (_secretConnectionString == null)
                    {
                        var secretsFileName = GetSecretsFilename();
                        if (File.Exists(secretsFileName))
                        {
                            _secretConnectionString = File.ReadAllText(secretsFileName);
                        }
                        //else
                        //{
                        //    Assert.Ignore($"Connection String string not set in ${secretsFileName}. To run these test we need a connection to a MySql instance");
                        //}
                    }

                }
            }

            return _secretConnectionString;
        }

        public bool ConfigExists()
        {
            if (!string.IsNullOrEmpty(_secretConnectionString))
            {
                var secretsFileName = GetSecretsFilename();
                return File.Exists(secretsFileName);

            }
            return true;
        }
    }

}
