using System;
using System.Collections.Concurrent;
using System.Data;
using CA.Blocks.DataAccess;

namespace CA.Blocks.SQLServerDataAccess
{

    public interface ITypeToSqlDbTypeProvider : ITypeToDbTypeProvider<SqlDbType>
    {
        
    }

    


    public class DefaultTypeToSqlDbTypeProvider : ITypeToSqlDbTypeProvider
    {
        private static readonly object _syncLock = new object();
        private readonly ConcurrentDictionary<string, SqlDbType> _typeMappings;

        public static ITypeToSqlDbTypeProvider DefaultInstance = new DefaultTypeToSqlDbTypeProvider();

        public DefaultTypeToSqlDbTypeProvider()
        {
            _typeMappings = new ConcurrentDictionary<string, SqlDbType>();
            TryAdd(typeof(long), SqlDbType.BigInt);
            TryAdd(typeof(long?), SqlDbType.BigInt);

            // binary
            TryAdd(typeof(byte[]), SqlDbType.VarBinary); // default 
            TryAdd(typeof(byte[]), SqlDbType.Binary, "Binary");
            TryAdd(typeof(byte[]), SqlDbType.Binary, "Image");

            // Byte
            TryAdd(typeof(byte), SqlDbType.TinyInt);
            TryAdd(typeof(byte?), SqlDbType.TinyInt);

            // char
            TryAdd(typeof(char), SqlDbType.NChar); // default 
            TryAdd(typeof(char?), SqlDbType.NChar); // default 
            TryAdd(typeof(char), SqlDbType.Char, "Char"); // default 
            TryAdd(typeof(char?), SqlDbType.Char, "Char"); // default 
            TryAdd(typeof(char), SqlDbType.NChar, "NChar"); // default 
            TryAdd(typeof(char?), SqlDbType.NChar, "NChar"); // default 

            // bool
            TryAdd(typeof(bool), SqlDbType.Bit); // default 
            TryAdd(typeof(bool?), SqlDbType.Bit); // default 

            // Dates
            TryAdd(typeof(DateTime), SqlDbType.DateTime2); // default 
            TryAdd(typeof(DateTime?), SqlDbType.DateTime2); // default 
            TryAdd(typeof(DateTime), SqlDbType.DateTime, "DateTime");
            TryAdd(typeof(DateTime), SqlDbType.DateTime2, "DateTime2");
            TryAdd(typeof(DateTime?), SqlDbType.DateTime, "DateTime");
            TryAdd(typeof(DateTime?), SqlDbType.DateTime2, "DateTime2");
            TryAdd(typeof(DateTime?), SqlDbType.DateTime, "SmallDateTime");
            TryAdd(typeof(DateTime?), SqlDbType.DateTime2, "SmallDateTime");

            // DateTimeOffset 
            TryAdd(typeof(DateTimeOffset), SqlDbType.DateTimeOffset); // default 
            TryAdd(typeof(DateTimeOffset?), SqlDbType.DateTimeOffset); // default 

#if NET6_0_OR_GREATER
            // DateOnly 
            TryAdd(typeof(DateOnly), SqlDbType.Date); // default 
            TryAdd(typeof(DateOnly?), SqlDbType.Date); // default 

#endif

            // Decimal

            TryAdd(typeof(Decimal), SqlDbType.Decimal); // default 
            TryAdd(typeof(Decimal?), SqlDbType.Decimal); // default
            TryAdd(typeof(Decimal), SqlDbType.Money, "Money");
            TryAdd(typeof(Decimal?), SqlDbType.Money, "Money");
            TryAdd(typeof(Decimal), SqlDbType.Money, "SmallMoney");
            TryAdd(typeof(Decimal?), SqlDbType.Money, "SmallMoney");

            

            // Double  
            TryAdd(typeof(Double), SqlDbType.Float); // default 
            TryAdd(typeof(Double?), SqlDbType.Float); // default

            // Guid
            TryAdd(typeof(Guid), SqlDbType.UniqueIdentifier); // default 
            TryAdd(typeof(Guid?), SqlDbType.UniqueIdentifier); // default

            // Single 
            TryAdd(typeof(Single), SqlDbType.Real); // default 
            TryAdd(typeof(Single?), SqlDbType.Real); // default

            // int
            TryAdd(typeof(int), SqlDbType.Int);
            TryAdd(typeof(int?), SqlDbType.Int);
            // short
            TryAdd(typeof(short), SqlDbType.SmallInt);
            TryAdd(typeof(short?), SqlDbType.SmallInt);



            // strings
            TryAdd(typeof(string), SqlDbType.NVarChar); // default
            TryAdd(typeof(string), SqlDbType.NText, "NText");
            TryAdd(typeof(string), SqlDbType.Text, "Text");
            TryAdd(typeof(string), SqlDbType.VarChar, "VarChar");
            TryAdd(typeof(string), SqlDbType.NVarChar, "NVarChar");

#if NET6_0_OR_GREATER
            // time 
            TryAdd(typeof(TimeOnly), SqlDbType.Time);
            TryAdd(typeof(TimeOnly?), SqlDbType.Time);
#endif
            // Structured need to be client registered
            // Udt needs to be client registered
            //TimeStamp  ?
            // Variant We ignore..
            // Xml 
            // Json is string.

            // Add more mappings as needed
        }

        private string GetKeyName(Type type, string byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{type}" : $"{type}-{byName.ToLower()}";
        }

        public void TryAdd(Type type, SqlDbType sqlDbType, string specificType = "", bool errorOnExists = false)
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

        public SqlDbType Resolve(Type type, string byName = "")
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
