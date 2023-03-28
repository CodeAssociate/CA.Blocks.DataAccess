//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2012 Ravin Enterprises Ltd. 
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
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.SqliteDataAccess.Adapters;
using Microsoft.Data.Sqlite;


namespace CA.Blocks.SqliteDataAccess
{
    /// <summary>
    /// Provides a SQL Lite implementation for DataAccessCore
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SqliteDataAccess : DataAccessCore, IDisposable
    {

        public const string FILTER_REPLACE_STRING = "/*##FILTER##*/";

        private readonly SqliteConnection _dbConnection;


        public SqliteDataAccess(IDataAccessConfig config, IDbRowTranslatorProvider dbRowTranslatorProvider = null) : base(config, dbRowTranslatorProvider)
        {
            _dbConnection = new SqliteConnection(ConnectionString);

        }
        // with Sqlite we do not use connection pooling we keep a connection with in the class.
        protected override CommandBehavior DefaultCommandBehavior => CommandBehavior.Default;

        public void Dispose()
        {
            if (_dbConnection != null
                && (_dbConnection.State == ConnectionState.Open
                    || _dbConnection.State == ConnectionState.Executing
                    || _dbConnection.State == ConnectionState.Fetching)
               )
            {
                _dbConnection.Close();
            }
        }


        protected virtual string GetConnectionContext()
        {
            return null;
        }

        private void SetCommandContext(SqliteConnection sqlConnection)
        {
          
        }

        public void BeginTransaction()
        {
            var cmd = CreateTextCommand("begin");
            ExecuteNonQuery(cmd);
        }

        public void CommitTransaction()
        {
            var cmd = CreateTextCommand("commit");
            ExecuteNonQuery(cmd);
        }

        public void RollBackTransaction()
        {
            var cmd = CreateTextCommand("rollback");
            ExecuteNonQuery(cmd);
        }

        protected override bool PrepCommand(IDbCommand cmd)
        {
            if (_dbConnection == null)
            {
                SqliteConnection sqlConnection = new SqliteConnection(ConnectionString);
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                return true;
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

        protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            return new SqliteDataAdapter((SqliteCommand)cmd);
        }


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
        #endregion
    }
}
