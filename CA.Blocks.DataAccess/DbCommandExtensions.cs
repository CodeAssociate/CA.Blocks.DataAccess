using System.Data.Common;
using CA.Blocks.DataAccess.Extensions;
using CA.Blocks.DataAccess.Model.Filter;

namespace CA.Blocks.DataAccess
{
	public static class DbCommandExtensions
	{
		public static DbCommand WithFilterParameters(this DbCommand cmd, BaseFilterSegment filter)
		{
			return cmd.WithParameters(filter.ToDbParameters());
		}

	}
}