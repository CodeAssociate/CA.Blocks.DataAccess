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
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Model.Paging;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;

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

        protected virtual string GetConnectionContext()
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

        protected override bool PrepCommand(IDbCommand cmd)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SetCommandContext(sqlConnection);
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
            return dbEx != null && TransientErrorNumbers().Contains(dbEx.ErrorCode);
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

        [System.Obsolete("Use CreateStoredProcedureCommand", true)]
        protected SqlCommand CreateBlankStoredProcedureCommand(string strStoredProcedureName)
        {
            return CreateStoredProcedureCommand(strStoredProcedureName);
        }


        [Obsolete("Replaced with the  sqlcmd.WithReturnParameter", true)]
        protected int GetStoredProcedureReturnValue(SqlCommand sqlcmd)
        {
            int result = -1;
            SqlParameter sqlparam = sqlcmd.Parameters["Return"];
            if (sqlparam != null)
            {
                if (sqlparam.Value != null)
                    result = (int)sqlparam.Value;
            }
            return result;
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
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1}", tableName, filter));
        }

        protected SqlCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1} Order By {2}", tableName, filter, orderBy));
        }

        #endregion StoredProcedureHelpers

        #region ParemeterHelpers

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



        /// <summary>
        /// To be removed
        /// </summary>
        /// <param name="cmd">cmd params</param>
        /// <param name="strParameterName"> name</param>
        /// <param name="objParameterValue"> value</param>
        /// <returns></returns>
        [System.Obsolete("Please use the ToSqlParameter Extension Method", true)]
        protected SqlParameter AddInputParamCommandAsByte(SqlCommand cmd, string strParameterName, byte? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.TinyInt, 1);
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

        #endregion ParemeterHelpers 

        #region SQLType Helpers

        /// <summary>
        /// This is usefull when you dont know the sql datatype but you do know the physical type example is datatable
        /// DataColumn dc = ??
        ///  AddInputParamCommand(cmd, dc.ColumnName, dr[dc], GetDBType(dc.DataType), dc.MaxLength);
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        protected SqlDbType GetDBType(Type theType)
        {
            SqlParameter p1 = new SqlParameter();
            TypeConverter tc = TypeDescriptor.GetConverter(p1.DbType);
            if (tc.CanConvertFrom(theType))
            {
                tc.ConvertFrom(theType.Name);
                p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
            }
            else
            {
                //Try brute force
                try
                {
                    p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
                }
                catch
                {
                    //Do Nothing
                }
            }
            return p1.SqlDbType;
        }

        #endregion

        #region SQL Bulk Update Methods

        protected SqlDataAdapter CreateBulkInsertAdapter(string storedProcedureName, int batchSize)
        {
            SqlDataAdapter result = new SqlDataAdapter();
            result.UpdateBatchSize = batchSize;
            SqlCommand cmd = CreateStoredProcedureCommand(storedProcedureName);
            cmd.UpdatedRowSource = UpdateRowSource.None;
            result.InsertCommand = cmd;
            return result;
        }

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

        protected void ExecuteBulkInsertAdapter(SqlDataAdapter bulkAdapter, DataTable dt)
        {
            try
            {
                PrepCommand(bulkAdapter.InsertCommand);
                // possibly move this function out as it nos not really belong here 
                CementExpressionsAsValues(dt);
                bulkAdapter.Update(dt);
            }
            finally
            {
                WrapUp(bulkAdapter.InsertCommand.Connection, true);
            }
        }
        #endregion SQL Bulk Update Methods

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
