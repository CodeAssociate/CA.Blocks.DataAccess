//===============================================================================
// Code Associate Data Access Block for .NET Core 
// DataAccessCore.cs
//
//===============================================================================
// Copyright (C) 2002-2022 Ravin Enterprises Ltd. 
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
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Translator.Extensions;
#pragma warning disable CS0618 // Type or member is obsolete

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

        /// <summary>
        /// This sets the default CommandBehavior for the DataReader, The Default assumes connection pooling however with with embedded databases there is no pooling
        /// so it Likely you will want overrider this
        /// </summary>
        protected virtual CommandBehavior DefaultCommandBehavior => CommandBehavior.CloseConnection;

        /// <summary>
        /// ConnectionString Used for the provider. This is used in the provider blocks to get a connection string to use  
        /// </summary>
        protected string ConnectionString { get; }
        
        protected DependencyInjection.IConnectionTokenResolver ConnectionTokenResolver { get; }

        #region private utility methods & constructors

        /// <summary>
        /// This is a protected constructor which must be called by the inheriting class, it will use config.Resolver to resolve the connectionStringKey to a valid connection string 
        /// </summary>
        protected DataAccessCore(IDataAccessConfig config, IDbRowTranslatorProvider? dbRowTranslatorProvider)
        {
            _dbRowTranslatorProvider = dbRowTranslatorProvider ?? DefaultDbRowTranslatorProvider.DefaultInstance;
            _options = config.Options;
            ConnectionString = config.Resolver.GetConnectionString(_options.ConnectionStringKey);
            ConnectionTokenResolver = config.ConnectionTokenResolver;
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


        /// <summary>
        /// This method provides a the ability to trace the commands executed against a store. The Trace will happen just before the execute of the command.
        /// The Design is such that you can override this method to implement your own logic.   
        /// Example you can hook this method up in app your application Insights to trace all the DB commands 
        /// </summary>
        /// <param name="cmd"></param>
        protected virtual void TraceDbStatement(IDbCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        }

        /// <summary>
        /// When a error occurs in executing the DbCommand command this method will be called
        ///  The Design is such that you can override this method to implement your own logic, you you get the command and well as the DbException.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ex"></param>
        protected virtual void TraceDbError(IDbCommand cmd, DbException ex)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        /// <summary>
        /// When a error occurs in executing the DbCommand command and the error is deemed to be a TransientError this method will be called
        ///  The Design is such that you can override this method to implement your own logic, you you get the command and well as the DbException.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ex"></param>
        protected virtual void TraceTransientErrorDbError(IDbCommand cmd, DbException ex)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }


        /// <summary>
        /// When a general occurs not related to the database such as network error this method will be called
        ///  The Design is such that you can override this method to implement your own logic, you get the command and well as the Exception.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ex"></param>
        protected virtual void TraceGeneralError(IDbCommand cmd, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(cmd.CommandText);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        
        private CancellationTokenSource CreateCancellationTokenSource(CancellationToken cancellationToken, int timeoutInSeconds)
        {
            // Create a CancellationTokenSource linked to the timeout duration of the Command
            var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));
            // Link the timeout token with the user's incoming CancellationToken
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
            // Note: Dispose timeoutCts since linkedCts manages its own lifecycle once linked
            timeoutCts.Dispose();
            return linkedCts;
        }
        
        #endregion private utility methods & constructors


        #region ExecuteWithTransientErrorRetry
        private T ExecuteWithTransientErrorRetry<T>(Func<T> action, IDbCommand cmd, bool autoCloseConnection = true)
        {
	        List<Exception>? exceptions = null;
            for (var tries = 0; tries < _options.TransientErrorRetryTotalNumberOfTimesToTry; tries++)
            {
                var closeConnection = true;
                try
                {
                    closeConnection = PrepCommand(cmd);
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * _options.TransientErrorRetryRetryIntervalSeconds * tries);
                    }
                    return action();
                }
                catch (DbException dbEx)
                {
	                if (exceptions == null)
	                {
		                exceptions = new List<Exception>();
	                }

					if (IsTransientError(dbEx))
                    {
                        if (tries < _options.TransientErrorRetryTotalNumberOfTimesToTry - 1)
                        {
                            // if we still have one more try
                            exceptions.Add(dbEx);
                            TraceTransientErrorDbError(cmd, dbEx);
                        }
                        else
                        {
                            // we tried TotalNumberOfTimesToTry times already so report the error
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
                    TraceGeneralError(cmd, ex);
                    throw;
                }
                finally
                {
                    if (autoCloseConnection) // if we executing a reader you can only close after you have read all the data
                    {
                        WrapUp(cmd.Connection, closeConnection);
                    }
                }
            }

            if (exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
            else
            {
                throw new Exception("Ünexpected exit with no exceptions");
            }
        }

        private async Task<T> ExecuteWithTransientErrorRetryAsync<T>(Func<Task<T>> action, IDbCommand cmd, bool autoCloseConnection = true, CancellationToken cancellationToken = default)
        {
	        List<Exception>? exceptions = null; 
            for (int tries = 0; tries < _options.TransientErrorRetryTotalNumberOfTimesToTry; tries++)
            {
                // Bail immediately if cancellation was requested before starting a new try
                cancellationToken.ThrowIfCancellationRequested();
                
                bool closeConnection = PrepCommand(cmd);
                try
                {
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        await Task.Delay(1000 * _options.TransientErrorRetryRetryIntervalSeconds * tries, cancellationToken).ConfigureAwait(false);
                    }
                    return await action().ConfigureAwait(false);
                }
                catch (OperationCanceledException ex)
                {
                    // 3. Do NOT treat cancellation as a database transient error or retry.
                    // Fast-fail and propagate the cancellation upwards.
                    TraceGeneralError(cmd, ex);
                    throw; 
                }
                catch (DbException dbEx)
                {
	                exceptions ??= new List<Exception>();
                    exceptions.Add(dbEx);
                    
	                if (IsTransientError(dbEx))
                    {
                        if (tries < _options.TransientErrorRetryTotalNumberOfTimesToTry - 1)
                        {
                            TraceTransientErrorDbError(cmd, dbEx);
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
                    TraceGeneralError(cmd, ex);
                    throw;
                }
                finally
                {
                    if (autoCloseConnection) // if we executing a reader you can only close after you have read all the data
                    {
                        WrapUp(cmd.Connection, closeConnection);
                    }
                }
            }
            if (exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
            else
            {
                throw new Exception("Ünexpected exit with no exceptions");
            }
        }

		#endregion



		#region abstract methods that must me implemented

		/// <summary>
		/// This provides a more abstract way to create a DbCommand, ths key advantage is that you can hook external components
		/// that know how to work at the generic DbCommand level. Example Profiling.
        /// </summary>
		/// <param name="sql"></param>
		/// <param name="cmdType"></param>
		/// <returns></returns>
		protected abstract DbCommand CreateDbCommand(string sql, CommandType cmdType = CommandType.Text);


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
            return ExecuteWithTransientErrorRetry(cmd.ExecuteNonQuery, cmd);
        }
        
        /// <inheritdoc cref="ExecuteNonQuery(IDbCommand)" />
        protected Task<int> ExecuteNonQueryAsync(IDbCommand cmd, CancellationToken cancellationToken = default)
        {
            var asyncCmd = cmd as DbCommand;
            if (asyncCmd == null)
            {
                throw new InvalidCastException("To Execute Async command the provider by implement DbCommand");
            }
            
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return ExecuteWithTransientErrorRetryAsync(async () =>
            {
#if CS80_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                using var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout);
                return await asyncCmd.ExecuteNonQueryAsync(linkedCts.Token);
#else
                using (var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout))
                {
                    return await asyncCmd.ExecuteNonQueryAsync(linkedCts.Token);
                }
#endif
            }, cmd, cancellationToken: cancellationToken);
        }
        #endregion ExecuteNonQuery

        #region ExecuteDataSet

        private void InternalExecuteDataSet(IDbCommand cmd, DataSet ds, string sTableNames)
        {
            bool success = false;
            for (int tries = 0; tries < _options.TransientErrorRetryTotalNumberOfTimesToTry; tries++)
            {
                bool closeConnection = true;
                try
                {
                    closeConnection = PrepCommand(cmd);
                    if (tries > 0)
                    {
                        // if RetryIntervalSeconds = 10 seconds then
                        // try0 = 0,  Try 1 wait 10 seconds, try two wait 20 seconds, Try three wait 30 seconds, then finally 40 seconds then bail. 
                        Thread.Sleep(1000 * _options.TransientErrorRetryRetryIntervalSeconds * tries);
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
                        if (tries < _options.TransientErrorRetryTotalNumberOfTimesToTry - 1)
                        {
                            TraceTransientErrorDbError(cmd, dbEx);
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
                    TraceGeneralError(cmd, ex);
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


        /// <inheritdoc cref="ExecuteDataSet(IDbCommand, DataSet, string)" />
        [Obsolete("ExecuteDataSet uses DbDataAdapter which is legacy architecture and is no longer recommended in modern .NET. Support will be dropped in the next major version (4) to optimize performance.", false)]
        protected DataSet ExecuteDataSet(IDbCommand cmd)
        {
            DataSet ds = new DataSet();
            return (ExecuteDataSet(cmd, ds, "Results"));
        }
        
        /// <summary>
        /// Executes the command into a new DataSet using the DbDataAdapter.  Useful for app that need or what DatSet, DataTable and DataRows.  The ExecuteTo it more modern 
        /// </summary>
        /// <param name="cmd"> A data set return, The first name name will be called Results the second will be called  Results1, third will be called Results2 etc</param>
        /// <param name="sTableNames"> LIst of table tables</param>>
        /// <param name="ds"> DataSet </param>>
        /// <remarks> This method is using the DbDataAdapter and DataTables, inside the data Adapter there is need to call Fill, there is currently no generic support FillAsync.
        /// Using this is very robust but as the  DbDataAdapter is mostly limited to maintenance at this point.
        /// </remarks>
        /// <returns></returns>
        
        [Obsolete("ExecuteDataSet uses DbDataAdapter which is legacy architecture and is no longer recommended in modern .NET. Support will be dropped in the next major version (4) to optimize performance.", false)]

        protected DataSet ExecuteDataSet(IDbCommand cmd, DataSet ds, string sTableNames)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            InternalExecuteDataSet(cmd, ds, sTableNames);
            return (ds);
        }

        #endregion ExecuteDataSet

        #region ExecuteTable

        /// <summary>
        /// Executes to ExecuteDataSet and returns the first DataTable <see cref="ExecuteDataSet(IDbCommand, DataSet, string)"/>
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        // about to happen but not in this commit
        [Obsolete("ExecuteDataSet uses DbDataAdapter which is legacy architecture and is no longer recommended in modern .NET. " +
                  "Support will be dropped in the next major version (4) to optimize performance." +
                  "You can use Execute(cmd).ToDateTable() as a replacement", false)]
        protected DataTable ExecuteDataTable(IDbCommand cmd)
        {
            DataSet ds = ExecuteDataSet(cmd);
            return (ds.Tables[0]);
        }



        #endregion ExecuteTable

        #region ExecuteDataRow

        /// <summary>
        /// Executes to ExecuteDataTable and returns the first row 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// <exception cref="DataException"> This function expects one or zero results, it is get two more more it will throw an exception</exception>
        [Obsolete("ExecuteDataSet uses DbDataAdapter which is legacy architecture and is no longer recommended in modern .NET. " +
                  "Support will be dropped in the next major version (4) to optimize performance." +
                  "You can use Execute(cmd).ToDateTable() then take first row as a replacement", false)]
        protected DataRow? ExecuteDataRow(IDbCommand cmd)
        {
            DataSet ds = ExecuteDataSet(cmd);
            DataRow? dr = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                    dr = ds.Tables[0].Rows[0];
                else
                    throw new DataException("Command was asked to execute a ExecuteDataRow however more than a single data row was found, ExecuteDataRow expects one or zero rows returned");
            }
            return dr;
        }

        #endregion ExecuteDataRow

        #region ExecuteScalar

        protected object? ExecuteScalar(IDbCommand cmd)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return ExecuteWithTransientErrorRetry(cmd.ExecuteScalar, cmd);
        }

        protected async Task<object?> ExecuteScalarAsync(IDbCommand cmd, CancellationToken cancellationToken = default)
        {
            var asyncCmd = cmd as DbCommand;
            if (asyncCmd == null)
            {
                throw new InvalidCastException("To Execute Async command the provider must implement DbCommand");
            }
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            return await ExecuteWithTransientErrorRetryAsync(async () =>
            {
#if CS80_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
            using var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout);
            return await asyncCmd.ExecuteScalarAsync(linkedCts.Token);
#else
                using (var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout))
                {
                    return await asyncCmd.ExecuteScalarAsync(linkedCts.Token);
                }
#endif
            }, cmd, cancellationToken: cancellationToken);
        }

         private T ConvertScalarAs<T>(object result, bool useCast)
         {
#if NET6_0_OR_GREATER
             if (typeof(T) == typeof(DateOnly))
             {
                 // waiting for driver support
                 var dt = (DateTime)result;
                 var dateOnly = new DateOnly(dt.Year, dt.Month, dt.Day);
                 return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(dateOnly.ToString());
             }
             if (typeof(T) == typeof(TimeOnly))
             {
                 // waiting for driver support
                 var ts = (TimeSpan)result;
                 var timeOnly = new TimeOnly(ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, ts.Nanoseconds);
                
                 return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(timeOnly.ToString());
             }
#endif
             // cast is faster but you only use it if the type is known and the same between .NET and the DB
             return useCast ? (T)result : (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(result.ToString());          
         }

         /// <summary>
        /// This will execute the cmd to a ScalarValue and cast the ScalarValue to the target Type. Use this command if you can know the data type on  the server.
        /// </summary>
        /// <typeparam name="T">This the target type to execute the result in to</typeparam>
        /// <param name="cmd">The IDbCommand cmd to execute</param>
        /// <returns></returns>
        protected T ExecuteScalarAs<T>(IDbCommand cmd) 
        {
            var result = ExecuteScalar(cmd);
            return (result == null || result == DBNull.Value) ? default : ConvertScalarAs<T>(result, true);
        }

        protected async Task<T> ExecuteScalarAsAsync<T>(IDbCommand cmd)
        {
            var result = await ExecuteScalarAsync(cmd);
            return (result == null || result == DBNull.Value) ? default : ConvertScalarAs<T>(result, true);
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
            var result = ExecuteScalar(cmd);
            return (result == null || result == DBNull.Value) ? default : ConvertScalarAs<T>(result, false);
        }

        protected async Task<T> ExecuteScalarWithConvertAsAsync<T>(IDbCommand cmd)
        {
            var result = await ExecuteScalarAsync(cmd);
            return (result == null || result == DBNull.Value) ? default : ConvertScalarAs<T>(result, false);
        }


        /// <summary>
        /// This will execute the cmd to a ScalarValue and convert the ScalarValue to string value, There is a overload top control the value of the null passed back.
        /// </summary>
        /// <param name="cmd">The IDbCommand cmd to execute</param>
        /// <param name="nullDefault">If execute result is null this this value will be passed back. </param>
        /// <returns></returns>
        protected string? ExecuteScalarAsString(IDbCommand cmd, string? nullDefault = null)
        {
            var result = ExecuteScalar(cmd);
            return result != null ? result.ToString() : nullDefault;
        }
        
        #endregion ExecuteScalar

        #region ExecuteReader

        protected IDataReader ExecuteReader(IDbCommand cmd)
        {
            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            //return InternalExecuteReader(cmd);
            return ExecuteWithTransientErrorRetry(() => cmd.ExecuteReader(DefaultCommandBehavior), cmd, false);
        }

        protected async Task<DbDataReader> ExecuteReaderAsync(IDbCommand cmd, CancellationToken cancellationToken = default)
        {
            if (!(cmd is DbCommand asyncCmd))
            {
                throw new InvalidCastException("To Execute Async command the provider by implement DbCommand");
            }

            if (_options.DebugTrace)
                TraceDbStatement(cmd);
            
            return await ExecuteWithTransientErrorRetryAsync(async () =>
            {
#if CS80_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                using var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout);
                return await asyncCmd.ExecuteReaderAsync(DefaultCommandBehavior, linkedCts.Token);
#else
                using (var linkedCts = CreateCancellationTokenSource(cancellationToken, cmd.CommandTimeout))
                {
                    return await asyncCmd.ExecuteReaderAsync(DefaultCommandBehavior, linkedCts.Token);
                }
#endif
            }, cmd, autoCloseConnection: false, cancellationToken: cancellationToken);
        }

        protected async Task<DbDataReader> ExecuteAsync(IDbCommand cmd, CancellationToken cancellationToken = default)
        {
            return await ExecuteReaderAsync(cmd, cancellationToken);
        }

        // TODO implement support but pass the token to the provider 
        /*
        protected async Task<DbDataReader> ExecuteAsync(IDbCommand cmd, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await ExecuteReaderAsync(cmd);
        }*/

        protected IDataReader Execute(IDbCommand cmd)
        {
            return ExecuteReader(cmd);
        }

		#endregion ExecuteReader


		protected dynamic? ExecuteObject(IDbCommand cmd)
        {
            var translator = new DynamicDbRow2ObjectTranslator();
            return translator.Translate(Execute(cmd).ToDataRow());
        }

        protected IList<dynamic?> ExecuteObjectList(IDbCommand cmd)
        {
            var translator = new DynamicDbRow2ObjectTranslator();
            return translator.Translate(Execute(cmd).ToDataTable());
        }


        /// <summary>
        ///  This is a shortcut method to <code>Execute(cmd).ToFirstOrDefault(translate);</code> please use that instead
        /// </summary>
        /// <returns></returns>
        protected T ExecuteTo<T>(IDbCommand cmd)
        {
            return Execute(cmd).ToFirstOrDefault<T>();
        }

        /// <summary>
        ///  This is a shortcut method to <code>Execute(cmd).ToFirstOrDefault(translate);</code> please use that instead
        /// </summary>
        /// <returns></returns>
        protected T ExecuteTo<T>(IDbCommand cmd, Func<IDataReader, T> translate)
        {
            return Execute(cmd).ToFirstOrDefault(translate);
        }
        
        /// <summary>
        ///  This is a shortcut method to <code>Execute(cmd).ToListOf(translate);</code> please use that instead
        /// </summary>
        /// <returns></returns>
        protected IList<T> ExecuteToListOf<T>(IDbCommand cmd)
        {
            var translator = _dbRowTranslatorProvider.Resolve<T>();
            return ExecuteToListOf(cmd, translator.Translate);
        }

        /// <summary>
        ///  This is a shortcut method to <code>ExecuteReader<T>(cmd).ToListOf<T>(translate);</T></T></code> please use that instead
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        protected IList<T> ExecuteToListOf<T>(IDbCommand cmd, Func<IDataReader, T> translate) 
        {
            return ExecuteReader(cmd).ToListOf(translate);
        }

        /// <summary>
        ///  This is a shortcut method to <code>ExecuteReader<T>(cmd).ToFirstOrDefault<T>(translate);</T></T></code> please use that instead
        /// </summary>
        /// <returns></returns>
        protected Task<T> ExecuteToAsync<T>(IDbCommand cmd) 
        {
            return ExecuteAsync(cmd).ToFirstOrDefault<T>();
        }

        /// <summary>
        ///  This is a shortcut method to <code>ExecuteReader<T>(cmd).ToFirstOrDefault<T>(translate);</T></T></code> please use that instead
        /// </summary>
        /// <returns></returns>
        protected Task<T> ExecuteToAsync<T>(IDbCommand cmd, Func<IDataReader, T> translate)
        {
            return ExecuteAsync(cmd).ToFirstOrDefault(translate);
        }

        protected Task<IList<T>> ExecuteToListOfAsync<T>(IDbCommand cmd) 
        {
            var translator = _dbRowTranslatorProvider.Resolve<T>();
            return ExecuteToListOfAsync(cmd, translator.Translate);
        }

        /// <summary>
        /// This is a shortcut to <code> ExecuteReaderAsync(cmd).ToListOfAsync(); </code> "/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        protected async Task<IList<T>> ExecuteToListOfAsync<T>(IDbCommand cmd, Func<IDataReader, T> translate) 
        {
            var dbResult = await ExecuteReaderAsync(cmd);
            return await dbResult.ToListOfAsync(translate);
        }

        /// <summary>
        /// This is used when working with an existing DataTable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The Source DataTable to translate</param>
        /// <returns></returns>
        protected IList<T> TranslateToListOf<T>(DataTable dt)
        {
            var translator = _dbRowTranslatorProvider.Resolve<T>();
            return translator.Translate(dt);
        }
        
        protected virtual DataTable GetSchema(string collectionNam, string[]? restrictionValues = null)
        {
	        throw new NotImplementedException("GetSchema not Not Implemented");
        }
    }
}
