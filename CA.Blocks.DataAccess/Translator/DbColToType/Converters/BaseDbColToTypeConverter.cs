using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

    /// <summary>
    /// What is the name of this 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDbColToTypeConverter<T> : IDbColToTypeConverter<T>, IDbColToTypeConverter
    {
        public abstract T GetDataValue(DataRow dr, string columnName);
        public abstract T GetDataValue(IDataReader dr, string columnName);

        public abstract T GetDataValue(DataRow dr, int columnIndex);
        public abstract T GetDataValue(IDataReader dr, int columnIndex);


        public object GetData(DataRow dr, string columnName)
        {
            try
            {
                return GetDataValue(dr, columnName);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException($"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (System.Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnName}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }

        public object GetData(IDataReader dr, string columnName)
        {
            // A little slow as we get the Ordinal time,  
            try
            {
                return GetDataValue(dr, columnName);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException(
                    $"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (System.Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnName}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }


        public object GetData(DataRow dr, int columnIndex)
        {
            try
            {
                return GetDataValue(dr, columnIndex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException($"The column in position '{columnIndex}' was expected in the result set but not found", ex);
            }
            catch (System.Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column in position '{columnIndex}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }

        public object GetData(IDataReader dr, int columnIndex)
        {
            // A little slow as we get the Ordinal time,  
            try
            {
                return GetDataValue(dr, columnIndex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException(
                    $"The column in position '{columnIndex}' was expected in the result set but not found", ex);
            }
            catch (System.Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnIndex}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }



    }
}
