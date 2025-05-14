#if NET6_0_OR_GREATER
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CA.Blocks.SqliteDataAccess.Builder
{
    public class SafeSqlBuilder
    {
        private StringBuilder _sb = new StringBuilder();
        private List<SqliteParameter> _parameters = [];

        public void AddSql(string sql)
        {
            _sb.Append(sql);
        }

        //public void AddSql(SqlStringHandler builder)
        //{
        //    _sb.Append(builder.GetFormattedText());
        //    var sqlParams = builder.GetParameters();
        //    if (sqlParams.Count > 0)
        //    {
        //        _parameters.AddRange(sqlParams);
        //    }
        //}

        public SqliteCommand BuildSqlCommand()
        {
            var cmd = new SqliteCommand { CommandText = _sb.ToString(), CommandType = CommandType.Text };
            cmd.Parameters.AddRange(_parameters.ToArray());
            return cmd;
        }

        public string GetSqlStatement()
        {
            return _sb.ToString();
        }
        public IList<SqliteParameter> GetParameters()
        {
            return _parameters;
        }
    }
}
#endif
