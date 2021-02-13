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
            MySqlConnection sqlConnection = new MySqlConnection(ConnectionString);
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
            MySqlCommand sqlcmd = new MySqlCommand
            {
                CommandText = strStoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };
            return (sqlcmd);
        }


        #region TextCommandType Helpers
        protected MySqlCommand CreateTextCommand(string sql)
        {
            MySqlCommand sqlcmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text
            };
            return (sqlcmd);
        }

        protected MySqlCommand CreateTextCommand(string sqlTemplate, string mainFilter)
        {
            var sql = sqlTemplate.Replace(FILTER_REPLACE_STRING, mainFilter);
            return CreateTextCommand(sql);
        }

        protected MySqlCommand CreateTableSelectCommand(string tableName, string filter)
        {
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1}", tableName, filter));
        }

        protected MySqlCommand CreateTableSelectCommand(string tableName, string filter, string orderBy)
        {
            return CreateTextCommand(string.Format("SELECT * FROM {0} {1} Order By {2}", tableName, filter, orderBy));
        }

        #endregion StoredProcedureHelpers

        


        //protected SqlParameter AddOutputParamCommand(SqlCommand cmd, string strParameterName, DbType odbType, Int32 maxParamSize)
        //{
        //    SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
        //    sqlparam.Direction = ParameterDirection.Output;
        //    if (maxParamSize > 0)
        //        sqlparam.Size = maxParamSize;
        //    cmd.Parameters.Add(sqlparam);

        //    return (sqlparam);
        //}

        //protected SqlParameter AddOutputParamCommand(SqlCommand cmd, string strParameterName, SqlDbType odbType, Int32 maxParamSize)
        //{
        //    SqlParameter sqlparam = new SqlParameter(strParameterName, odbType);
        //    sqlparam.Direction = ParameterDirection.Output;
        //    if (maxParamSize > 0)
        //        sqlparam.Size = maxParamSize;
        //    cmd.Parameters.Add(sqlparam);

        //    return (sqlparam);
        //}


        //protected SqlParameter AddAdapterInputParamCommand(SqlCommand cmd, string strParameterName, string sourceColName, DataTable sourceDataTable)
        //{
        //    SqlParameter sqlparam;

        //    if (sourceDataTable.Columns.Contains(sourceColName))
        //    {
        //        DataColumn dc = sourceDataTable.Columns[sourceColName];
        //        sqlparam = new SqlParameter(strParameterName, dc.DataType);
        //        sqlparam.Direction = ParameterDirection.Input;

        //        sqlparam.SourceColumn = sourceColName;

        //        sqlparam.SourceVersion = DataRowVersion.Current;

        //        cmd.Parameters.Add(sqlparam);
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format("SourceColName {0} does not exist in the SourceDataTable as such cannot be added as a parameter!", sourceColName));
        //    }
        //    return (sqlparam);
        //}

        //protected SqlParameter AddAdapterInputParamCommand(SqlCommand cmd, string strParameterName, DataTable sourceDataTable)
        //{
        //    return
        //        AddAdapterInputParamCommand(cmd, strParameterName, strParameterName.Replace("@", string.Empty),
        //                                    sourceDataTable);
        //}

        //#endregion ParemeterHelpers 

        //#region SQLType Helpers

        ///// <summary>
        ///// This is usefull when you dont know the sql datatype but you do know the physical type example is datatable
        ///// DataColumn dc = ??
        /////  AddInputParamCommand(cmd, dc.ColumnName, dr[dc], GetDBType(dc.DataType), dc.MaxLength);
        ///// </summary>
        ///// <param name="theType"></param>
        ///// <returns></returns>
        //protected SqlDbType GetDBType(Type theType)
        //{
        //    SqlParameter p1 = new SqlParameter();
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

        //#endregion

        //#region SQL Bulk Update Methods

        //protected SqlDataAdapter CreateBulkInsertAdapter(string storedProcedureName, int batchSize)
        //{
        //    SqlDataAdapter result = new SqlDataAdapter();
        //    result.UpdateBatchSize = batchSize;
        //    SqlCommand cmd = CreateStoredProcedureCommand(storedProcedureName);
        //    cmd.UpdatedRowSource = UpdateRowSource.None;
        //    result.InsertCommand = cmd;
        //    return result;
        //}

        //// gets the first col which has an expression on.  
        //// This will need to be refactored if you have expressions based on expressions as you will need to be aware of dependency order
        //// if no expressions are found it will return null. 
        //private DataColumn GetColunmWithExpression(DataTable dt)
        //{
        //    DataColumn result = null;
        //    foreach (DataColumn dcloop in dt.Columns)
        //    {
        //        if (!string.IsNullOrEmpty(dcloop.Expression))
        //        {
        //            result = dcloop;
        //            break;
        //        }
        //    }
        //    return result;
        //}

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
        //#endregion SQL Bulk Update Methods

        #region 

        protected DataTable ExecuteDataTable(MySqlCommand cmd, PagingRequest page)
        {
            // this is sql server specific and only for direct queries

            string sortOrder = page.GetOrderBy();
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
                return $"({sourceQuery.Replace(";", String.Empty)}) Order By table_name LIMIT @take OFFSET @skip;";
            }
            else
            {
                throw new ApplicationException("To Execute ExecuteDataTable using a PagingRequest the Command must be text query and start with 'Select'   ");
            }
        }

        #endregion 
    }
}