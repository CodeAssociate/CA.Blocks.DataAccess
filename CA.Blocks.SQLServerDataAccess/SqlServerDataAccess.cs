//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2026 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.SQLServerDataAccess.Builder;
using CA.Blocks.SQLServerDataAccess.Model;

namespace CA.Blocks.SQLServerDataAccess
{
    /// <summary>
    /// Provides a SQL server implementation for DataAccessCore
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SqlServerDataAccess : DataAccessCore
    {

        public const string FILTER_REPLACE_STRING = "/*##FILTER##*/";

        public SqlServerDataAccess(IDataAccessConfig config, IDbRowTranslatorProvider? dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
        {

        }


        [System.Obsolete("If you using SQL server 2016 + it is best to migrate to GetSessionContext")]
        protected virtual string? GetConnectionContext()
        {
            return null;
        }

        protected virtual IList<SqlServerSessionContext>? GetSessionContext()
        {
            return null;
        }


        protected virtual string? GetConnectionToken()
        {
            if (ConnectionTokenResolver != null)
            {
                return ConnectionTokenResolver.GetConnectionToken(ConnectionString);
            }
            return null;
        }

     
        private void SetCommandContext(SqlConnection sqlConnection)
        {
#pragma warning disable CS0618
            var context = GetConnectionContext();
#pragma warning restore CS0618
            if (!string.IsNullOrWhiteSpace(context))
            {
                var cmd = CreateTextCommand("SET CONTEXT_INFO @AppContext");
                cmd.Parameters.Add(Encoding.ASCII.GetBytes(context).ToSqlParameter("@AppContext"));
                cmd.Connection = sqlConnection;
                cmd.ExecuteNonQuery();
            }
        }

        private void SetSessionContext(SqlConnection sqlConnection)
        {
            var context = GetSessionContext();
            if (context != null && context.Count > 0)
            {
                var i = 0;
                var builder = new BatchSqlBuilder();
                foreach (var contextItem in context)
                {
                    builder.AddSqlFragment("EXEC sp_set_session_context ");
                    builder.AddIndexedParameter((x) => contextItem.Key.ToSqlParameter(x), "@Key", i);
                    builder.AddSqlComma();
                    builder.AddIndexedParameter((x) => contextItem.ValueAsSqlParameter(x), "@Value", i);
                    builder.AddSqlComma();
                    builder.AddIndexedParameter((x) => contextItem.ReadOnly.ToSqlParameter(x), "@ReadOnly", i);
                    builder.AddLine(";");
                    i++;
                }
                var cmd = CreateDbCommand(builder.GetSqlStatement()).WithParameters(builder.GetParameters());
                cmd.Connection = sqlConnection;
                cmd.ExecuteNonQuery();
            }
        }


        private void SetAccessToken(SqlConnection sqlConnection)
        {
            var token = GetConnectionToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                sqlConnection.AccessToken = token;
            }
        }

        protected SqlConnection CreateSqlConnection()
        {
	        SqlConnection sqlConnection = new SqlConnection(ConnectionString);
	        SetAccessToken(sqlConnection);
	        sqlConnection.Open();
	        SetCommandContext(sqlConnection);
	        SetSessionContext(sqlConnection);
            return sqlConnection;
		}


        protected override bool PrepCommand(IDbCommand cmd)
        {
            // we respect and connections pre set. if it is set and in a open state
#pragma warning disable IDE0074 // we need support for .NET standard 
            if (cmd.Connection == null || cmd.Connection.State != ConnectionState.Open)
#pragma warning restore IDE0074
            {
	            cmd.Connection = CreateSqlConnection();
	            return true;
			}
            else
            {
                // if we are using a existing valid connection then leave the connection state 
	            return false;
            }

        }

        protected virtual List<int> TransientErrorNumbers()
        {
            // https://docs.microsoft.com/en-us/azure/azure-sql/database/troubleshoot-common-errors-issues
            // Add serverless paused and warming up errors to transient error lists //42108, 42109
            return new List<int> { 4060, 42108, 42109, 40197, 40501, 40613, 49918, 49919, 49920, 4221, 11001,  };
        }

        protected override bool IsTransientError(DbException dbEx)
        {
            return dbEx is SqlException sqlexception && TransientErrorNumbers().Contains(sqlexception.Number);
        }

        protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            return (new SqlDataAdapter((SqlCommand)cmd));
        }

        #region StoredProcedureHelpers

        protected SqlCommand CreateStoredProcedureCommand(string strStoredProcedureName)
        {
            return  new SqlCommand
            {
                CommandText = strStoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };
        }
        

        #endregion StoredProcedureHelpers

        #region TextCommandType Helpers

        protected override DbCommand CreateDbCommand(string sql, CommandType cmdType = CommandType.Text)
        {
			return new SqlCommand
			{
				CommandText = sql,
				CommandType = cmdType
			};
		}


		// unless you using the SqlCommand it is best to use CreateSqlCommand
		protected SqlCommand CreateTextCommand(string sql)
        {
           return new SqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
        }

        [System.Obsolete("Use the Sql builder", true)]
        protected SqlCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }

        #endregion 
    }
}
