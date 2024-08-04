using System.Data.Odbc;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Generic;

namespace CA.Blocks.OdbcDataAccess
{

	public class OdbcDataAccess : AbstractedDbDataAccessConnector<OdbcConnection, OdbcDataAdapter, OdbcCommand>
	{

		public OdbcDataAccess(IDataAccessConfig config, bool pooled = false) :
			base(config, pooled, null)
		{

		}
	}
}
