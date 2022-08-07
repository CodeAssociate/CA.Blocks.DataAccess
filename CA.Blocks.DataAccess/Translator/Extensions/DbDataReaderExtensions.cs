using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;

namespace CA.Blocks.DataAccess.Translator.Extensions
{
    // When Executing the Data Reader Async we will be back back a DbDataReader and not an IDataReader
    public static class DbDataReaderAsyncExtensions
    {

        private static async Task<IList<T>> ExecuteToListAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            IList<T> result = new List<T>();
            while (await dbReader.ReadAsync())
            {
                result.Add(translate(dbReader));
            }
            return result;
        }

        public static async Task<IList<T>> ToListOfAsync<T>(this DbDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            IList<T> result;
            {
                try
                {
                    result = await ExecuteToListAsync(dbReader, translate);
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }


        public static async Task<IList<T>> ToSingleNamedColumnListAsync<T>(this DbDataReader dbReader, string colName, Func<IDataReader, string, T> converter)
        {
            IList<T> result = new List<T>();
            try
            {
                while (await dbReader.ReadAsync())
                {
                    result.Add(converter(dbReader, colName));
                }
            }
            finally
            {
                dbReader.Close();
            }
            return result;
        }

        public static Task<IList<T>> ToSingleNamedColumnListAsync<T>(this DbDataReader dbReader, string colName)
        {

            IDbColToTypeConverter<T> converter = (IDbColToTypeConverter<T>)DefaultDbColToTypeProvider.DefaultInstance.Resolve<T>();
            return ToSingleNamedColumnListAsync(dbReader, colName, converter.GetDataValue);
        }
    }
}