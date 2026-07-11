using Azure.Core;
using System;


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
        private DateTimeOffset _dateTimeExpires;
        /// <summary>
        /// Get a new Azure Token
        /// </summary>
        /// <returns></returns>
        public string GetConnectionToken(string connectionString)
        {
            if (_existingToken != null)
            {
                if (_dateTimeExpires > DateTimeOffset.Now.Subtract(new TimeSpan(0,0,15)))
                {
                    return _existingToken;
                }
                // else flow throw to get a new token as this token is going to expire in the next 15 seconds
            }

            if (connectionString.Contains("database.windows.net") && !(connectionString.Contains("Password=")))
            {
                var token = _tokenCredential?.GetToken(
                    new TokenRequestContext(new[] { "https://database.windows.net" }),
                    default);
                if (token.HasValue)
                {
                    _existingToken = token.Value.Token;
                    _dateTimeExpires = token.Value.ExpiresOn;
                    return _existingToken;
                }

                return null;
            }
            else
            {
                // The connection string for a AzureManagedIdentityC must not contain a password and must be for a database in the realm database.windows.net
                return null;
            }
        }
    }
}