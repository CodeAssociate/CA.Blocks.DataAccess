using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CA.Blocks.DataAccess.Model.Filter
{
    public enum BaseFilterSegmentCondition
    {
        And,
        Or
    }

    public abstract class BaseFilterSegment
    {
        private readonly BaseFilterSegmentCondition _condition;
        readonly StringBuilder _filter = new StringBuilder();

        protected BaseFilterSegment()
        {
            _condition = BaseFilterSegmentCondition.And;
        }

        protected BaseFilterSegment(BaseFilterSegmentCondition condition)
        {
            _condition = condition;
        }

        protected void AddFilter(string filter)
        {
            if (_filter.Length > 0 )
            {
                _filter.Append($" {_condition} ");
            }
            _filter.Append(filter);
        }

        protected void AddFilter(string filter, IDataParameter sqlparam)
        {
            AddFilter(filter);
            AssignParameterValue(sqlparam);
        }


        protected void AssignParameterValue(IDataParameter sqlparam)
        {
            if (Parameters.All(x => x.ParameterName != sqlparam.ParameterName))
            {
                Parameters.Add(sqlparam);
            }
            else
            {
                var element = Parameters.FirstOrDefault(x => x.ParameterName == sqlparam.ParameterName);
                Debug.Assert(element != null, nameof(element) + " != null");
                if (element.DbType != sqlparam.DbType)
                {
                    throw new ApplicationException(
                        $"The Parameter {sqlparam.ParameterName} has been given two different types {sqlparam.DbType} and {element.DbType}, this is not allowed within a single query");
                }
                if (!element.Value.Equals(sqlparam.Value))
                {
                    throw new ApplicationException(
                        $"The Parameter {sqlparam.ParameterName} has been given two different values {sqlparam.Value} and {element.Value}, this is not allowed within a single query");
                }
            }
        }


        public IList<IDataParameter> Parameters { get; } = new List<IDataParameter>();

        public string ToSQLFilter(bool includeWhere = false)
        {
            if (includeWhere && _filter.Length > 0)
                return $"WHERE {_filter.ToString()}";
            else
                return _filter.ToString();

        }
    }
}
