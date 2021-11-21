using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;

namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    /// <summary>
    /// This class provides a wrapper for Managed Identity authentication using tokens.  This is a common pattern used in Azure where you can authenticate using a Token. Other than that this is  just like the SqlServerDataAccess class 
    /// </summary>
    public class AzureSqlServerDataAccess : SqlServerDataAccess
    {
        private readonly IConnectionTokenResolver _connectionTokenResolver;

        public AzureSqlServerDataAccess(IDataAccessConfig config, IConnectionTokenResolver connectionTokenResolver, IDbRowTranslatorProvider dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
        {
            _connectionTokenResolver = connectionTokenResolver;
        }

        /// <summary>
        /// Get the Token from the registered  IConnectionTokenResolver provider
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionToken()
        {
            return _connectionTokenResolver?.GetConnectionToken(ConnectionString);
        }
    }
}