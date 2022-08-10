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


        #region Multi Result Sets
        //2
        public static async Task<ResultsSet<T1, T2>> ToResultsSetAsync<T1, T2>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2
            )
            where T1 : new()
            where T2 : new()
        {
            ResultsSet<T1, T2> result = new ResultsSet<T1, T2>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1);
                    if (await dbReader.NextResultAsync())
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2);
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static Task<ResultsSet<T1, T2>> ToResultsSetAsync<T1, T2>(this DbDataReader dbReader)
            where T1 : new()
            where T2 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            return ToResultsSetAsync<T1, T2>(dbReader, translator1.Translate, translator2.Translate);
        }


        //3
        public static async Task<ResultsSet<T1, T2, T3>> ToResultsSetAsync<T1, T2, T3>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3
        )
            where T1 : new()
            where T2 : new()
            where T3 : new()
        {
            ResultsSet<T1, T2, T3> result = new ResultsSet<T1, T2, T3>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1);
                    if (await dbReader.NextResultAsync())
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2);
                        if (await dbReader.NextResultAsync())
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3);
                        }
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3>> ToResultsSetAsync<T1, T2, T3>(this DbDataReader dbReader)
            where T1 : new()
            where T2 : new()
            where T3 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            return ToResultsSetAsync<T1, T2, T3>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate);
        }

        //4
        public static async Task<ResultsSet<T1, T2, T3, T4>> ToResultsSetAsync<T1, T2, T3, T4>(this DbDataReader dbReader,
            Func<IDataReader, T1> translate1,
            Func<IDataReader, T2> translate2,
            Func<IDataReader, T3> translate3,
            Func<IDataReader, T4> translate4
        )
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
        {
            ResultsSet<T1, T2, T3, T4> result = new ResultsSet<T1, T2, T3, T4>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1);
                    if (await dbReader.NextResultAsync())
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2);
                        if (await dbReader.NextResultAsync())
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3);
                            if (await dbReader.NextResultAsync())
                            {
                                result.Results4 = await ExecuteToListAsync(dbReader, translate4);
                            }
                        }
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3, T4>> ToResultsSetAsync<T1, T2, T3, T4>(this DbDataReader dbReader)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            return ToResultsSetAsync<T1, T2, T3, T4>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate);
        }


        // 5 
        public static async Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSetAsync<T1, T2, T3, T4, T5>(this DbDataReader dbReader,
         Func<IDataReader, T1> translate1,
         Func<IDataReader, T2> translate2,
         Func<IDataReader, T3> translate3,
         Func<IDataReader, T4> translate4,
         Func<IDataReader, T5> translate5
     )
         where T1 : new()
         where T2 : new()
         where T3 : new()
         where T4 : new()
         where T5 : new()
        {
            ResultsSet<T1, T2, T3, T4, T5> result = new ResultsSet<T1, T2, T3, T4, T5>();
            {
                try
                {
                    result.Results1 = await ExecuteToListAsync(dbReader, translate1);
                    if (await dbReader.NextResultAsync())
                    {
                        result.Results2 = await ExecuteToListAsync(dbReader, translate2);
                        if (await dbReader.NextResultAsync())
                        {
                            result.Results3 = await ExecuteToListAsync(dbReader, translate3);
                            if (await dbReader.NextResultAsync())
                            {
                                result.Results4 = await ExecuteToListAsync(dbReader, translate4);
                                if (await dbReader.NextResultAsync())
                                {
                                    result.Results5 = await ExecuteToListAsync(dbReader, translate5);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static Task<ResultsSet<T1, T2, T3, T4, T5>> ToResultsSetAsync<T1, T2, T3, T4, T5>(this DbDataReader dbReader)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            var translator5 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T5>();
            return ToResultsSetAsync<T1, T2, T3, T4, T5>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate, translator5.Translate);
        }
        #endregion
    }
}