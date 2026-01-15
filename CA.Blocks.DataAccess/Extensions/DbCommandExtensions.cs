using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using CA.Blocks.DataAccess.Model.Filter;

namespace CA.Blocks.DataAccess.Extensions
{
    public static class DbCommandExtensions
    {
        public static DbCommand WithFilterParameters(this DbCommand cmd, BaseFilterSegment filter)
        {
            return cmd.WithParameters(filter.ToDbParameters());
        }

        public static DbCommand WithParameters(this DbCommand cmd, IEnumerable<DbParameter> parameters)
        {
            return cmd.WithParameters<DbCommand, DbParameter>(parameters);
        }

        public static DbCommand WithParameter(this DbCommand cmd, DbParameter parameter)
        {
            return cmd.WithParameter<DbCommand, DbParameter>(parameter);
        }

        public static DbCommand WithReturnResult(this DbCommand cmd)
        {
            var sqlParam = cmd.CreateParameter();
            sqlParam.ParameterName = "Return";
            sqlParam.DbType = DbType.Int32;
            sqlParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(sqlParam);
            return cmd;
        }

        public static int? GetReturnResult(this DbCommand cmd)
        {
            int? result = null;
            var sqlParam = cmd.Parameters["Return"];
            if (sqlParam.DbType == DbType.Int32 && sqlParam.Direction == ParameterDirection.ReturnValue)
            {
                if (sqlParam.Value != null && sqlParam.Value != DBNull.Value)
                    result = (int)sqlParam.Value;
            }
            return result;
        }
    }
}