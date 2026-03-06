//using System;
//using System.IO;
//using CA.Blocks.DataAccess.DI;
//using NUnit.Framework;

//namespace CA.Blocks.MySQLDataAccessUnitTests.Base
//{
//    public class MySQLTestDataAccessKeyToConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
//    {
//        private string _secretConnectionString = null;
//        private static object _lockObj = new object();


//        private string GetSecretsFilename()
//        {
//            var secretsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CA.Secrets");
//            return Path.Combine(secretsPath, "CA.Blocks.MySQLDataAccessUnitTests.Base.MySQLTestDataAccessKeyToConnectionStringResolver.txt");
//        }

//        public string GetConnectionString(string connectionStringKey)
//        {
//            if (_secretConnectionString == null)
//            {
//                lock (_lockObj)
//                {
//                    if (_secretConnectionString == null)
//                    {
//                        var secretsFileName = GetSecretsFilename();
//                        if (File.Exists(secretsFileName))
//                        {
//                            _secretConnectionString = File.ReadAllText(secretsFileName);
//                        }
//                        else
//                        {
//                            Assert.Ignore($"Connection String string not set in ${secretsFileName}. To run these test we need a connection to a MySql instance");
//                        }
//                    }

//                }
//            }

//            return _secretConnectionString;
//        }

//        public bool ConfigExists()
//        {
//            if (!string.IsNullOrEmpty(_secretConnectionString))
//            {
//                var secretsFileName = GetSecretsFilename();
//                return File.Exists(secretsFileName);

//            }
//            return true;
//        }
//    }
//}
