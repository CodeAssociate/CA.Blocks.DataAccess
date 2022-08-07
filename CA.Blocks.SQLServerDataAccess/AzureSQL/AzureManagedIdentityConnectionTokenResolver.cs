using Azure.Core;

namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    public class AzureManagedIdentityConnectionTokenResolver : IConnectionTokenResolver
    {
        private readonly TokenCredential _tokenCredential;

        public AzureManagedIdentityConnectionTokenResolver(TokenCredential tokenCredential)
        {
            _tokenCredential = tokenCredential;
        }

        private string _existingToken;
        /// <summary>
        /// Get a new Azure Token
        /// </summary>
        /// <returns></returns>
        public string GetConnectionToken(string connectionString)
        {
            if (_existingToken != null)
            {
                return _existingToken;
            }

            if (connectionString.Contains("database.windows.net") && !(connectionString.Contains("Password=")))
            {
                var token = _tokenCredential?.GetToken(
                    new TokenRequestContext(new[] { "https://database.windows.net" }),
                    default);
                _existingToken = token?.Token;
                return _existingToken;
            }
            else
            {
                // The connection string for a AzureManagedIdentityC must not contain a password and must be for a database in the realm database.windows.net
                return null;
            }
        }
    }
}