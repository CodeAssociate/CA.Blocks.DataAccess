using System;
using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
#if NET6_0_OR_GREATER
using System.Threading.Tasks;
#endif

namespace CA.Blocks.DataAccess.Generic
{

	public class AbstractedDbDataAccessConnector<CON, ADP, CMD> : DataAccessCore , IDisposable
#if NET6_0_OR_GREATER
		, IAsyncDisposable
#endif

	where CON : DbConnection, new()
		where ADP : DbDataAdapter, new()
		where CMD : DbCommand, new()
	{
		private readonly CON _dbConnection;

		public AbstractedDbDataAccessConnector(IDataAccessConfig config, bool pooled = false, IDbRowTranslatorProvider dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
		{
			if (!pooled)
			{
				// as we do not know what driver is used we assume it is not pooled and create managed instance to be used 
				// we do this as if not pooled it expensive to create connect and dispose a connection
				_dbConnection = CreateNewConnection();
				if (_dbConnection == null)
				{
					throw new DataException($"Unable to create instance for {typeof(CON).FullName} ");
				}
			}
		}

		protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
		{
			return (DbDataAdapter)Activator.CreateInstance(typeof(ADP), (CMD)cmd);
		}

		private CON CreateNewConnection()
		{
			return (CON)Activator.CreateInstance(typeof(CON), ConnectionString);
		}

		protected override bool PrepCommand(IDbCommand cmd)
		{
			if (_dbConnection == null)
			{
				var sqlConnection = CreateNewConnection();
				if (sqlConnection != null)
				{
					sqlConnection.Open();
					cmd.Connection = sqlConnection;
					return true;
				}

				throw new DataException($"Unable to create instance for {typeof(CON).FullName} ");
			}
			else
			{
				if (_dbConnection.State == ConnectionState.Closed)
				{
					_dbConnection.Open();
				}

				cmd.Connection = _dbConnection;
				return false;
			}
		}

		protected override bool IsTransientError(DbException dbEx)
		{
			return false;
		}

		public void Dispose()
		{
			if (_dbConnection != null
			    && (_dbConnection.State == ConnectionState.Open
			        || _dbConnection.State == ConnectionState.Executing
			        || _dbConnection.State == ConnectionState.Fetching)
			   )
			{
				_dbConnection.Close();
				_dbConnection.Dispose();
			}
		}

#if NET6_0_OR_GREATER
		public async ValueTask DisposeAsync()
		{
			if (_dbConnection != null
			    && (_dbConnection.State == ConnectionState.Open
			        || _dbConnection.State == ConnectionState.Executing
			        || _dbConnection.State == ConnectionState.Fetching)
			   )
			{
				await _dbConnection.CloseAsync();
				await _dbConnection.DisposeAsync();
			}
		}
#endif

		protected override DbCommand CreateDbCommand(string sql, CommandType cmdType = CommandType.Text)
		{
			return new CMD { CommandText = sql, CommandType = cmdType };
		}

		protected CMD CreateTextCommand(string sql)
		{
			return new CMD { CommandText = sql, CommandType = CommandType.Text };
		}


		protected DataTable GetSchema(string collectionNam, string[] restrictionValues = null)
		{
			if (_dbConnection == null)
			{
				var sqlConnection = CreateNewConnection();
				sqlConnection.Open();
				DataTable result = restrictionValues == null ? sqlConnection.GetSchema(collectionNam) : sqlConnection.GetSchema(collectionNam, restrictionValues);
				sqlConnection.Close();
				return result;
			}
			else
			{

				if (_dbConnection.State == ConnectionState.Closed)
				{
					_dbConnection.Open();
				}
				DataTable result = restrictionValues == null ? _dbConnection.GetSchema(collectionNam) : _dbConnection.GetSchema(collectionNam, restrictionValues);
				return result;
			}
		}
	}
}
