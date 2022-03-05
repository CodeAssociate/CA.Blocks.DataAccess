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


        [System.Obsolete(" If you using SQL server 2016 + it is best to migrate to GetSessionContext")]
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
            string context = GetConnectionContext();
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
                    cmd.Parameters.Add(contextItem.Key.ToSqlParameter("@Key", SpecificSQLStringType.NVarChar));
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
            return new List<int> { 4060, 40197, 40501, 40613, 49918, 49919, 49920, 4221, 11001 };
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
            SqlCommand sqlcmd = new SqlCommand
            {
                CommandText = strStoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };
            return (sqlcmd);
        }
        

        #endregion StoredProcedureHelpers

        #region TextCommandType Helpers
        protected SqlCommand CreateTextCommand(string sql)
        {
            SqlCommand sqlcmd = new SqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
            return (sqlcmd);
        }

        protected SqlCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }

        protected SqlCommand CreateTableSelectCommand(string tableName, string filter)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter}");
        }

        protected SqlCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand($"SELECT * FROM {tableName} {filter} Order By {orderBy}");
        }

        #endregion StoredProcedureHelpers

        #region ParemeterHelpers

        /*
        [System.Obsolete("Please use the ToSqlParameter Extension Methods", true)]
        protected SqlParameter AddInputParamCommand(SqlCommand cmd, string strParameterName, object objParameterValue, DbType odbType, int maxParamSize)
        {
            SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Input;

            if (maxParamSize > 0) sqlparam.Size = maxParamSize;

            if ((objParameterValue == null || objParameterValue == DBNull.Value))
            {
                sqlparam.Value = DBNull.Value;
                //Added the following as sometimes the type is changed to int32
                sqlparam.DbType = odbType;
            }
            else
                sqlparam.Value = objParameterValue;

            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method", true)]
        protected SqlParameter AddInputParamCommand(SqlCommand cmd, string strParameterName, object objParameterValue, SqlDbType odbType, int maxParamSize)
        {
            SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Input;

            if (maxParamSize > 0) sqlparam.Size = maxParamSize;

            if ((objParameterValue == null || objParameterValue == DBNull.Value))
            {
                sqlparam.Value = DBNull.Value;
                //Added the following as sometimes the type is changed to int32
                sqlparam.SqlDbType = odbType;
            }
            else
                sqlparam.Value = objParameterValue;

            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }

        

        protected SqlParameter AddOutputParamCommand(SqlCommand cmd, string strParameterName, DbType odbType, Int32 maxParamSize)
        {
            SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Output;
            if (maxParamSize > 0)
                sqlparam.Size = maxParamSize;
            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }

        protected SqlParameter AddOutputParamCommand(SqlCommand cmd, string strParameterName, SqlDbType odbType, Int32 maxParamSize)
        {
            SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Output;
            if (maxParamSize > 0)
                sqlparam.Size = maxParamSize;
            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }


        protected SqlParameter AddAdapterInputParamCommand(SqlCommand cmd, string strParameterName, string sourceColName, DataTable sourceDataTable)
        {
            SqlParameter sqlparam;

            if (sourceDataTable.Columns.Contains(sourceColName))
            {
                DataColumn dc = sourceDataTable.Columns[sourceColName];
                sqlparam = new SqlParameter(strParameterName, dc.DataType);
                sqlparam.Direction = ParameterDirection.Input;

                sqlparam.SourceColumn = sourceColName;

                sqlparam.SourceVersion = DataRowVersion.Current;

                cmd.Parameters.Add(sqlparam);
            }
            else
            {
                throw new Exception(string.Format("SourceColName {0} does not exist in the SourceDataTable as such cannot be added as a parameter!", sourceColName));
            }
            return (sqlparam);
        }

        protected SqlParameter AddAdapterInputParamCommand(SqlCommand cmd, string strParameterName, DataTable sourceDataTable)
        {
            return
                AddAdapterInputParamCommand(cmd, strParameterName, strParameterName.Replace("@", string.Empty),
                                            sourceDataTable);
        }
        */

        #endregion ParemeterHelpers

        #region 

        // With SQL 2012 we can use syntax OFFSET x ROWS FETCH NEXT y ROWS ONLY.. but this will only work with 2012. for now leave as is. 
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
