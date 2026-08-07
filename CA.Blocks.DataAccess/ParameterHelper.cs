using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace CA.Blocks.DataAccess
{
    // this class encapsulates common functions working with DbParameters, not all providers implement the IDbDataParameter as there some big differences
    // where we can abstracted some common logic
    public static class ParameterHelper
    {

        /// <summary>
        /// Deals with Null value from the .NET world into DBNull.Value
        /// </summary>
        /// <returns> object to set in the Parameter</returns>
        public static object ToDbParameterValue<T>(T? input) where T : struct
        {
            return input.HasValue ? (object)input : (object)DBNull.Value;
        }
        
        public static object ToDbParameterValue(object? input)
        {
            return input != null ? (object)input : (object)DBNull.Value;
        }


        // Not all providers derive from DbParameter but enough do to make this work


        public static TC WithParameters<TC, TP>(this TC cmd, IEnumerable<TP> parameters)
            where TC : DbCommand
            where TP : DbParameter
        {
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }

        public static TC WithParameter<TC, TP>(this TC cmd, TP parameter) 
            where TC : DbCommand
            where TP : DbParameter
        {
            cmd.Parameters.Add(parameter);
            return cmd;
        }

        public static T AsOutput<T>(this T dbParameter) where T : DbParameter
        {
            dbParameter.Direction = ParameterDirection.Output;
            return dbParameter;
        }

        public static T AsInputOutput<T>(this T dbParameter) where T : DbParameter
        {
            dbParameter.Direction = ParameterDirection.InputOutput;
            return dbParameter;
        }

        public static T? ToValue<TP, T>(this TP dbParameter)  where TP : DbParameter
        {
            var result = default(T);
            if (dbParameter.Direction == ParameterDirection.Output || dbParameter.Direction == ParameterDirection.InputOutput)
            {
                if (dbParameter.Value != null && dbParameter.Value != DBNull.Value)
                    result = (T)dbParameter.Value;
            }
            return result;
        }

        public static T? ToValueWithConvert<TP, T>(this TP dbParameter)  where TP : DbParameter
        {
            var result = default(T);
            if (dbParameter.Direction != ParameterDirection.Output &&  dbParameter.Direction != ParameterDirection.InputOutput) 
                return result;
            if (dbParameter.Value == null || dbParameter.Value == DBNull.Value) 
                return result;
            
            var valueAsString = dbParameter.Value.ToString();
            if (string.IsNullOrEmpty(valueAsString)) 
                return result;
            var o = TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(valueAsString);
            if (o == null) 
                return result;
            result = (T)o;
            return result;
        }


        public static string? PrepStringInput(string? input, bool useEmptyStringForNull, int trimInputTo)
        {
            switch (input)
            {
                case null when useEmptyStringForNull:
                    return string.Empty;
                case null:
                    return null;
                default:
                    return trimInputTo > 0 && input.Length > trimInputTo ? input.Substring(0, trimInputTo) : input;
            }
        }

    }
}
