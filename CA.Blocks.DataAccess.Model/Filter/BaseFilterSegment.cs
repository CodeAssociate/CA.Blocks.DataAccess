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
        // When building a filter and we know it is not valid we need to put in place a filter that will not break the Segment 
        private const string IGNORE_FILTER_STRING = " 1 = 1 ";

        private readonly BaseFilterSegmentCondition _condition;
        private readonly StringBuilder _filter = new StringBuilder();

        protected BaseFilterSegment()
        {
            _condition = BaseFilterSegmentCondition.And;
        }

        protected BaseFilterSegment(BaseFilterSegmentCondition condition)
        {
            _condition = condition;
        }

        public virtual bool IsValid()
        {
            return true;
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
            
            if (IsValid())
            {
                return includeWhere && _filter.Length > 0 ? $"WHERE {_filter}" : _filter.ToString();
            }
            else
            {
                return includeWhere ? $"WHERE {IGNORE_FILTER_STRING}" : IGNORE_FILTER_STRING;
            }
        }
    }
}
