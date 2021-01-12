namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// This interface provides the lookup for connectionStringKey this could be from app.config or appsettings.json or could be custom logic
    /// </summary>
    /// <include file = 'IDataAccessKeyToConnectionStringResolverdocs.xml' path='codedocs/member[@name="IDataAccessKeyToConnectionStringResolver"]/*' />
    public interface IDataAccessKeyToConnectionStringResolver
    {
        /// <include file='IDataAccessKeyToConnectionStringResolverdocs.xml' path='codedocs/member[@name="IDataAccessKeyToConnectionStringResolver.GetConnectionString"]/*' />
        string GetConnectionString(string connectionStringKey);
    }
}
