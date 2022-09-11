using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;

namespace CA.Blocks.DataAccess.Translator.Extensions
{
    public static class DbDataReaderSingeRowExtensions
    {
        public static async Task<T> ToFirstOrDefaultAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            T result;
            try
            {
                var hasData = await dbReader.ReadAsync();
                result = hasData ? translate(dbReader) : default;
            }
            finally
            {
                dbReader.Close();
            }
            
            return result;
        }

        public static Task<T> ToFirstOrDefaultAsync<T>(this DbDataReader dbReader) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToFirstOrDefaultAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToFirstOrDefault<T>(this Task<DbDataReader> dbReaderTask) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask;
            return await ToFirstOrDefaultAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToFirstOrDefault<T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate) where T : new()
        {
            var dbReader = await dbReaderTask;
            return await ToFirstOrDefaultAsync(dbReader, translate);
        }


        public static async Task<T> ToFirstAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            T result = await ToFirstOrDefaultAsync(dbReader, translate);
            if (result == null || result.Equals(default(T)))
            {
                throw new DataException("Expected Single Result,but No row was found");
            };
            return result;
        }

        public static Task<T> ToFirstAsync<T>(this DbDataReader dbReader) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToFirstAsync(dbReader, translator.Translate);
        }
        public static async Task<T> ToFirst<T>(this Task<DbDataReader> dbReaderTask) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask;
            return await ToFirstAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToFirst<T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate) where T : new()
        {
            var dbReader = await dbReaderTask;
            return await ToFirstAsync(dbReader, translate);
        }

        public static async Task<T> ToSingleOrDefaultAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            T result;
            try
            {
                var hasData = await dbReader.ReadAsync();
                result = hasData ? translate(dbReader) : default;

                if (await dbReader.ReadAsync())
                {
                    throw new DataException("Expected Single Result, but more that one row was found");
                }
            }
            finally
            {
                dbReader.Close();
            }

            return result;
        }


        public static Task<T> ToSingleOrDefaultAsync<T>(this DbDataReader dbReader) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToSingleOrDefaultAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToSingleOrDefault<T>(this Task<DbDataReader> dbReaderTask) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask;
            return await ToSingleOrDefaultAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToSingleOrDefault<T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate) where T : new()
        {
            var dbReader = await dbReaderTask;
            return await ToSingleOrDefaultAsync(dbReader, translate);
        }

        public static async Task<T> ToSingleAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            T result = await ToSingleOrDefaultAsync(dbReader, translate);
            if (result == null || result.Equals(default(T)))
            {
                throw new DataException("Expected Single Result,but No row was found");
            };
            return result;
        }

        public static Task<T> ToSingleAsync<T>(this DbDataReader dbReader) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToSingleAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToSingle<T>(this Task<DbDataReader> dbReaderTask) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            var dbReader = await dbReaderTask;
            return await ToSingleAsync(dbReader, translator.Translate);
        }

        public static async Task<T> ToSingle<T>(this Task<DbDataReader> dbReaderTask, Func<IDataReader, T> translate) where T : new()
        {
            var dbReader = await dbReaderTask;
            return await ToSingleAsync(dbReader, translate);
        }

    }
}