#if NET6_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CA.Blocks.DataAccess.Builders
{
    public abstract class SqlStringBuilder<SP>()
        where SP : class, IDataParameter
    {
        private StringBuilder _sqlsb = new StringBuilder();
        private List<SP> _parameters = new List<SP>();


        public abstract SP CreateNewParameterFor(Type t, string name, string targetDbType);

        public void AppendSql(string sql)
        {
            _sqlsb.Append(sql);
        }

        public void AppendSqlParameter<T>(T p)
        {
            var sqlParam = p as SP;
            if (sqlParam == null)
            {
                throw new ArgumentException("The parameter must be a IDataParameter to use it directly");
            }

            _sqlsb.Append(sqlParam.ParameterName);
            _parameters.Add(sqlParam);
        }

        public void AppendNewSqlParameter<T>(T t, string format)
        {
            string paramName = format;
            var targetDbType = string.Empty;
            if (format.Contains('|'))
            {
                var formatParts = format.Split('|');
                paramName = formatParts[0];
                targetDbType = formatParts[1];
            }

            _sqlsb.Append(paramName);
            var dataParam = CreateNewParameterFor(typeof(T), paramName, targetDbType);
            dataParam.Value = ParameterHelper.ToDbParameterValue(t);

            _parameters.Add(dataParam);
        }

        public string ToSqlStatement()
        {
            return _sqlsb.ToString();
        }

        public List<SP> GetParameters()
        {
            return _parameters;
        }
    }
}
#endif