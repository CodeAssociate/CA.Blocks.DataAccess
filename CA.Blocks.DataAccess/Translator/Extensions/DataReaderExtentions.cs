using System;
using System.Collections.Generic;
using System.Data;
using CA.Blocks.DataAccess.Model.Results;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;

namespace CA.Blocks.DataAccess.Translator.Extensions
{
    public static class DataReaderExtensions
    {
        private static IList<T> ExecuteToList<T>(IDataReader dbReader, Func<IDataReader, T> translate) where T : new()
        {
            IList<T> result = new List<T>();
            while (dbReader.Read())
            {
                result.Add(translate(dbReader));
            }
            return result;
        }

        public static IList<T> ToListOf<T>(this IDataReader dbReader, Func<IDataReader, T> translate) where T : new()
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
        
        #endregion

    }
}
