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

        public SqlLiteDataAccess(IDataAccessConfig config) : base(config)
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

        #region ParemeterHelpers

        //protected SqliteParameter AddInputParamCommand(SqliteCommand cmd, string strParameterName, object objParameterValue, DbType odbType, int maxParamSize)
        //{
        //    var sqlparam = new SqliteParameter(strParameterName, odbType);
        //    sqlparam.Direction = ParameterDirection.Input;

        //    if (maxParamSize > 0) sqlparam.Size = maxParamSize;

        //    if ((objParameterValue == null || objParameterValue == DBNull.Value))
        //    {
        //        sqlparam.Value = DBNull.Value;
        //        //Added the following as sometimes the type is changed to int32
        //        sqlparam.DbType = odbType;
        //    }
        //    else
        //        sqlparam.Value = objParameterValue;

        //    cmd.Parameters.Add(sqlparam);

        //    return (sqlparam);
        //}

        //protected SqliteParameter AddInputParamCommand(SqliteCommand cmd, string strParameterName, object objParameterValue, SqlDbType odbType, int maxParamSize)
        //{
        //    SqliteParameter sqlparam = new SqliteParameter(strParameterName, odbType);
        //    sqlparam.Direction = ParameterDirection.Input;

        //    if (maxParamSize > 0) sqlparam.Size = maxParamSize;

        //    if ((objParameterValue == null || objParameterValue == DBNull.Value))
        //    {
        //        sqlparam.Value = DBNull.Value;
        //        //Added the following as sometimes the type is changed to int32
        //        sqlparam.SqlDbType = odbType;
        //    }
        //    else
        //        sqlparam.Value = objParameterValue;

        //    cmd.Parameters.Add(sqlparam);

        //    return (sqlparam);
        //}

        /*

        //protected SqlParameter AddInputParamCommandAsDeciaml(SqlCommand cmd, string strParameterName, decimal? objParameterValue)
        //{
        //    return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Decimal, 38);
        //}

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsGuid(SqliteCommand cmd, string strParameterName, Guid? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.UniqueIdentifier, 16);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsByte(SqliteCommand cmd, string strParameterName, byte? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.TinyInt, 1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsBinary(SqliteCommand cmd, string strParameterName, byte[] objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.VarBinary, -1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsBit(SqliteCommand cmd, string strParameterName, bool objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Bit, 1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsString(SqliteCommand cmd, string strParameterName, string objParameterValue, int maxSize)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.VarChar, maxSize);
        }

        // dont care about the size of the object this assumes the check as been done already else SQL will raise an error
        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsString(SqliteCommand cmd, string strParameterName, string objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.VarChar, -1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsBool(SqliteCommand cmd, string strParameterName, bool? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Bit, 1);
        }
        [System.Obsolete("Do Conversion to Y ? N Outside then use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsCharBool(SqliteCommand cmd, string strParameterName, bool objParameterValue)
        {
            char DBValue = objParameterValue ? 'Y' : 'N';
            return AddInputParamCommand(cmd, strParameterName, DBValue, SqlDbType.Char, 1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsChar(SqliteCommand cmd, string strParameterName, char? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Char, 1);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsDateTime(SqliteCommand cmd, string strParameterName, DateTime? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.DateTime, 8);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsDateTime2(SqliteCommand cmd, string strParameterName, DateTime? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.DateTime2, 8);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsSmallDateTime(SqliteCommand cmd, string strParameterName, DateTime? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.SmallDateTime, 4);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsMoney(SqliteCommand cmd, string strParameterName, Decimal? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Money, 0);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsDecimal(SqliteCommand cmd, string strParameterName, Decimal? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Decimal, 0);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsFloat(SqliteCommand cmd, string strParameterName, Double? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Float, 0);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsDouble(SqliteCommand cmd, string strParameterName, Double? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Float, 0);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsTimeSpan(SqliteCommand cmd, string strParameterName, TimeSpan? objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.Time, 0);
        }

        [System.Obsolete("Please use the ToSqlParameter Extension Method")]
        protected SqliteParameter AddInputParamCommandAsStringMax(SqliteCommand cmd, string strParameterName, string objParameterValue)
        {
            return AddInputParamCommand(cmd, strParameterName, objParameterValue, SqlDbType.VarChar, int.MaxValue);
        }

        protected SqliteParameter AddOutputParamCommand(SqliteCommand cmd, string strParameterName, DbType odbType, Int32 maxParamSize)
        {
            SqliteParameter sqlparam = new SqliteParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Output;
            if (maxParamSize > 0)
                sqlparam.Size = maxParamSize;
            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }

        protected SqliteParameter AddOutputParamCommand(SqliteCommand cmd, string strParameterName, SqlDbType odbType, Int32 maxParamSize)
        {
            SqliteParameter sqlparam = new SqliteParameter(strParameterName, odbType);
            sqlparam.Direction = ParameterDirection.Output;
            if (maxParamSize > 0)
                sqlparam.Size = maxParamSize;
            cmd.Parameters.Add(sqlparam);

            return (sqlparam);
        }


        protected SqliteParameter AddAdapterInputParamCommand(SqliteCommand cmd, string strParameterName, string sourceColName, DataTable sourceDataTable)
        {
            SqliteParameter sqlparam;

            if (sourceDataTable.Columns.Contains(sourceColName))
            {
                DataColumn dc = sourceDataTable.Columns[sourceColName];
                sqlparam = new SqliteParameter(strParameterName, dc.DataType);
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

        protected SqliteParameter AddAdapterInputParamCommand(SqliteCommand cmd, string strParameterName, DataTable sourceDataTable)
        {
            return
                AddAdapterInputParamCommand(cmd, strParameterName, strParameterName.Replace("@", string.Empty),
                                            sourceDataTable);
        }
        */
        #endregion ParemeterHelpers 

        #region SQLType Helpers

        /// <summary>
        /// This is usefull when you dont know the sql datatype but you do know the physical type example is datatable
        /// DataColumn dc = ??
        ///  AddInputParamCommand(cmd, dc.ColumnName, dr[dc], GetDBType(dc.DataType), dc.MaxLength);
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        //protected SqlDbType GetDBType(Type theType)
        //{
        //    SqliteParameter p1 = new SqliteParameter();
        //    TypeConverter tc = TypeDescriptor.GetConverter(p1.DbType);
        //    if (tc.CanConvertFrom(theType))
        //    {
        //        tc.ConvertFrom(theType.Name);
        //        p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
        //    }
        //    else
        //    {
        //        //Try brute force
        //        try
        //        {
        //            p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
        //        }
        //        catch
        //        {
        //            //Do Nothing
        //        }
        //    }
        //    return p1.SqlDbType;
        //}

        #endregion

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
