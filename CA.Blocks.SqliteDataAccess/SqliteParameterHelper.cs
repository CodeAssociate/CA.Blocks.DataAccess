using System;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SqliteDataAccess
{
	public static class SqliteParameterHelper
	{
		public static SqliteType GetDefaultStorageTypeFor(Type type)
		{
			if (type == null)
			{
				return SqliteType.Text;
			}

			if (Nullable.GetUnderlyingType(type) != null)
			{
				type = Nullable.GetUnderlyingType(type);
			}

			// deal with null types
			if (type == typeof(byte) ||
			    type == typeof(short) ||
			    type == typeof(int) ||
			    type == typeof(long) ||
			    type == typeof(bool) || 
				type == typeof(sbyte) ||
			    type == typeof(ushort) ||
			    type == typeof(uint) ||
			    type == typeof(ulong)
			   )
			{
				return SqliteType.Integer;
			}
			if (type == typeof(Single) ||
			    type == typeof(Double)
			   )
			{
				return SqliteType.Real;
			}
			return type.FullName == "System.Byte[]" ? SqliteType.Blob : SqliteType.Text;
		}
	}
}