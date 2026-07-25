---
layout: default
title: Samples
nav_exclude: true
---

### Samples 

#### Simple Single Row

In this example, we are going to use the data from the local SQL Server, selecting the metadata from the `sysobjects` table and executing the results into the .NET class called `ExampleSysObject2`.

Under the hood, the execution path will ask SQL Server to execute the result into a `DataRow`. This will only work if there is zero or one row. If there are multiple rows returned, the blocks by design will throw a `DataException`. The premise behind this is that the blocks are not going to take the "first or default" as the answer. If you want the first, then you can ask SQL to return `TOP 1` only. If you are querying by primary or alternate key, there should be one result at most.     


The Target Class Note in this example we have matched the name of the class to the names in SQL.
``` csharp
public class ExampleSysObject2
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime refdate { get; set; }
}
```


``` csharp
public class ExampleReadDataSingleRow : SqlServerDataAccess
{
    public ExampleReadDataSingleRow() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    public ExampleSysObject2 GetSysObjectByName()
    {
        var cmd = CreateTextCommand("Select id, name, refdate  from Sysobjects where name = 'sysobjects'");
        return ExecuteTo<ExampleSysObject2>(cmd);
    }

    public ExampleSysObject2 GetSysObjectByName2()
    {
        var cmd = CreateTextCommand("Select * from Sysobjects where name = 'sysobjects'");
        return ExecuteTo<ExampleSysObject2>(cmd);
    }
}

```

As we know the name is an alternate key and unique to the table, we can query by name, then execute the result into a single object. In the example above, we have also shown that if you use the name in the C# class to match the SQL names, you can use the `SELECT *` notation. Although this does work, it is recommended to use named columns such that you are not transferring unneeded data.  

The following method will raise a DataException error as there will be more than one row in the table that match the filter.
``` csharp
public ExampleSysObject2 GetSysObjectByName2()
{
    var cmd = CreateTextCommand("Select * from Sysobjects");
    return ExecuteTo<ExampleSysObject2>(cmd);
}
```

The following method will return null as there are no records where 1=2
``` csharp
public ExampleSysObject2 GetSysObjectByName2()
{
    var cmd = CreateTextCommand("Select * from Sysobjects where 1=2");
    return ExecuteTo<ExampleSysObject2>(cmd);
}
```
 