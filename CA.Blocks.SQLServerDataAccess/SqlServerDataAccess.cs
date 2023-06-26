//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2020 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Model.Paging;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
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

        public SqlServerDataAccess(IDataAccessConfig config, IDbRowTranslatorProvider dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
        {

        }


        [System.Obsolete("If you using SQL server 2016 + it is best to migrate to GetSessionContext")]
        protected virtual string GetConnectionContext()
        {
            return null;
        }

        protected virtual IList<SqlServerSessionContext> GetSessionContext()
        {
            return null;
        }


        protected virtual string GetConnectionToken()
        {
            return null;
        }

     
        private void SetCommandContext(SqlConnection sqlConnection)
        {
#pragma warning disable CS0618
            string context = GetConnectionContext();
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
                foreach (var contextItem in context)
                {
                    var cmd = CreateTextCommand("EXEC sp_set_session_context @key, @value, @read_only;");
                    cmd.Parameters.Add(contextItem.Key.ToSqlParameter("@Key"));
                    cmd.Parameters.Add(contextItem.ValueAsSqlParameter("@value"));
                    cmd.Parameters.Add(contextItem.ReadOnly.ToSqlParameter("@read_only"));
                    cmd.Connection = sqlConnection;
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void SetAccessToken(SqlConnection sqlConnection)
        {
            string token = GetConnectionToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                sqlConnection.AccessToken = token;
            }
        }

        protected override bool PrepCommand(IDbCommand cmd)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SetAccessToken(sqlConnection);
            sqlConnection.Open();
            SetCommandContext(sqlConnection);
            SetSessionContext(sqlConnection);
            cmd.Connection = sqlConnection;
            return true;
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
        protected SqlCommand CreateTextCommand(string sql)
        {
           return new SqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
        }

        protected SqlCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }


        [System.Obsolete("This will be removed as to many examples of not using parameterised queries ")]
        protected SqlCommand CreateTableSelectCommand(string tableName, string filter)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter}");
        }

        [System.Obsolete("This will be removed as to many examples of not using parameterised queries ")]
        protected SqlCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter} Order By {orderBy}");
        }


        // With SQL 2012 we can use syntax OFFSET x ROWS FETCH NEXT y ROWS ONLY.. but this will only work with 2012. for now leave as is. 
        [System.Obsolete("If you using SQL server 2012 +  you can now use the simple OFFSET X ROWS FETCH NEXT Y ROWS this is is for legacy databases ")]
        protected DataTable ExecuteDataTable(SqlCommand cmd, PagingRequest page)
        {
            // this is sql server specific and only for direct queries

            string sortOrder = page.GetOrderBy();
            string sqlSelect = $" ROW_NUMBER() Over (Order By {sortOrder}) As RowNumber, ";
            cmd.CommandText = WrapPagingQuery(cmd.CommandText, sqlSelect);
            cmd.Parameters.Add((page.Skip + 1).ToSqlParameter("@PagingRowNumberFrom"));
            cmd.Parameters.Add((page.Skip + page.Take).ToSqlParameter("@PagingRowNumberTo"));
            return ExecuteDataTable(cmd);
        }

        protected string WrapPagingQuery(string sourceQuery, string orderOver)
        {
            sourceQuery = sourceQuery.Trim();
            if (sourceQuery.StartsWith("Select", StringComparison.CurrentCultureIgnoreCase))
            {
                sourceQuery = "Select " + orderOver + sourceQuery.Substring(6);
                string pagingWrapperSQL = @"With PagingWrapper As 
                            (
                              {0}
                            ) 
                        Select PagingWrapper.*
                        from PagingWrapper
                        Where PagingWrapper.RowNumber Between @PagingRowNumberFrom AND @PagingRowNumberTo
                        Order By PagingWrapper.RowNumber Asc";

                return string.Format(pagingWrapperSQL, sourceQuery);
            }
            else
            {
                throw new ApplicationException("To Execute ExecuteDataTable using a PagingRequest the Command must be text query and start with 'Select'   ");
            }
        }

        #endregion 
    }
}
