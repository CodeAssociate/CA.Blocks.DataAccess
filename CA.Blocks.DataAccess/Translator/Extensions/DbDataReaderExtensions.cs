using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Model.Results;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;


namespace CA.Blocks.DataAccess.Translator.Extensions
{
    public static class DbDataReaderAsyncExtensions
    {



#if NET6_0_OR_GREATER

		/// <summary>
		/// This allows direct execution of a reader to a IAsyncEnumerable this allows streaming of the data without fetching the entire
		/// rowset. This is C# 8.0+ feature so you need .net 6 or greater to use
		/// It is important to note when using this method the caller is responsible for closing the reader when done.
		/// The connection will remain open until the rowset is closed. Unless you are actively looking for streaming ability
		/// it is best to use the ToListOfor ToDictionaryAsync to overrides which will manage the reader connection for you.
		/// Also note whilst you can execute the linq lamdas on the IEnumerable this will typically result in the full read.
		/// Best to use the IList.
		/// Typical cases .Take()
		/// FirstOrDefault ()
		/// Single()
		/// or streaming a very large set with millions of rows. (again with warning you holding the connection open while this is happening.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbReader"></param>
		/// <param name="translate"></param>
		/// <returns></returns>
		public static async IAsyncEnumerable<T> ExecuteToEnumerableAsync<T>(DbDataReader dbReader, Func<IDataReader, T> translate)
        {
            while (await dbReader.ReadAsync().ConfigureAwait(false))
            {
                yield return translate(dbReader);
            }
            yield break;
        }
#endif

	    #region ToListOf

#if NET6_0_OR_GREATER

		/// <summary>
		/// This is a private function that that will execute ExecuteToList but does not close the reader.
		/// The reader is used with one of the public methods.
		/// </summary>
		private static async Task<IList<T>> ExecuteToListAsync<T>(DbDataReader dbReader, Func<IDataReader, T> translate)
        {
			var result = new List<T>();
            await foreach (var item in ExecuteToEnumerableAsync(dbReader, translate).ConfigureAwait(false))
            {
                result.Add(item);
            }
            return result;
        }
#else
        // Asynchronous streams will be part of .Net Standard 2.1, as we target .Net Standard 2.0 for support for the Framework 4.8 or older we cannot use Asynchronous streams 
        // You can use the Sync version

        private static async Task<IList<T>> ExecuteToListAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) 
        {
            IList<T> result = new List<T>();
            while (await dbReader.ReadAsync())
            {
                result.Add(translate(dbReader));
            }
            return result;
        }
#endif


        public static async Task<IList<T>> ToListOfAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate)
        {
            IList<T> result;
            {
                try
                {
                    result = await ExecuteToListAsync(dbReader, translate).ConfigureAwait(false);
                }
                finally
                {
#if NET6_0_OR_GREATER
                await dbReader.CloseAsync().ConfigureAwait(false);
#else
                dbReader.Close();
#endif
                }
            }
            return result;
        }

        public static Task<IList<T>> ToListOfAsync<T>(this DbDataReader dbReader)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToListOfAsync<T>(dbReader, translator1.Translate);
        }

        public static async Task<IList<T>> ToListOf<T>(this Task<DbDataReader> dbReaderTask)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToListOfAsync<T>(dbReader, translator1.Translate).ConfigureAwait(false);
        }

        public static async Task<IList<T>> ToListOf<T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToListOfAsync<T>(dbReader, translate).ConfigureAwait(false);
        }

		#endregion
		//**
#region ExecuteToDictionaryAsync

#if NET6_0_OR_GREATER

		/// <summary>
		/// This is a private function that that will execute ExecuteToDictionaryAsync but does not close the reader.
		/// The reader is used with one of the public methods.
		/// </summary>
		private static async Task<IDictionary<Key, T>> ExecuteToDictionaryAsync<Key, T>(DbDataReader dbReader, Func<IDataReader, T> translate, Func<T, Key> keySelector)
		{
			var result = new Dictionary<Key, T>();
            await foreach (var item in ExecuteToEnumerableAsync(dbReader, translate).ConfigureAwait(false))
            {
                result.Add(keySelector.Invoke(item), item);
            }
            return result;
        }
#else
        // Asynchronous streams will be part of .Net Standard 2.1, as we target .Net Standard 2.0 for support for the Framework 4.8 or older we cannot use Asynchronous streams 
        // You can use the Sync version

        private static async Task<IDictionary<Key, T>> ExecuteToDictionaryAsync<Key, T>(this DbDataReader dbReader, Func<IDataReader, T> translate, Func<T, Key> keySelector) 
        {
			var result = new Dictionary<Key, T>();
			while (await dbReader.ReadAsync())
			{
				var item = translate(dbReader);

				result.Add(keySelector.Invoke(item), item);
            }
            return result;
        }
#endif


        public static async Task<IDictionary<Key, T>> ToDictionaryAsync<Key, T>(this DbDataReader dbReader, Func<IDataReader, T> translate, Func<T, Key> keySelector)
        {
			IDictionary<Key, T> result;
			{
                try
                {
                    result = await ExecuteToDictionaryAsync<Key, T>(dbReader, translate, keySelector).ConfigureAwait(false);
                }
                finally
                {
#if NET6_0_OR_GREATER
                await dbReader.CloseAsync().ConfigureAwait(false);
#else
                dbReader.Close();
#endif
                }
            }
            return result;
        }

        public static Task<IDictionary<Key, T>> ToDictionaryAsync<Key, T>(this DbDataReader dbReader, Func<T, Key> keySelector)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToDictionaryAsync<Key, T>(dbReader, translator1.Translate, keySelector);
        }

        public static async Task<IDictionary<Key, T>> ToDictionaryAsync<Key, T>(this Task<DbDataReader> dbReaderTask, Func<T, Key> keySelector)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToDictionaryAsync<Key, T>(dbReader, translator1.Translate, keySelector).ConfigureAwait(false);
        }

        public static async Task<IDictionary<Key, T>> ToDictionary<Key, T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate, Func<T, Key> keySelector)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToDictionaryAsync<Key, T>(dbReader, translate, keySelector).ConfigureAwait(false);
        }

#endregion




#region ToSingleNamedColumnList
        public static async Task<IList<T>> ToSingleNamedColumnListAsync<T>(this DbDataReader dbReader, string colName, Func<IDataReader, string, T> converter)
        {
            IList<T> result = new List<T>();
            try
            {
                while (await dbReader.ReadAsync().ConfigureAwait(false))
                {
                    result.Add(converter(dbReader, colName));
                }
            }
            finally
            {
#if NET6_0_OR_GREATER
	            await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
			}
			return result;
        }

        public static Task<IList<T>> ToSingleNamedColumnListAsync<T>(this DbDataReader dbReader, string colName)
        {

            IDbColToTypeConverter<T> converter = (IDbColToTypeConverter<T>)DefaultDbColToTypeProvider.DefaultInstance.Resolve<T>();
            return ToSingleNamedColumnListAsync(dbReader, colName, converter.GetDataValue);
        }


        public static async Task<IList<T>> ToSingleNamedColumnList<T>(this Task<DbDataReader> dbReaderTask, string colName)
        {
            IDbColToTypeConverter<T> converter = (IDbColToTypeConverter<T>)DefaultDbColToTypeProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToSingleNamedColumnListAsync<T>(dbReader, colName, converter.GetDataValue).ConfigureAwait(false);
        }

        public static async Task<IList<T>> ToSingleNamedColumnList<T>(this Task<DbDataReader> dbReaderTask, string colName, Func<IDataReader, string, T> converter)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToSingleNamedColumnListAsync<T>(dbReader, colName, converter).ConfigureAwait(false);
        }

        #endregion

        #region ToDataTable This gets asked many Times on stackoverflow howto ExecuteDataTableAsync  The DataAdapter has not async support

        //private 

        private static DataRow DataReaderToDataRow(DbDataReader reader, DataRow newRow)
        {
	        for (var i = 0; i < reader.FieldCount; i++)
	        {
		        if (reader.IsDBNull(i))
		        {
			        newRow[i] = DBNull.Value;

		        }
		        else
		        {
			        newRow[i] = reader[i];
		        }
	        }
	        return newRow;
        }

        private static DataTable CreateDataTableSchemaFromDataReader(DbDataReader reader)
		{
			var result = new DataTable();

			for (int i = 0; i < reader.FieldCount; i++)
			{
				result.Columns.Add(new DataColumn
				{
                    ColumnName = reader.GetName(i),
#if NET6_0_OR_GREATER
					DataType = reader.GetFieldType(i)!
#else
					DataType = reader.GetFieldType(i)
#endif
				});
			}
			return result;
		}

		public static async Task<DataTable> ToDataTable(this DbDataReader dbReader)
		{
			DataTable dt = new DataTable();
			bool schemaCreated = false;
			try
			{
				while (await dbReader.ReadAsync().ConfigureAwait(false))
				{
					if (!schemaCreated)
					{
						dt = CreateDataTableSchemaFromDataReader(dbReader);
						schemaCreated = true;
					}

					dt.Rows.Add(DataReaderToDataRow(dbReader, dt.NewRow()));
				}
			}
			finally
			{
#if NET6_0_OR_GREATER
				await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
			}
			return dt;
		}

		public static async Task<DataTable> ToDataTable(this Task<DbDataReader> dbReaderTask)
		{
			var dbReader = await dbReaderTask.ConfigureAwait(false);
			return await ToDataTable(dbReader).ConfigureAwait(false);
		}

		#endregion

		#region Multi Result Sets
		//2
		public static async Task<ResultsSet<T1, T2>> ToResultsSetAsync<T1, T2>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2
            )
        {
            ResultsSet<T1, T2> result = new ResultsSet<T1, T2>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1).ConfigureAwait(false);
                    if (await dbReader.NextResultAsync().ConfigureAwait(false))
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2).ConfigureAwait(false);
                    }
                }
                finally
                {
#if NET6_0_OR_GREATER
	                await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
				}
			}
            return result;
        }

        public static Task<ResultsSet<T1, T2>> ToResultsSetAsync<T1, T2>(this DbDataReader dbReader)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            return ToResultsSetAsync<T1, T2>(dbReader, translator1.Translate, translator2.Translate);
        }


        public static async Task<ResultsSet<T1, T2>> ToResultsSet<T1, T2>(this Task<DbDataReader> dbReaderTask)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2>(dbReader, translator1.Translate, translator2.Translate).ConfigureAwait(false);
        }

        public static async Task<ResultsSet<T1, T2>> ToResultsSet<T1, T2>(this Task<DbDataReader> dbReaderTask, 
            Func<IDataReader, T1> translate1, 
            Func<IDataReader, T2> translate2)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2>(dbReader, translate1, translate2).ConfigureAwait(false);
        }


        //3
        public static async Task<ResultsSet<T1, T2, T3>> ToResultsSetAsync<T1, T2, T3>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3
        )
        {
            ResultsSet<T1, T2, T3> result = new ResultsSet<T1, T2, T3>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1).ConfigureAwait(false);
                    if (await dbReader.NextResultAsync().ConfigureAwait(false))
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2).ConfigureAwait(false);
                        if (await dbReader.NextResultAsync().ConfigureAwait(false))
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3).ConfigureAwait(false);
                        }
                    }
                }
                finally
                {
#if NET6_0_OR_GREATER
	                await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
				}
			}
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3>> ToResultsSetAsync<T1, T2, T3>(this DbDataReader dbReader)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            return ToResultsSetAsync<T1, T2, T3>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate);
        }


        public static async Task<ResultsSet<T1, T2, T3>> ToResultsSet<T1, T2, T3>(this Task<DbDataReader> dbReaderTask)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate).ConfigureAwait(false);
        }

        public static async Task<ResultsSet<T1, T2, T3>> ToResultsSet<T1, T2, T3>(this Task<DbDataReader> dbReaderTask,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3>(dbReader, translate1, translate2, translate3).ConfigureAwait(false);
        }


        //4
        public static async Task<ResultsSet<T1, T2, T3, T4>> ToResultsSetAsync<T1, T2, T3, T4>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3,
            Func<IDataReader, T4> translate4
        )
        {
            ResultsSet<T1, T2, T3, T4> result = new ResultsSet<T1, T2, T3, T4>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1).ConfigureAwait(false);
                    if (await dbReader.NextResultAsync().ConfigureAwait(false))
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2).ConfigureAwait(false);
                        if (await dbReader.NextResultAsync().ConfigureAwait(false))
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3).ConfigureAwait(false);
                            if (await dbReader.NextResultAsync().ConfigureAwait(false))
                            {
                                result.Results4 = await ExecuteToListAsync(dbReader, translate4).ConfigureAwait(false);
                            }
                        }
                    }
                }
                finally
                {
#if NET6_0_OR_GREATER
	                await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
				}
			}
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3, T4>> ToResultsSetAsync<T1, T2, T3, T4>(this DbDataReader dbReader)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            return ToResultsSetAsync<T1, T2, T3, T4>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate);
        }

        public static async Task<ResultsSet<T1, T2, T3, T4>> ToResultsSet<T1, T2, T3, T4>(this Task<DbDataReader> dbReaderTask)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3, T4>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate).ConfigureAwait(false);
        }

        public static async Task<ResultsSet<T1, T2, T3, T4>> ToResultsSet<T1, T2, T3, T4>(this Task<DbDataReader> dbReaderTask,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3,
            Func<IDataReader, T4> translate4)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3, T4>(dbReader, translate1, translate2, translate3, translate4).ConfigureAwait(false);
        }


        // 5 
        public static async Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSetAsync<T1, T2, T3, T4, T5>(this DbDataReader dbReader,
         Func<IDataReader, T1> translate1,
         Func<IDataReader, T2> translate2,
         Func<IDataReader, T3> translate3,
         Func<IDataReader, T4> translate4,
         Func<IDataReader, T5> translate5
     )
        {
            ResultsSet<T1, T2, T3, T4, T5> result = new ResultsSet<T1, T2, T3, T4, T5>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1).ConfigureAwait(false);
                    if (await dbReader.NextResultAsync().ConfigureAwait(false))
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2).ConfigureAwait(false);
                        if (await dbReader.NextResultAsync().ConfigureAwait(false))
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3).ConfigureAwait(false);
                            if (await dbReader.NextResultAsync().ConfigureAwait(false))
                            {
                                result.Results4 = await ExecuteToListAsync(dbReader, translate4).ConfigureAwait(false);
                                if (await dbReader.NextResultAsync().ConfigureAwait(false))
                                {
                                    result.Results5 = await ExecuteToListAsync(dbReader, translate5).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
                finally
                {
#if NET6_0_OR_GREATER
	                await dbReader.CloseAsync().ConfigureAwait(false);
#else
				dbReader.Close();
#endif
				}
			}
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSetAsync<T1, T2, T3, T4, T5>(this DbDataReader dbReader)
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            var translator5 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T5>();
            return ToResultsSetAsync<T1, T2, T3, T4, T5>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate, translator5.Translate);
        }

        public static async Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSet<T1, T2, T3, T4, T5>(this Task<DbDataReader> dbReaderTask)
        {

            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            var translator5 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T5>();
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3, T4, T5>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate, translator5.Translate).ConfigureAwait(false);
        }

        public static async Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSet<T1, T2, T3, T4, T5>(this Task<DbDataReader> dbReaderTask,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3,
            Func<IDataReader, T4> translate4,
            Func<IDataReader, T5> translate5)
        {
            var dbReader = await dbReaderTask.ConfigureAwait(false);
            return await ToResultsSetAsync<T1, T2, T3, T4, T5>(dbReader, translate1, translate2, translate3, translate4, translate5).ConfigureAwait(false);
        }

#endregion
    }
}