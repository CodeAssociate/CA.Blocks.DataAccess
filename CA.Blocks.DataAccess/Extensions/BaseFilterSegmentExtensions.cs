using CA.Blocks.DataAccess.Model.Filter;
using System;
using System.Collections.Generic;
using System.Data.Common;


namespace CA.Blocks.DataAccess.Extensions
{
	public static class BaseFilterSegmentExtensions
	{

		// A simple Property copier in the event the diver does not support the ICloneable interface
		private static T Clone<T>(T source)
		{
			var result = (T)Activator.CreateInstance(source.GetType());
			var publicProperties = source.GetType().GetProperties();
			foreach (var property in publicProperties)
			{
				if (property.CanRead && property.CanWrite)
				{
					property.SetValue(result, property.GetValue(source));
				}
			}
			return result;
		}

		public static IList<DbParameter> ToDbParameters(this BaseFilterSegment filter)
		{
			var result = new List<DbParameter>();
			foreach (var dbParameter in filter.Parameters)
			{
				if (dbParameter is ICloneable)
				{
					result.Add((DbParameter)((ICloneable)dbParameter).Clone());
				}
				else
				{
					// Not all drivers support the ICloneable interface on the DbParameter
					// Try a simple object public a copy. 
					if (dbParameter is DbParameter dataParameter)
					{
						
						result.Add(Clone(dataParameter));
					}
					else
					{
						throw new NotSupportedException("Cannot clone DbParameter");
					}
				}
			}
			return result;
		}
	}
}
