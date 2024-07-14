using CA.Blocks.DataAccess.Model.Paging;

namespace CA.Blocks.DataAccess.Extensions
{
	public static class PagingRequestExtensions
	{
		// sql server
		public static string ToSqlServerPagingOffset(this PagingRequest pr)
		{
			return $"Order by {pr.GetOrderBy()} OFFSET {pr.Skip} Rows FETCH Next {pr.Take} ROWS ONLY;";
		}

		// sqllite, mysql, postgresql
		public static string ToLimitOffset(this PagingRequest pr)
		{
			return $"order by {pr.GetOrderBy()} limit {pr.Take} offset {pr.Skip};";
		}
	}
}
