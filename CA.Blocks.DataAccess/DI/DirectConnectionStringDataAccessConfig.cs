namespace CA.Blocks.DataAccess.DI
{
	// This config delegates the connection string resolution to the IDataAccessKeyToConnectionStringResolver,  ie the connection string key is not used
	public class DirectConnectionStringDataAccessConfig : DataAccessConfig
	{
		public DirectConnectionStringDataAccessConfig(IDataAccessKeyToConnectionStringResolver resolver) :
			base(new DataAccessConfigOptions(), resolver)
		{

		}
	}
}