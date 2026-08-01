using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;

namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    /// <summary>
    /// This class provides a wrapper for Managed Identity authentication using tokens.  This is a common pattern used in Azure where you can authenticate using a Token. Other than that this is  just like the SqlServerDataAccess class
    /// </summary>
    [System.Obsolete("You can get the package CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure. then inject the CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure.AzureManagedIdentityTokenResolver into the IDataAccessConfig")]
    public class AzureSqlServerDataAccess
    {
    }
}