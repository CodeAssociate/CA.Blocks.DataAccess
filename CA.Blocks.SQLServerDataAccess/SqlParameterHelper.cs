using System;
using System.Collections.Generic;
using CA.Blocks.DataAccess;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess
{
    public static class SqlParameterHelper
    {
        public static SqlParameter CreateNewParameterFor(Type t, string name, string targetDbType = "")
        {
            return new SqlParameter
            { 
                ParameterName = name,
                SqlDbType = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(t, targetDbType)
            };
        }

        public static SqlParameter CreateNewParameterFor<T>(T t, string name, string targetDbType= "") where T : notnull
        {
            return new SqlParameter
            { 
                ParameterName = name.StartsWith("@") ? name : $"@{name}", 
                SqlDbType = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(typeof(T), targetDbType), 
                Value = ParameterHelper.ToDbParameterValue(t)
            };
        }

        // Used to reduce verbosity of the ToSqlParameter
        public static List<SqlParameter> AsSqlParameters(params ParameterMap[] sourceParams)
        {
            var result = new List<SqlParameter>();
            foreach (var item in sourceParams)
            {
                var sqlParam =
                    SqlParameterHelper.CreateNewParameterFor(item.Type, item.ParameterName, item.SpecificType);
                sqlParam.Value = ParameterHelper.ToDbParameterValue(item.Value);

                result.Add(sqlParam);
            }
            return result;
        }
    }


    
#if NET6_0_OR_GREATER
    public class ParameterMap(object value, string parameterName, string specificType = "")
    {
        public Type Type { get; } = value.GetType();
        public object Value { get; } = value;
        public string ParameterName { get; } = parameterName.StartsWith('@') ? parameterName : $"@{parameterName}";
        public string SpecificType { get; } = specificType;
    }
#else
    public class ParameterMap
    {
        public ParameterMap(object value, string parameterName, string specificType = "")
        {
            Value = value;
            Type = value.GetType();
            ParameterName = parameterName.StartsWith("@") ? parameterName : $"@{parameterName}";
            SpecificType = specificType;
        }

        public Type Type { get; private set; } 
        public object Value { get; private set; }
        public string ParameterName { get; private set; }
        public string SpecificType { get; private set; }
    }
#endif
}