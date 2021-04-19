using System;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{

    /// <summary>
    /// EnumDbColToTypeConverter Docs 
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class EnumDbColToTypeConverter<T> : BaseDbColToTypeConverter<T> where T : struct
    {
        private readonly bool _ignoreCase;
        public EnumDbColToTypeConverter(bool ignoreCase = true)
        {
            _ignoreCase = ignoreCase;
        }

        public override T GetDataValue(DataRow dr, string columnName)
        {
            string value = dr.AsString(columnName);
            Enum.TryParse<T>(value, _ignoreCase, out var result);
            return result;
        }

        public override T GetDataValue(IDataReader dr, string columnName)
        {
            string value = dr.AsString(columnName);
            Enum.TryParse<T>(value, _ignoreCase, out var result);
            return result;
        }
    }

    public class NullEnumDbColToTypeConverter<T> : BaseDbColToTypeConverter<T?> where T : struct
    {
        private readonly bool _ignoreCase;
        public NullEnumDbColToTypeConverter(bool ignoreCase = true)
        {
            _ignoreCase = ignoreCase;
        }


        public override T? GetDataValue(DataRow dr, string columnName)
        {
            var value = dr.AsString(columnName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                Enum.TryParse<T>(value, _ignoreCase, out var result);
                return result;
            }
        }

        public override T? GetDataValue(IDataReader dr, string columnName)
        {
            var value = dr.AsString(columnName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                Enum.TryParse<T>(value, _ignoreCase, out var result);
                return result;
            }
        }
    }
}
