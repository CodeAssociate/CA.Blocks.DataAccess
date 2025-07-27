using System;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;

namespace CA.Blocks.DataAccess.Translator.Extensions
{
    public static class DataReaderSingleRowExtensions
    {
        public static T ToFirstOrDefault<T>(this IDataReader dbReader, Func<IDataReader, T> translate)
        {
            T result;
            try
            {
                result = DataReaderExtensions.ExecuteToEnumerable(dbReader, translate).FirstOrDefault();
            }
            finally
            {
                dbReader.Close();
            }
            return result;
        }
        public static T ToFirstOrDefault<T>(this IDataReader dbReader)
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToFirstOrDefault(dbReader, translator.Translate);
        }

        public static T ToFirst<T>(this IDataReader dbReader, Func<IDataReader, T> translate)
        {
            T result = ToFirstOrDefault(dbReader, translate);
            if (result == null || result.Equals(default(T)))
            {
                throw new DataException("Expected Single Result,but No row was found");
            }
            return result;
        }

        public static T ToFirst<T>(this IDataReader dbReader)
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToFirst(dbReader, translator.Translate);
        }

        public static T ToSingle<T>(this IDataReader dbReader, Func<IDataReader, T> translate) 
        {

            T result = ToSingleOrDefault(dbReader, translate);
            if (result == null || result.Equals(default(T)))
            {
                throw new DataException("Expected Single Result,but No row was found");
            }
            return result;
        }

        public static T ToSingle<T>(this IDataReader dbReader)
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToSingle(dbReader, translator.Translate);
        }

        public static T ToSingleOrDefault<T>(this IDataReader dbReader, Func<IDataReader, T> translate)
        {
            T result;
            try
            {
                result = DataReaderExtensions.ExecuteToEnumerable(dbReader, translate).FirstOrDefault();
                if (dbReader.Read())
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

        public static T ToSingleOrDefault<T>(this IDataReader dbReader)
        {
            var translator = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>();
            return ToSingleOrDefault(dbReader, translator.Translate);
        }
    }
}