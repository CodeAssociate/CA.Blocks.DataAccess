---
layout: default
title: Samples
nav_exclude: true
---

### Samples 

#### Simple Single Value

In this example, we are going to use the data from the local SQL Server, selecting the metadata from the `sysobjects` table and executing the results into a single return type.  


``` csharp
public class ExampleReadDataSingleValue: SqlServerDataAccess
{
    public ExampleReadDataSingleValue() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    public int GetSysObjectsCount()
    {
        var cmd = CreateTextCommand("Select count(*) from Sysobjects");
        return ExecuteScalarAs<int>(cmd);
    }

    public int? GetValueThatMightBeNull()
    {
        var cmd = CreateTextCommand("Select id from Sysobjects where 1=2"); // zero rows
        return ExecuteScalarAs<int?>(cmd);
    }

    public int? GetValueThatMightBeNull2()
    {
        var cmd = CreateTextCommand("Select null as col"); // 1 row value null
        return ExecuteScalarAs<int?>(cmd);
    }

    public int GetValueThatMustBeConverted()
    {
        var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
        return ExecuteScalarWithConvertAs<int>(cmd);
    }
}


```

There are four cases when executing a command to a scalar value:
1. When you execute the command and know there will be one and only one value of a known type. If you know you are working with an `int`, you can use:
```
ExecuteScalarAs<int> 
```
Or if you know you are working with `DateTime`, you can use:
```
ExecuteScalarAs<DateTime>
```

2. Where you execute and the database returns zero rows. In this case, you will get the default value of `T`. So `int?` will be `null`, however `int` will be `0`. 
3. Where you execute and the database returns a null value. In this case, you will get the default value of `T`. So `int?` will be `null`, however `int` will be `0`. 
4. When you know the value is one type in the database and you need to convert it to another value. For example, going from `tinyint` to `int` you can use:
```
ExecuteScalarWithConvertAs<int>
```
When doing the conversion you will need to deal with conversion errors, i.e., `tinyint` to `int` will always work, but `int` to `byte` will not always work.


