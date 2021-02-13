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

using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace CA.Blocks.DataAccess
{
    /// <summary>
    /// <para>
    /// This class provides the abstract implementation for the Code Associate Data Access Block.  
    /// The Abstract implementation is build upon utilizing common System.Data methods and interfacing out the Specific 
    /// DBCommand using the IDbCommand interface.  In doing this all specializations built on top of this class will behave 
    /// in the same manor. This class is abstract and cannot be created. 
    /// </para>
    /// </summary>
    public abstract class DataAccessCore
    {
        private readonly IDataAccessConfigOptions _options;
        private readonly IDbRowTranslatorProvider _dbRowTranslatorProvider;

        private const int TotalNumberOfTimesToTry = 4;
        private const int RetryIntervalSeconds = 10;

        protected string ConnectionString { get; }

        #region private utility methods & constructors



        /// <summary>
        /// This is a protected constructor which must be called by the inheriting class, it will use config.Resolver to resolve the connectionStringKey to a valid connection string 
        /// </summary>
        protected DataAccessCore(IDataAccessConfig config, IDbRowTranslatorProvider dbRowTranslatorProvider)
        {
            _dbRowTranslatorProvider = dbRowTranslatorProvider ?? DefaultDbRowTranslatorProvider.DefaultInstance;
            _options = config.Options;
            ConnectionString = config.Resolver.GetConnectionString(_options.ConnectionStringKey);
        }

        /// <summary>
        /// The WrapUp procedure is called when completing a database call. It will establish 
        /// whether or not to close the connection pending the variable closeConnection which 
        /// would have been passed back from the PrepCommand. The PrepCommand and WrapUp work 
        /// in tandem when executing commands though this common class. 
        /// </summary>
        /// <param name="conn">the connection to close pending closeConnection</param>
        /// <param name="closeConnection"> determines if conn should be closed on complete</param>
        protected void WrapUp(IDbConnection conn, bool closeConnection)
        {
            if (closeConnection)
            {
                if (conn != null
                    && (conn.State == ConnectionState.Open
                    || conn.State == ConnectionState.Executing
                    || conn.State == ConnectionState.Fetching)
                    )
                {
                    conn.Close();
                }
            }
        }

        // If using sql then a SQL trace will be better, this is for data sources that do not have good tracing tools
        // you will override this method and trace to your preferred tool.  the trace happens after execute
        protected virtual void TraceDbStatement(IDbCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        }

        protected virtual void TraceDbError(IDbCommand cmd, DbException ex)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        protected virtual void TraceGenralError(IDbCommand cmd, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        #endregion private utility methods & constructors

        #region abstract methods that must me implemented


        protected abstract DbDataAdapter GetDataAdapter(IDbCommand cmd);


        /// <summary>
        /// The Prep Command is abstract method that must be implemented by the providers. The method will create the the provider specific connection setting the connection string, 
        /// Opening the connection, set any context on the connection the assign the connection to the command for execution, it will also and indicate to the blocks if the connection
        /// should be closed on complete execution. In most cases it is best to close the connection, at the provider will managed the connection pool.
        /// </summary>
        /// <param name="cmd">The command to assign the connection to</param>
        /// <returns> a bool value to indicated if the blocks should close the connection when finished. </returns>
        protected abstract bool PrepCommand(IDbCommand cmd);


        protected abstract bool IsTransientError(DbException dbEx);
        #endregion

        #region ExecuteNonQuery


        private int InternalExecuteNonQuery(IDbCommand cmd)
        {
            bool success = false;
            int rowCount = 0;
            for (int tries = 0;  tries <= TotalNumberOfTimesToTry; tries++)
            {
                bool closeConnection = PrepCommand(cmd);
                try
                {
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * RetryIntervalSeconds * tries);
                    }
                   
                    rowCount = cmd.ExecuteNonQuery();
                    success = true;
                    break;
                }
                catch (DbException dbEx)
                {
                    if (IsTransientError(dbEx))
                    {
                        if (tries < TotalNumberOfTimesToTry)
                        {
                            continue;
                        }
                        else
                        {
                            // we tried TotalNumberOfTimesToTry times already to error
                            TraceDbError(cmd, dbEx);
                            throw;
                        }
                    }
                    else
                    {
                        TraceDbError(cmd, dbEx);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TraceGenralError(cmd, ex);
                    throw;
                }
                finally
                {
                    WrapUp(cmd.Connection, closeConnection);
                }
            }

            if (success)
            {
                return rowCount;
            }
            else
            {
                throw new ApplicationException("InternalExecuteNonQuery failed to find exit path");
            }
        }

        /// <summary>
        /// Will execute a value sql query that does not return any results back to the client. This is typically Data modification statements such as insert , update or delete or catalog operations  such as creating tables, indexes etc
        /// </summary>
        /// <param name="cmd">The command to execute, used the provider instance to create the Command to be executed</param>
        /// <returns>For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. For all other types of statements, the return value is -1.</returns>
        /// <example><code>
        ///public int IncreasePriceBy10Percent()
        ///{
        ///    /// This will Increase all Prices in the product table by 10%  and return the number of rows affected
        ///    var cmd = CreateTextCommand("update products set price = price * 1.1");
        ///    return ExecuteNonQuery(cmd);
        ///}
        ///</code></example>
        /// <remarks>
        /// Although the ExecuteNonQuery returns no rows, any output parameters or return values mapped to parameters are populated with data. This can useful when executing stored procedures. 
        /// </remarks>
        protected int ExecuteNonQuery(IDbCommand cmd)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return InternalExecuteNonQuery(cmd);
        }
        #endregion ExecuteNonQuery


        #region ExecuteDataSet

        private void InternalExecuteDataSet(IDbCommand cmd, DataSet ds, string sTableNames)
        {
            bool success = false;
            for (int tries = 0; tries <= TotalNumberOfTimesToTry; tries++)
            {
                bool closeConnection = PrepCommand(cmd);
                try
                {
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * RetryIntervalSeconds * tries);
                    }

                    string[] sTableNNameArray = sTableNames.Split(',');
                    using (DbDataAdapter theDataAdapter = GetDataAdapter(cmd))
                    {
                        for (int i = 1; i < sTableNNameArray.Length; i++)
                        {
                            theDataAdapter.TableMappings.Add(sTableNNameArray[0].Trim() + Convert.ToString(i), sTableNNameArray[i].Trim());
                        }
                        theDataAdapter.Fill(ds, sTableNNameArray[0].Trim());
                    }
                    success = true;
                    break;
                }
                catch (DbException dbEx)
                {
                    if (IsTransientError(dbEx))
                    {
                        if (tries < TotalNumberOfTimesToTry)
                        {
                            continue;
                        }
                        else
                        {
                            // we tried TotalNumberOfTimesToTry times already to error
                            TraceDbError(cmd, dbEx);
                            throw;
                        }
                    }
                    else
                    {
                        TraceDbError(cmd, dbEx);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TraceGenralError(cmd, ex);
                    throw;
                }
                finally
                {
                    WrapUp(cmd.Connection, closeConnection);
                }
            }

            if (!success)
            {
                throw new ApplicationException("InternalExecuteDataSet failed to find exit path");
            }
        }
        
        /// <summary>
        /// Executes the command into a new Dataset 
        /// </summary>
        /// <param name="cmd"> A data set return, The first name name will be called Results the second will be called  Results1, third will be called Results2 etc</param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(IDbCommand cmd)
        {
            DataSet ds = new DataSet();
            return (ExecuteDataSet(cmd, ds, "Results"));
        }

        protected DataSet ExecuteDataSet(IDbCommand cmd, DataSet ds, string sTableNames)
        {
            // full ownership of the connection
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            InternalExecuteDataSet(cmd, ds, sTableNames);
            return (ds);
        }

        #endregion ExecuteDataSet

        #region ExecuteTable

        protected DataTable ExecuteDataTable(IDbCommand cmd)
        {
            DataSet ds = ExecuteDataSet(cmd);
            return (ds.Tables[0]);
        }

        #endregion ExecuteTable

        #region ExecuteDataRow

        protected DataRow ExecuteDataRow(IDbCommand cmd)
        {
            DataSet ds = ExecuteDataSet(cmd);
            DataRow dr = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                    dr = ds.Tables[0].Rows[0];
                else
                    throw new DataException("Command was asked to execute a ExecuteDataRow however more than a single data row was found, ExecuteDataRow expects one or zero rows returned");
            }
            return (dr);
        }

        #endregion ExecuteDataRow

        #region ExecuteScalar


        private object InternalExecuteScalar(IDbCommand cmd)
        {
            bool success = false;
            object result = null ;
            for (int tries = 0; tries <= TotalNumberOfTimesToTry; tries++)
            {
                bool closeConnection = PrepCommand(cmd);
                try
                {
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * RetryIntervalSeconds * tries);
                    }

                    result = cmd.ExecuteScalar();
                    success = true;
                    break;
                }
                catch (DbException dbEx)
                {
                    if (IsTransientError(dbEx))
                    {
                        if (tries < TotalNumberOfTimesToTry)
                        {
                            continue;
                        }
                        else
                        {
                            // we tried TotalNumberOfTimesToTry times already to error
                            TraceDbError(cmd, dbEx);
                            throw;
                        }
                    }
                    else
                    {
                        TraceDbError(cmd, dbEx);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TraceGenralError(cmd, ex);
                    throw;
                }
                finally
                {
                    WrapUp(cmd.Connection, closeConnection);
                }
            }

            if (success)
            {
                return result;
            }
            else
            {
                throw new ApplicationException("InternalExecuteScalar failed to find exit path");
            }
        }

        protected object ExecuteScalar(IDbCommand cmd)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return InternalExecuteScalar(cmd);
        }

        /// <summary>
        /// This will execute the cmd to a ScalarValue and cast the ScalarValue to the target Type. Use this command if you can know the data type on  the server.
        /// </summary>
        /// <typeparam name="T">This the target type to execute the result in to</typeparam>
        /// <param name="cmd">The IDbCommand cmd to execute</param>
        /// <returns></returns>
        protected T ExecuteScalarAs<T>(IDbCommand cmd) 
        {
            Object result = ExecuteScalar(cmd);
            return  (result == null || result == DBNull.Value) ? default : (T)result;
            //return (T?) result ?? default;
        }

        /// <summary>
        ///  This will execute the cmd to a ScalarValue and convert the ScalarValue to the target Type TypeConverterAttributelooking for a TypeConverterAttribute.
        /// If it cannot find a TypeConverterAttribute, it traverses the base class hierarchy of the class until it finds a primitive type.  This is useful when
        /// changing types between the server  and  the code example store as a tiny int on the server and and int in the application code. Throw error on conversion errors
        /// </summary>
        /// <typeparam name="T">This the target type to execute the result in to</typeparam>
        /// <param name="cmd">The IDbCommand cmd to execute</param>
        /// <returns></returns>
        protected T ExecuteScalarWithConvertAs<T>(IDbCommand cmd)
        {
            Object result = ExecuteScalar(cmd);
            return (result == null || result == DBNull.Value) ? default : (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(result.ToString());
        }

        /// <summary>
        /// This will execute the cmd to a ScalarValue and convert the ScalarValue to string value, There is a overload top control the value of the null passed back.
        /// </summary>
        /// <param name="cmd">The IDbCommand cmd to execute</param>
        /// <param name="nullDefault">If execute result is null this this value will be passed back. </param>
        /// <returns></returns>
        protected string ExecuteScalarAsString(IDbCommand cmd, string nullDefault = null)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? result.ToString() : nullDefault;
        }
        #region Obsolete

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected int ExecuteScalarAsInt(IDbCommand cmd, int nullDefault = 0)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? int.Parse(result.ToString()) : nullDefault;
        }

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected short ExecuteScalarAsShort(IDbCommand cmd, short nullDefault = 0)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? short.Parse(result.ToString()) : nullDefault;
        }

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected byte ExecuteScalarAsByte(IDbCommand cmd, byte nullDefault = 0)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? byte.Parse(result.ToString()) : nullDefault;
        }

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected long ExecuteScalarAsLong(IDbCommand cmd, long nullDefault = 0)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? long.Parse(result.ToString()) : nullDefault;
        }

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected Guid ExecuteScalarAsGuid(IDbCommand cmd, Guid nullDefault)
        {
            Object result = ExecuteScalar(cmd);
            return result != null ? Guid.Parse(result.ToString()) : nullDefault;
        }

        [System.Obsolete("Use ExecuteScalarAs<int> or ExecuteScalarWithConvertAs<int>")]
        protected Guid ExecuteScalarAsGuid(IDbCommand cmd)
        {
            return ExecuteScalarAsGuid(cmd, Guid.Empty);
        }
        #endregion


        #endregion ExecuteScalar

        #region ExecuteReader
        private IDataReader InternalExecuteReader(IDbCommand cmd)
        {
            bool success = false;
            IDataReader result = null;
            for (int tries = 0; tries <= TotalNumberOfTimesToTry; tries++)
            {
                PrepCommand(cmd);
                try
                {
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * RetryIntervalSeconds * tries);
                    }

                    result = (cmd.ExecuteReader(CommandBehavior.CloseConnection));
                    success = true;
                    break;
                }
                catch (DbException dbEx)
                {
                    if (IsTransientError(dbEx))
                    {
                        if (tries < TotalNumberOfTimesToTry)
                        {
                            continue;
                        }
                        else
                        {
                            // we tried TotalNumberOfTimesToTry times already to error
                            TraceDbError(cmd, dbEx);
                            throw;
                        }
                    }
                    else
                    {
                        TraceDbError(cmd, dbEx);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TraceGenralError(cmd, ex);
                    throw;
                }
                finally
                {
                   // the reader will close the connection
                }
            }

            if (success)
            {
                return result;
            }
            else
            {
                throw new ApplicationException("InternalExecuteScalar failed to find exit path");
            }
        }


  
        protected IDataReader ExecuteReader(IDbCommand cmd)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return InternalExecuteReader(cmd);
        }

        #endregion ExecuteReader

        protected dynamic ExecuteObject(IDbCommand cmd)
        {
            var translator = new DynamicDbRow2ObjectTranslator();
            return translator.Translate(ExecuteDataRow(cmd));
        }

        protected IList<dynamic> ExecuteObjectList(IDbCommand cmd)
        {
            var translator = new DynamicDbRow2ObjectTranslator();
            return translator.Translate(ExecuteDataTable(cmd));
        }


        // execute using a provider translator
        protected T ExecuteTo<T>(IDbCommand cmd) where T : new()
        {
            var translator = _dbRowTranslatorProvider.Resolve<T>();
            return translator.Translate(ExecuteDataRow(cmd));
        }

        protected IList<T> ExecuteToListOf<T>(IDbCommand cmd) where T : new()
        {
            return TranslateToListOf<T>(ExecuteDataTable(cmd));
        }

        /// <summary>
        /// This is used when working with an existing DataTable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The Source DataTable to translate</param>
        /// <returns></returns>
        protected IList<T> TranslateToListOf<T>(DataTable dt) where T : new()
        {
            var translator = _dbRowTranslatorProvider.Resolve<T>();
            return translator.Translate(dt);
        }

    }
}
