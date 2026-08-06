using Azure.Core;
using System;
using System.Threading;
using CA.Blocks.DataAccess.DependencyInjection;

namespace CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure
{
    public class AzureManagedIdentityTokenResolver : IConnectionTokenResolver
    {
        private readonly TokenCredential _tokenCredential;

        public AzureManagedIdentityTokenResolver(TokenCredential tokenCredential)
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
                // else flow through to get a new token as this token is going to expire in the next 15 seconds
            }
            if (connectionString.Contains("database.windows.net"))
            {
                var token = _tokenCredential?.GetToken(
                    new TokenRequestContext(new[] { "https://database.windows.net" }),
                    CancellationToken.None);
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
                // The connection string for a AzureManagedIdentity must be for a database in the realm database.windows.net
                return null;
            }
        }
    }
}