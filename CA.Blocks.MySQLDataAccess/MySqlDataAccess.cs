//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2021 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Model.Paging;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using MySqlConnector;

namespace CA.Blocks.MySQLDataAccess
{
    /// <summary>
    /// Provides a MySql implementation for DataAccessCore
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class MySqlDataAccess : DataAccessCore
    {

        public const string FILTER_REPLACE_STRING = "/*##FILTER##*/";

        public MySqlDataAccess(IDataAccessConfig config, IDbRowTranslatorProvider dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
        {

        }


        protected virtual string GetConnectionContext()
        {
            return null;
        }


        protected override bool PrepCommand(IDbCommand cmd)
        {
            var sqlConnection = new MySqlConnection(ConnectionString);
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
            return true;
        }

        protected override bool IsTransientError(DbException dbEx)
        {
            return false; 
        }

        protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            
            return new MySqlDataAdapter((MySqlCommand)cmd);
        }

      

        protected MySqlCommand CreateStoredProcedureCommand(string strStoredProcedureName)
        {
            return  new MySqlCommand
            {
                CommandText = strStoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };
        }

        protected override DbCommand CreateSqlCommand(string sql, CommandType cmdType = CommandType.Text)
        {
			return new MySqlCommand
			{
				CommandText = sql,
				CommandType = cmdType
			};
		}


		#region TextCommandType Helpers
		protected MySqlCommand CreateTextCommand(string sql)
        {
            return new MySqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
        }

        protected MySqlCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }

        protected MySqlCommand CreateTableSelectCommand(string tableName, string filter)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter}");
        }

        protected MySqlCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter} Order By {orderBy}");
        }

        #endregion StoredProcedureHelpers


        #region 

        protected DataTable ExecuteDataTable(MySqlCommand cmd, PagingRequest page)
        {
            // this is sql server specific and only for direct queries

            var sortOrder = page.GetOrderBy();
            cmd.CommandText = WrapPagingQuery(cmd.CommandText, sortOrder);
            cmd.Parameters.Add((page.Skip).ToSqlParameter("@skip"));
            cmd.Parameters.Add((page.Take).ToSqlParameter("@take"));
            return ExecuteDataTable(cmd);
        }


        protected string WrapPagingQuery(string sourceQuery, string orderOver)
        {
            sourceQuery = sourceQuery.Trim();
            if (sourceQuery.StartsWith("Select", StringComparison.CurrentCultureIgnoreCase))
            {
                return $"({sourceQuery.Replace(";", string.Empty)}) Order By table_name LIMIT @take OFFSET @skip;";
            }
            else
            {
                throw new ApplicationException("To Execute ExecuteDataTable using a PagingRequest the Command must be text query and start with 'Select'   ");
            }
        }

        #endregion 
    }
}