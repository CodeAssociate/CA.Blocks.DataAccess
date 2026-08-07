using System.Collections.Concurrent;
using CA.Blocks.DataAccess;
using NpgsqlTypes;

namespace CA.Blocks.PostgresDataAccess
{
    public interface ITypeToSqlDbTypeProvider : ITypeToDbTypeProvider<NpgsqlDbType>
    {
        
    }

    

    public class DefaultTypeToSqlDbTypeProvider : ITypeToSqlDbTypeProvider
    {
        private static readonly object _syncLock = new object();
        private readonly ConcurrentDictionary<string, NpgsqlDbType> _typeMappings;

        public static readonly ITypeToSqlDbTypeProvider DefaultInstance = new DefaultTypeToSqlDbTypeProvider();

        public DefaultTypeToSqlDbTypeProvider()
        {
            _typeMappings = new ConcurrentDictionary<string, NpgsqlDbType>();

            TryAdd(typeof(long), NpgsqlDbType.Bigint);
            TryAdd(typeof(long[]), NpgsqlDbType.Array | NpgsqlDbType.Bigint);
            TryAdd(typeof(long?), NpgsqlDbType.Bigint);

            
            // binary
            TryAdd(typeof(byte[]), NpgsqlDbType.Bytea);
            // Note postgres does not have variable binary eveything goes into byte array

            // Note Postgress soe not have a byte data type best to use smallint
            // Byte 
            TryAdd(typeof(byte), NpgsqlDbType.Smallint);
            TryAdd(typeof(byte?), NpgsqlDbType.Smallint);

            
            // char
            TryAdd(typeof(char), NpgsqlDbType.Char); 
            TryAdd(typeof(char?), NpgsqlDbType.Char);

            
              
            // bool
            TryAdd(typeof(bool), NpgsqlDbType.Boolean); 
            TryAdd(typeof(bool?), NpgsqlDbType.Boolean); 

            
             
            // Dates
            TryAdd(typeof(DateTime), NpgsqlDbType.Timestamp);
            TryAdd(typeof(DateTime?), NpgsqlDbType.Timestamp);


            // DateTimeOffset  PostgreSQL does not have a direct a type or offset equivalent to SQL Server’s DATETIMEOFFSET type.
            // you need to store this is two columns saving an offest will get converted into UTC time.
            //TryAdd(typeof(DateTimeOffset), NpgsqlDbType.TimestampTz); 
            //TryAdd(typeof(DateTimeOffset?), NpgsqlDbType.TimestampTz); 

#if NET6_0_OR_GREATER
            // DateOnly 
            TryAdd(typeof(DateOnly), NpgsqlDbType.Date); // default 
            TryAdd(typeof(DateOnly?), NpgsqlDbType.Date); // default 

#endif
            
            // Decimal

            TryAdd(typeof(Decimal), NpgsqlDbType.Numeric); // default 
            TryAdd(typeof(Decimal?), NpgsqlDbType.Numeric); // default
            TryAdd(typeof(Decimal), NpgsqlDbType.Money, "money");
            TryAdd(typeof(Decimal?), NpgsqlDbType.Money, "money");


            // Double  
            TryAdd(typeof(Double), NpgsqlDbType.Double); // default 
            TryAdd(typeof(Double?), NpgsqlDbType.Double); // default

            // Guid
            TryAdd(typeof(Guid), NpgsqlDbType.Uuid); // default 
            TryAdd(typeof(Guid?), NpgsqlDbType.Uuid); // default
           
           // Single 
           TryAdd(typeof(Single), NpgsqlDbType.Real); // default 
           TryAdd(typeof(Single?), NpgsqlDbType.Real); // default
           

            // int
            TryAdd(typeof(int), NpgsqlDbType.Integer);
            TryAdd(typeof(int?), NpgsqlDbType.Integer);
            // short
            TryAdd(typeof(short), NpgsqlDbType.Smallint);
            TryAdd(typeof(short?), NpgsqlDbType.Smallint);

            

            // strings
            TryAdd(typeof(string), NpgsqlDbType.Varchar); // default
            TryAdd(typeof(string), NpgsqlDbType.Text, "Text");
            TryAdd(typeof(string), NpgsqlDbType.Char, "Char");
            TryAdd(typeof(string), NpgsqlDbType.Json, "Json");
            TryAdd(typeof(string), NpgsqlDbType.Jsonb, "Jsonb");

            // time 
            TryAdd(typeof(TimeOnly), NpgsqlDbType.Time);
            TryAdd(typeof(TimeOnly?), NpgsqlDbType.Time);

            /*
            // Structured need to be client registered
            // Udt needs to be client registered
            //TimeStamp  ?
            // Variant We ignore..
            // Xml 
            // Json is string.

            // Add more mappings as needed
             */


            
        }



        private string GetKeyName(Type type, string? byName = "")
        {
            return string.IsNullOrWhiteSpace(byName) ? $"{type}" : $"{type}-{byName.ToLower()}";
        }

        public void TryAdd(Type type, NpgsqlDbType sqlDbType, string specificType = "", bool errorOnExists = false)
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

        public NpgsqlDbType Resolve(Type type, string? byName = "")
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
