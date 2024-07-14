using CA.Blocks.DataAccess.Model.Filter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CA.Blocks.DataAccess.Extensions
{
	public static class BaseFilterSegmentExtensions
	{
		public static IList<DbParameter> ToDbParameters(this BaseFilterSegment filter)
		{
			return filter.Parameters.Select(p => p as DbParameter).ToList();
		}
	}
}
