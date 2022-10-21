using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.Model.Results;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;

namespace CA.Blocks.DataAccess.Translator.Extensions
{
    public static class DataReaderExtensions
    {

        /// <summary>
        /// This allows direct execution of a reader to a Enumerable this allows streaming of the data without fetching the entire
        /// rowset. It it important to note when using this method the caller is responsible for closing the reader when done.
        /// The connection will remain open until the rowset is closed. Unless you are actively looking for streaming ability
        /// it is best to use the ToListOf overrides which will manage the reader connection for you.
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
        public static IEnumerable<T> ExecuteToEnumerable<T>(IDataReader dbReader, Func<IDataReader, T> translate)
        {
            while (dbReader.Read())
            {
                yield return translate(dbReader);
            }
            yield break;
        }
        /// <summary>
        /// This is a private function that that will execute ExecuteToList but does not close the reader.
        /// The reader is used with one of the public methods.
        /// </summary>
        private static IList<T> ExecuteToList<T>(IDataReader dbReader, Func<IDataReader, T> translate)
        {
            return ExecuteToEnumerable(dbReader, translate).ToList();
        }

        public static IList<T> ToListOf<T>(this IDataReader dbReader, Func<IDataReader, T> translate)
        {
            IList<T> result;
            {
                try
                {
                    result = ExecuteToList(dbReader, translate);
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }
        
        public static IList<T> ToListOf<T>(this IDataReader dbReader) where T : new()
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToListOf(dbReader, translator.Translate);
        }

        public static IList<T> ToSingleNamedColumnList<T>(this IDataReader dbReader, string colName, Func<IDataReader, string, T> converter)
        {
            IList<T> result = new List<T>();
            try
            {
                while (dbReader.Read())
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

        public static IList<T> ToSingleNamedColumnList<T>(this IDataReader dbReader, string colName)
        {
        
            IDbColToTypeConverter<T> converter = (IDbColToTypeConverter<T>)DefaultDbColToTypeProvider.DefaultInstance.Resolve<T>();
            return ToSingleNamedColumnList(dbReader, colName, converter.GetDataValue);
        }

        #region Multi Result Sets
        //2
        public static ResultsSet<T1, T2> ToResultsSet<T1, T2>(this IDataReader dbReader,
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
                    result.Results1 = ExecuteToList(dbReader, translate1);
                    if (dbReader.NextResult())
                    {
                        result.Results2 = ExecuteToList(dbReader, translate2);
                    }
                    else
                    {
                        result.Results2 = new List<T2>();
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static ResultsSet<T1, T2> ToResultsSet<T1, T2>(this IDataReader dbReader)
            where T1 : new()
            where T2 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            return ToResultsSet<T1, T2>(dbReader, translator1.Translate, translator2.Translate);
        }


        //3
        public static ResultsSet<T1, T2, T3> ToResultsSet<T1, T2, T3>(this IDataReader dbReader,
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
                    result.Results1 = ExecuteToList(dbReader, translate1);
                    if (dbReader.NextResult())
                    {
                        result.Results2 = ExecuteToList(dbReader, translate2);
                        if (dbReader.NextResult())
                        {
                            result.Results3 = ExecuteToList(dbReader, translate3);
                        }
                        else
                        {
                            result.Results3 = new List<T3>();
                        }
                    }
                    else
                    {
                        result.Results2 = new List<T2>();
                        result.Results3 = new List<T3>();
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static ResultsSet<T1, T2, T3> ToResultsSet<T1, T2, T3>(this IDataReader dbReader)
            where T1 : new()
            where T2 : new()
            where T3 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            return ToResultsSet<T1, T2, T3>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate);
        }

        //4
        public static ResultsSet<T1, T2, T3, T4> ToResultsSet<T1, T2, T3, T4>(this IDataReader dbReader,
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
                    result.Results1 = ExecuteToList(dbReader, translate1);
                    if (dbReader.NextResult())
                    {
                        result.Results2 = ExecuteToList(dbReader, translate2);
                        if (dbReader.NextResult())
                        {
                            result.Results3 = ExecuteToList(dbReader, translate3);
                            if (dbReader.NextResult())
                            {
                                result.Results4 = ExecuteToList(dbReader, translate4);
                            }
                            else
                            {
                                result.Results4 = new List<T4>();
                            }
                        }
                        else
                        {
                            result.Results3 = new List<T3>();
                            result.Results4 = new List<T4>();
                        }
                    }
                    else
                    {
                        result.Results2 = new List<T2>();
                        result.Results3 = new List<T3>();
                        result.Results4 = new List<T4>();
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }
            return result;
        }

        public static ResultsSet<T1, T2, T3, T4> ToResultsSet<T1, T2, T3, T4>(this IDataReader dbReader)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
        {
            var translator1 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T1>();
            var translator2 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T2>();
            var translator3 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T3>();
            var translator4 = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T4>();
            return ToResultsSet<T1, T2, T3, T4>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate);
        }


        // 5 
        public static ResultsSet<T1, T2, T3, T4, T5> ToResultsSet<T1, T2, T3, T4, T5>(this IDataReader dbReader,
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
                    result.Results1 = ExecuteToList(dbReader, translate1);
                    if (dbReader.NextResult())
                    {
                        result.Results2 = ExecuteToList(dbReader, translate2);
                        if (dbReader.NextResult())
                        {
                            result.Results3 = ExecuteToList(dbReader, translate3);
                            if (dbReader.NextResult())
                            {
                                result.Results4 = ExecuteToList(dbReader, translate4);
                                if (dbReader.NextResult())
                                {
                                    result.Results5 = ExecuteToList(dbReader, translate5);
                                }
                                else
                                {
                                    result.Results5 = new List<T5>();
                                }
                            }
                            else
                            {
                                result.Results4 = new List<T4>();
                                result.Results5 = new List<T5>();
                            }
                        }
                        else
                        {
                            result.Results3 = new List<T3>();
                            result.Results4 = new List<T4>();
                            result.Results5 = new List<T5>();
                        }
                    }
                    else
                    {
                        result.Results2 = new List<T2>();
                        result.Results3 = new List<T3>();
                        result.Results4 = new List<T4>();
                        result.Results5 = new List<T5>();
                    }
                }
                finally
                {
                    dbReader.Close();
                }
            }

            return result;
        }

        public static ResultsSet<T1, T2, T3, T4, T5> ToResultsSet<T1, T2, T3, T4, T5>(this IDataReader dbReader)
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
            return ToResultsSet<T1, T2, T3, T4, T5>(dbReader, translator1.Translate, translator2.Translate, translator3.Translate, translator4.Translate, translator5.Translate);
        }
        #endregion

    }
}
