using System;
using System.Collections.Generic;
using System.Linq;
using CA.Blocks.DataAccess.Model.Filter;
using CA.Blocks.DataAccess.Model.Paging;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess.Extensions
{
    public static class BaseFilterSegmentExtensions
    {
        public static IList<SqlParameter> ToSqlParameters(this BaseFilterSegment filter)
        {
            return filter.Parameters.Select(p => (SqlParameter)((ICloneable)p).Clone()).ToList();
        }
    }

    public static class SqlCommandExtensions
    {
        public static SqlCommand WithFilterParameters(this SqlCommand cmd, BaseFilterSegment filter)
        {
            return cmd.WithParameters(filter.ToSqlParameters());
        }

        public static SqlCommand WithPagingParameters(this SqlCommand cmd, PagingRequest pr)
        {
            return cmd.WithParameters(pr.ToSqlParameters());
        }
    }

    public static class PagingRequestExtensions
    {
        public static string ToSQLPagingOffset(this PagingRequest pr)
        {
            return $"Order by {pr.GetOrderBy()} OFFSET @PagingSkip Rows FETCH Next @PagingTake ROWS ONLY;";
        }
        public static IList<SqlParameter> ToSqlParameters(this PagingRequest pr)
        {
            return new List<SqlParameter>
            {
                pr.Skip.ToSqlParameter("@PagingSkip"), pr.Take.ToSqlParameter("@PagingTake")
            };
        }
    }


}
