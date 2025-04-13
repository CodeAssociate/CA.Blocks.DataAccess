//using System;
//using System.Collections.Concurrent;
//using System.Data;

//namespace CA.Blocks.SQLServerDataAccessUnitTests;

//public interface ITypeToSqlDbTypeProvider
//{
//    void TryAdd(Type type, SqlDbType sqlDbType, string specificType = "", bool errorOnExists = false);
//    SqlDbType Resolve(Type type, string byName = "");

//    /*
//    IDbColToTypeConverter Resolve<T>(string byName = "");
//    IDbColToTypeConverter Resolve(Type targetType, string byName = "");
//    void Add<T>(IDbColToTypeConverter<T> typeConverter, string byName = "");
//    void TryAdd<T>(IDbColToTypeConverter<T> typeConverter, string byName = "", bool errorOnExists = false);

//    */
//}

//public class DefaultTypeToSqlDbTypeProvider : ITypeToSqlDbTypeProvider
//{
//    private static readonly object _syncLock = new object();
//    private readonly ConcurrentDictionary<string, SqlDbType> _typeMappings;

//    public static ITypeToSqlDbTypeProvider DefaultInstance = new DefaultTypeToSqlDbTypeProvider();

//    public DefaultTypeToSqlDbTypeProvider()
//    {
//        _typeMappings = new ConcurrentDictionary<string, SqlDbType>();
//        TryAdd(typeof(long), SqlDbType.BigInt);
//        TryAdd(typeof(long?), SqlDbType.BigInt);

//        // binary
//        TryAdd(typeof(byte[]), SqlDbType.VarBinary); // default 
//        TryAdd(typeof(byte[]), SqlDbType.Binary, "Binary");

//        // char
//        TryAdd(typeof(char), SqlDbType.NChar); // default 
//        TryAdd(typeof(char?), SqlDbType.NChar); // default 
//        TryAdd(typeof(char), SqlDbType.Char, "Char"); // default 
//        TryAdd(typeof(char?), SqlDbType.Char, "Char"); // default 
//        TryAdd(typeof(char), SqlDbType.NChar, "NChar"); // default 
//        TryAdd(typeof(char?), SqlDbType.NChar, "NChar"); // default 

//        // bool
//        TryAdd(typeof(bool), SqlDbType.Bit);// default 
//        TryAdd(typeof(bool?), SqlDbType.Bit);// default 

//        // Dates
//        TryAdd(typeof(DateTime), SqlDbType.DateTime2); // default 
//        TryAdd(typeof(DateTime?), SqlDbType.DateTime2); // default 
//        TryAdd(typeof(DateTime), SqlDbType.DateTime, "DateTime");  
//        TryAdd(typeof(DateTime), SqlDbType.DateTime2, "DateTime2");  
//        TryAdd(typeof(DateTime?), SqlDbType.DateTime, "DateTime");  
//        TryAdd(typeof(DateTime?), SqlDbType.DateTime2, "DateTime2");
//        TryAdd(typeof(DateTime?), SqlDbType.DateTime, "SmallDateTime");
//        TryAdd(typeof(DateTime?), SqlDbType.DateTime2, "SmallDateTime");

//        // ints


//        // strings
//        TryAdd(typeof(string), SqlDbType.NVarChar); // default
//        TryAdd(typeof(string), SqlDbType.NText, "NText");
//        TryAdd(typeof(string), SqlDbType.Text, "Text");
//        TryAdd(typeof(string), SqlDbType.VarChar, "VarChar");
//        TryAdd(typeof(string), SqlDbType.NVarChar, "NVarChar");
        

///*

//   /// <summary>
//   /// <see cref="T:System.Decimal" />. A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1.</summary>
//   Decimal = 5,
//   /// <summary>
//   /// <see cref="T:System.Double" />. A floating point number within the range of -1.79E +308 through 1.79E +308.</summary>
//   Float = 6,
//   /// <summary>
//   /// <see cref="T:System.Array" /> of type <see cref="T:System.Byte" />. A variable-length stream of binary data ranging from 0 to 2 31 -1 (or 2,147,483,647) bytes.</summary>
//   Image = 7,
//   /// <summary>
//   /// <see cref="T:System.Int32" />. A 32-bit signed integer.</summary>
//   Int = 8,
//   /// <summary>
//   /// <see cref="T:System.Decimal" />. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808) to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth of a currency unit.</summary>
//   Money = 9,

//   /// <summary>
//   /// <see cref="T:System.Single" />. A floating point number within the range of -3.40E +38 through 3.40E +38.</summary>
//   Real = 13, // 0x0000000D
//   /// <summary>
//   /// <see cref="T:System.Guid" />. A globally unique identifier (or GUID).</summary>
//   UniqueIdentifier = 14, // 0x0000000E
//   /// <summary>

//   /// <summary>
//   /// <see cref="T:System.Int16" />. A 16-bit signed integer.</summary>
//   SmallInt = 16, // 0x00000010
//   /// <summary>
//   /// <see cref="T:System.Decimal" />. A currency value ranging from -214,748.3648 to +214,748.3647 with an accuracy to a ten-thousandth of a currency unit.</summary>
//   SmallMoney = 17, // 0x00000011

//   /// <summary>
//   /// <see cref="T:System.Array" /> of type <see cref="T:System.Byte" />. Automatically generated binary numbers, which are guaranteed to be unique within a database. <see langword="timestamp" /> is used typically as a mechanism for version-stamping table rows. The storage size is 8 bytes.</summary>
//   Timestamp = 19, // 0x00000013
//   /// <summary>
//   /// <see cref="T:System.Byte" />. An 8-bit unsigned integer.</summary>
//   TinyInt = 20, // 0x00000014
//   /// <summary>

//   /// <summary>
//   /// <see cref="T:System.String" />. A variable-length stream of non-Unicode characters ranging between 1 and 8,000 characters. Use <see cref="F:System.Data.SqlDbType.VarChar" /> when the database column is <see langword="varchar(max)" />.</summary>
//   /// <summary>
//   /// <see cref="T:System.Object" />. A special data type that can contain numeric, string, binary, or date data as well as the SQL Server values Empty and Null, which is assumed if no other type is declared.</summary>
//   Variant = 23, // 0x00000017
//   /// <summary>An XML value. Obtain the XML as a string using the <see cref="M:System.Data.SqlClient.SqlDataReader.GetValue(System.Int32)" /> method or <see cref="P:System.Data.SqlTypes.SqlXml.Value" /> property, or as an <see cref="T:System.Xml.XmlReader" /> by calling the <see cref="M:System.Data.SqlTypes.SqlXml.CreateReader" /> method.</summary>
//   Xml = 25, // 0x00000019
//   /// <summary>A SQL Server user-defined type (UDT).</summary>
//   Udt = 29, // 0x0000001D
//   /// <summary>A special data type for specifying structured data contained in table-valued parameters.</summary>
//   Structured = 30, // 0x0000001E
//   /// <summary>Date data ranging in value from January 1,1 AD through December 31, 9999 AD.</summary>
//   Date = 31, // 0x0000001F
//   /// <summary>Time data based on a 24-hour clock. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL Server <see langword="time" /> value.</summary>
//   Time = 32, // 0x00000020
//   /// <summary>Date and time data. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds.</summary>
//   DateTime2 = 33, // 0x00000021
//   /// <summary>Date and time data with time zone awareness. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range is -14:00 through +14:00.</summary>
//   DateTimeOffset = 34, // 0x00000022
//   /// <summary>A JSON value.</summary>
//   Json = 35, // 0x00000023
// */

//        // Add more mappings as needed
//    }

//    private string GetKeyName(Type type, string byName = "")
//    {
//        return string.IsNullOrWhiteSpace(byName) ? $"{type}" : $"{type}-{byName.ToLower()}";
//    }

//    public void TryAdd(Type type, SqlDbType sqlDbType, string specificType = "", bool errorOnExists = false)
//    {
//        var key = GetKeyName(type, specificType);

//        lock (_syncLock)
//        {
//            if (!_typeMappings.TryAdd(key, sqlDbType) && errorOnExists)
//            {
//                throw new ApplicationException($"There is already a ITypeConverter Type registered for {key} they must be unique");
//            }
//        }
//    }

//    public SqlDbType Resolve(Type type, string byName = "")
//    {
//        var key = GetKeyName(type, byName);
//        if (_typeMappings.TryGetValue(key, out var sqlDbType))
//        {
//            return sqlDbType;
//        }
//        throw new ApplicationException($"There is no Type Mapping for Type registered for {key}");
//    }

//}
