---
layout: default
title: Custom Column Translators
description: "Custom Column Translators
parent: How To
nav_order: 9
---

{: .no_toc .text-delta }
## Custom Column Translators Overview

{:toc_levels="3"}

1. TOC
{:toc}

---

While the row translators have the responsibility of mapping the table structure, which consists of rows, columns, and cells, into the class structure with properties, the focus of the row translators is at the row level—reading each row and performing the conversions. In doing the conversion, we repeat the pattern for the columns within each row. Each column will have a known structured type and will have to be converted into a known property in C#. Working within the cells is the responsibility of the column translators.

The column to type converters that are registered are the default converters used with automatic mapping. With custom mapping, you can choose whether to use the default converters or go direct.

The Converters are high-frequency functions that can be executed millions of times in normal operation as such they need to balance performance with flexibility in mind whilst dealing with null values

At its core, the row translators have the responsibility of getting to the cell level, and then they delegate the conversion to the column converter.

The best way of explaining is by way of example:


### The Humble Boolean.
A boolean value can be true or false, or if defined as nullable, can be true, false, or null. So let's consider we have a cell from a database and we need to convert that to a boolean. We know that the target is a boolean, so we create a `BoolDbColToTypeConverter`; we can then ask the converter to convert a value from the database into the boolean. What will happen is this converter will call `GetDataValue`, i.e.:

```csharp
    public override bool GetDataValue(IDataReader dr, string columnName)
    {
        return dr.AsBool(columnName);
    }
```
This then delegates the logic to `DataReader` extensions to get the value from the data reader column as a bool:

```csharp
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

The non-nullable version is the same as the nullable version except it will throw an exception if the data returned was null. The code will resolve the index of the column and then finally deal with the boolean value. Dealing with the boolean value comes down to:
1) How the provider stores values. In SQL Server, there is a `bit` field that can store a boolean. In MySQL, a boolean is stored as a `byte`, and in SQLite, a boolean value is simply stored as an integer (0 == false and 1 == true).
2) The data type used for storing the value. In SQL Server, we can use a string "True", "False" or 1, 0; this can be stored as a `bit`, `byte`, `smallint`, `int`, or `bigint`. 
3) How the code will detect and convert the result. 

To balance performance and flexibility, the blocks will test if the raw value from the data source is already in the target type. So if you are using SQL Server and the storage type is a `bit` and you are selecting that value, then we read the data directly. If not, then we try the .NET `IConvertible` method `Convert.ToBoolean` to get the value (byte, short, int, long, decimal, float, string) into a boolean value. The blocks stop at this level. This level is comprehensive and, if using a string, culture-aware; i.e., if it sees the value "Wahr" and you are running in German, it will return "True". The default converters do not extend beyond that.  


Some systems can define boolean values as a `char`. Example: "Y", "N" (Yes, No) or "T", "F" (True, False). For those systems, you will need to provide a custom translator.


See CharValueBoolDbColToTypeConverter as an example