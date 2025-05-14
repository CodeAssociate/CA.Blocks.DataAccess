using CA.Blocks.DataAccess;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Concurrent;


namespace CA.Blocks.SqliteDataAccess
{

    public interface IDefaultTypeToSqliteTypeProvider : ITypeToDbTypeProvider<SqliteType>
    {

    }



    public class DefaultTypeToSqliteTypeProvider : IDefaultTypeToSqliteTypeProvider
    {
        private static readonly object _syncLock = new object();
        private readonly ConcurrentDictionary<string, SqliteType> _typeMappings;

        public static IDefaultTypeToSqliteTypeProvider DefaultInstance = new DefaultTypeToSqliteTypeProvider();

        public DefaultTypeToSqliteTypeProvider()
        {
            //TODO Add type mappings for sql lite
        }

        private string GetKeyName(Type type, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{type}" : $"{type}-{byName.ToLower()}";
        }

        public void TryAdd(Type type, SqliteType sqlDbType, string specificType = "", bool errorOnExists = false)
        {
            var key = GetKeyName(type, specificType);

            lock (_syncLock)
            {
                if (!_typeMappings.TryAdd(key, sqlDbType) && errorOnExists)
                {
                    throw new ApplicationException(
                        $"There is already a ITypeConverter Type registered for {key} they must be unique");
                }
            }
        }

        public SqliteType Resolve(Type type, string byName = "")
        {
            var key = GetKeyName(type, byName);
            if (_typeMappings.TryGetValue(key, out var sqlDbType))
            {
                return sqlDbType;
            }

            throw new ApplicationException($"There is no Type Mapping for Type registered for {key}");
        }
    }
}
