//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2019 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLLiteDataAccess.Adapters;
using CA.Blocks.DataAccess.Model.Paging;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SQLLiteDataAccess
{
    /// <summary>
    /// Provides a SQL Lite implementation for DataAccessCore
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SqlLiteDataAccess : DataAccessCore
    {

        public const string FILTER_REPLACE_STRING = "/*##FILTER##*/";

        public SqlLiteDataAccess(IDataAccessConfig config, IDbRowTranslatorProvider dbRowTranslatorProvider) : base(config, dbRowTranslatorProvider)
        {

        }

        protected virtual string GetConnectionContext()
        {
            return null;
        }

        private void SetCommandContext(SqliteConnection sqlConnection)
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

        protected override bool PrepCommand(IDbCommand cmd)
        {
            SqliteConnection sqlConnection = new SqliteConnection(ConnectionString);
            sqlConnection.Open();
            SetCommandContext(sqlConnection);
            cmd.Connection = sqlConnection;
            return true;
        }

        protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            return new SqliteDataAdapter((SqliteCommand)cmd);
        }

        #region StoredProcedureHelpers

        protected SqliteCommand CreateBlankStoredProcedureCommand(string strStoredProcedureName, bool bolIncludeReturnValue = false)
        {
            SqliteCommand sqlcmd = new SqliteCommand
            {
                CommandText = strStoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };
            if (bolIncludeReturnValue)
            {
                var sqlparam = sqlcmd.CreateParameter();
                sqlparam.ParameterName = "Return";
                sqlparam.SqliteType = SqliteType.Integer;
                sqlparam.Direction = ParameterDirection.ReturnValue;
                sqlcmd.Parameters.Add(sqlparam);
            }
            return (sqlcmd);
        }


        protected int GetStoredProcedureReturnValue(SqliteCommand sqlcmd)
        {
            int result = -1;
            var sqlparam = sqlcmd.Parameters["Return"];
            if (sqlparam != null)
            {
                if (sqlparam.Value != null)
                    result = (int)sqlparam.Value;
            }
            return result;
        }


        #endregion StoredProcedureHelpers

        #region TextCommandType Helpers
        protected SqliteCommand CreateTextCommand(string sql)
        {
            SqliteCommand sqlcmd = new SqliteCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
            return (sqlcmd);
        }

        protected SqliteCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }

        protected SqliteCommand CreateTableSelectCommand(string tableName, string filter)
        {
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1}", tableName, filter));
        }

        protected SqliteCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1} Order By {2}", tableName, filter, orderBy));
        }

        #endregion StoredProcedureHelpers

       
        #region SQL Bulk Update Methods

        //protected SqlDataAdapter CreateBulkInsertAdapter(string storedProcedureName, int batchSize)
        //{
        //    SqlDataAdapter result = new SqlDataAdapter();
        //    result.UpdateBatchSize = batchSize;
        //    SqlCommand cmd = CreateBlankStoredProcedureCommand(storedProcedureName, false);
        //    cmd.UpdatedRowSource = UpdateRowSource.None;
        //    result.InsertCommand = cmd;
        //    return result;
        //}

        // gets the first col which has an expression on.  
        // This will need to be refactored if you have expressions based on expressions as you will need to be aware of dependency order
        // if no expressions are found it will return null. 
        private DataColumn GetColunmWithExpression(DataTable dt)
        {
            DataColumn result = null;
            foreach (DataColumn dcloop in dt.Columns)
            {
                if (!string.IsNullOrEmpty(dcloop.Expression))
                {
                    result = dcloop;
                    break;
                }
            }
            return result;
        }

        protected void CementExpressionsAsValues(DataTable dt)
        {
            DataColumn colWithExpression = GetColunmWithExpression(dt);
            int excapeCounter = 0;
            while (colWithExpression != null && excapeCounter < dt.Columns.Count)
            {

                string tempColName = colWithExpression.ColumnName + Guid.NewGuid().ToString();
                dt.Columns.Add(tempColName, colWithExpression.DataType);

                foreach (DataRow dr in dt.Rows)
                {
                    dr[tempColName] = dr[colWithExpression.ColumnName];
                }
                dt.Columns.Remove(colWithExpression);
                dt.Columns[tempColName].ColumnName = colWithExpression.ColumnName;

                colWithExpression = GetColunmWithExpression(dt);
                excapeCounter++;
            }
        }

        //protected void ExecuteBulkInsertAdapter(SqlDataAdapter bulkAdapter, DataTable dt)
        //{
        //    try
        //    {
        //        PrepCommand(bulkAdapter.InsertCommand);
        //        // possibly move this function out as it nos not really belong here 
        //        CementExpressionsAsValues(dt);
        //        bulkAdapter.Update(dt);
        //    }
        //    finally
        //    {
        //        WrapUp(bulkAdapter.InsertCommand.Connection, true);
        //    }
        //}
        #endregion SQL Bulk Update Methods

        #region 

        // With SQL 2012 we can use syntax OFFSET x ROWS FETCH NEXT y ROWS ONLY.. but this will only work with 2012. for now leave as is. 
        public DataTable ExecuteDataTable(SqliteCommand cmd, PagingRequest page)
        {
            // this is sql server specific and only for direct quries

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
