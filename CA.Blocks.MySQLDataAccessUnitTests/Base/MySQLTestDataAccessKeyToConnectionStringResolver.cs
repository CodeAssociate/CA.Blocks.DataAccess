using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.MySQLDataAccessUnitTests.Base
{
    public class MySQLTestDataAccessKeyToConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    {
        private string _secretConnectionString = null;
        private static object _lockObj = new object();

        public string GetConnectionString(string connectionStringKey)
        {
            if (_secretConnectionString == null)
            {
                lock (_lockObj)
                {
                    var secretsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CA.Secrets");
                    var secretsFileName = Path.Combine(secretsPath,
                        "CA.Blocks.MySQLDataAccessUnitTests.Base.MySQLTestDataAccessKeyToConnectionStringResolver.txt");
                    if (File.Exists(secretsFileName))
                    {
                        _secretConnectionString = File.ReadAllText(secretsFileName);
                    }
                    else
                    {
                        throw new ApplicationException($"Connection String string not set in ${secretsFileName}");
                        
                    }

                }
            }

            return _secretConnectionString;
        }
    }
}
