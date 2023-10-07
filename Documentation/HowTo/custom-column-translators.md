### Custom Column Translators  

While the Row Translators have the responsibility of mapping the Table structure which is in rows, columns and cells into the class structure with properties.  The focus on the Row Translators is at the row level, reading each row and doing the conversions. In doing the conversion we repeat the pattern for the columns within each row. Each column will have a known structured type and will have to get converted into a known property in C#. Working within the cells is the responsibility of the Column translators.

The Col To Type Converters that are registered are the default Converters used with Automatic Mapping.  With Custom mapping, you can choose if you use the default converts or go direct.

The Converters are high-frequency functions that can be executed millions of times in normal operation as such they need to balance performance with flexibility in mind whilst dealing with null values

At its core, the Row Translators have the responsibility of getting to the cell level, and then they delegate the conversion to the Column Converter.

The best way of Explaining is by way of example:


### The Humble Boolean.
A boolean value can be true or false, or is defined as nullable can be true, false, null. So let's consider we have a cell from a database and we need to convert that to a boolean We know that the target is a boolean so we create BoolDbColToTypeConverter we can then ask the converter to convert a value from the database into the boolean, what will happen is this will converter will call the  GetDataValue ie

``` C#
    public override bool GetDataValue(IDataReader dr, string columnName)
    {
        return dr.AsBool(columnName);
    }
```
This then delegates the logic to DataReader extensions to get value from the data reader Column as a bool 

``` C#
    public static bool AsBool(this IDataReader dr, string colName)
    {
        var val = dr.AsNullBool(colName);
        return ThrowExceptionIfIsNull(val, colName, "bool");
    }

    public static bool? AsNullBool(this IDataReader dr, string colName)
    {
        var columnIndex = dr.GetOrdinal(colName);
        return AsNullBool(dr, columnIndex);
    }

    public static bool? AsNullBool(this IDataReader dr, int columnIndex)
    {
        if (dr.IsDBNull(columnIndex))
            return null;
        else
        {
            var value = dr[columnIndex];
            if (value is bool b)
            {
                return b;
            }
            return Convert.ToBoolean(value);
        }
    }
```

The Not null version is the same as the null version except it will Throw an Exception If the data returned was Null.  The code will resolve the index of the column and then finally deal with the boolean value. Dealing with the boolean value comes down to
1) how to provider stores values. In SQL Server there is a bit field that can store a boolean, In MYSQL a boolean is stored as a Byte and in SQlite a boolean value is simply stored as an integer 0 == false and 1 is true.
2) The Data type used for sorting the value, IN SQL Server, can use a string "True", "False" or 1, 0 this can be stored as a bit, byte, smallint, int or long. 
3) How the code will detect this can convert the result. 

To give the mix between performance and flexibility the blocks will test if the raw value from the data source is already in the target type, so if you are using an SQL server and the storage type is a bit and you selecting that value, then the read the data directly, if not then we try the .NET IConvertible method Convert.ToBoolean to get the value ( byte, short, int, long, decimal, float, string ) into a boolean value.  The Blocks stop at this level.  This level is comprehensive and if using a string Culture aware, ie if it sees the value "Wahr" and you running in German it will return "True".  The Default converters do not extend beyond that.  


Some systems can define Boolean values as a Char. example "Y", "N"  (Yes, NO) or "T", "F" (True, False)  or "N", "F" (On, Off) for those systems you will need to provide a Custom Translator 


See CharValueBoolDbColToTypeConverter as an example