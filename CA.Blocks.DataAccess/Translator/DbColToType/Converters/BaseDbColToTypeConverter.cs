using System;
using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    /// <summary>
    /// The abstract col Converter, this is defines the common interface for extracting a Table Cell value to a given type
    /// </summary>
    /// <typeparam name="T"> The Type of data to extract to</typeparam>
    public abstract class BaseDbColToTypeConverter<T> : IDbColToTypeConverter<T>, IDbColToTypeConverter
    {
        /// <summary>
        /// Get a value as specified type from a DataRow specified by column Name
        /// </summary>
        /// <param name="dr"> Target DataRow</param>
        /// <param name="columnName">Name of columnName in Data row</param>
        /// <returns> The Data as Type T</returns>
        public abstract T? GetDataValue(DataRow dr, string columnName);

        /// <summary>
        /// Get a value as specified type from a IDataReader specified by the column Name 
        /// </summary>
        /// <param name="dr"> Target IDataReader</param>
        /// <param name="columnName">Name of the column in the IDataReader  </param>
        /// <returns> The Data as Type T</returns>
        public abstract T? GetDataValue(IDataReader dr, string columnName);

        /// <summary>
        /// Get a value as specified type from a DataRow specified by the column Index 
        /// </summary>
        /// <param name="dr"> Target DataRow</param>
        /// <param name="columnIndex">Index of the column in the DataRow  </param>
        /// <returns> The Data as Type T</returns>
        public abstract T? GetDataValue(DataRow dr, int columnIndex);

        /// <summary>
        /// Get a value as specified type from a IDataReader specified by the column Index 
        /// </summary>
        /// <param name="dr"> Target IDataReader</param>
        /// <param name="columnIndex">Index of the column in the IDataReader  </param>
        /// <returns> The Data as Type T</returns>
        public abstract T? GetDataValue(IDataReader dr, int columnIndex);
        
        /// <summary>
        /// Get a value as as object from a DataRow specified by column Name
        /// </summary>
        /// <param name="dr"> Target DataRow</param>
        /// <param name="columnName">Name of columnName in Data row</param>
        /// <returns> object </returns>
        public object? GetData(DataRow dr, string columnName)
        {
            try
            {
                return GetDataValue(dr, columnName);
            }
            catch (ArgumentException ex)
            {
                throw new ConverterColumnNotFoundException($"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException(
                    $"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnName}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get a value as object from a IDataReader specified by the column Name 
        /// </summary>
        /// <param name="dr"> Target IDataReader</param>
        /// <param name="columnName">Name of the column in the IDataReader  </param>
        /// <returns> object </returns>
        public object? GetData(IDataReader dr, string columnName)
        {
            // A little slow as we get the Ordinal time,  
            try
            {
                return GetDataValue(dr, columnName);
            }
            catch (ArgumentException ex)
            {
                throw new ConverterColumnNotFoundException(
                    $"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException(
                    $"The column '{columnName}' was expected in the result set but not found", ex);
            }
            catch (Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnName}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get a value as object from a DataRow specified by the column Index 
        /// </summary>
        /// <param name="dr"> Target DataRow</param>
        /// <param name="columnIndex">Index of the column in the DataRow  </param>
        /// <returns> object </returns>
        public object? GetData(DataRow dr, int columnIndex)
        {
            try
            {
                
                return GetDataValue(dr, columnIndex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ConverterColumnNotFoundException($"The column in position '{columnIndex}' was expected in the result set but not found", ex);
            }
            catch (Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column in position '{columnIndex}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get a value as object from a IDataReader specified by the column Index 
        /// </summary>
        /// <param name="dr"> Target IDataReader</param>
        /// <param name="columnIndex">Index of the column in the IDataReader  </param>
        /// <returns> object </returns>
        public object? GetData(IDataReader dr, int columnIndex)
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
            catch (Exception ex)
            {
                throw new ConverterColumnBadDataException(
                    $"The column '{columnIndex}' is expecting data for type {typeof(T).FullName} but was not able to convert source data. Convert error was {ex.Message}", ex);
            }
        }
    }
}
