using System;

namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    /// <summary>
    /// When connecting to a Azure Database we have the option of using a Token for Auth   
    /// </summary>
    [Obsolete("Moved to CA.Blocks.DataAccess.DependencyInjection can you use that directly")]
    public interface IConnectionTokenResolver : CA.Blocks.DataAccess.DependencyInjection.IConnectionTokenResolver
    {
    }
}
