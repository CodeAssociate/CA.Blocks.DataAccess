### Custom Column Translators  

While the The Row Translators have the responsibility of mapping the Table structure which is rows, columns and cells into the class structure of class, and properties.  The focus on the Row Translators is at the rows level, the reading each row  and doing the conversion. In doing the conversion we repeat the  pattern for the columns within each row. Each column will have a known structured type and will have to get converted into a known property in C#.

The Col To Type Converters which are registered are an the default Converters used with Automatic Mapping.  With the Custom mapping you can choose if you use the default the converts or go direct.

The Converters are high frequency functions can be executed millions of times in normal operation as such they need to balance performance with flexibility in mind whilst dealing with null values

At it core the Row Translators have the responsibility of getting to the cell level, and then they delegate the conversion to the Column Converter.

The best way of Explaining is by why of example:


### The Humble Boolean.
A boolean value can be true or false, or is defined as nullable can be true, false, null.

So let consider we have a cell from a database and we need to convert that to the boolean 
We know that the target is a boolean so we create BoolDbColToTypeConverter we can then ask the converter to convert value from the database into the boolean, what will happen is this will converter will call the  GetDataValue ie
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

The Not null version is the same as the null version except it will Throw Exception If data returned was Null.  The code will resolve the index of the column then then finally deal with the boolean value.

Dealing the the boolean value come down to
1) how to provider stores values. In SQL Sever there is a bit field that can store a boolean, In mySQL a boolean is stored as a Byte and in SQlite a boolean value is simply stored as integer 0 == false and 1 is true.
2) The Data type used for sorting the value, IN SQL server can use a string "True", "False" or 1, 0 this can be stored as a bit, byte, smallint, int or long. 
3) How the code will detect this can convert the result. 

To give the mix between performance and flexibility the blocks will test if the raw value from the data source is already in the target type, so if you are using SQL server and the storage type is a bit and you selecting that value, then the read the data directly, if not then we try the .NET IConvertible method Convert.ToBoolean to get the value ( byte, short, int, long , decimal, float, string ) into a boolean value.  The Blocks stop at this level.  This level is comprehensive and if using a string Culture aware, ie if it sees the value "Wahr" and you running in German it will return "True".  The Default converters do no extent beyond that.  


Some systems can define a Boolean values as a Char. example "Y", "N"  (Yes, NO) or "T","F" (True, False)  or "N", "F" (On, Off) for those systems you will need to provide a Custom Translator 


See CharValueBoolDbColToTypeConverter as a example