---
layout: default
title: Select To Lists
description: "Select To Lists"
parent: How to
nav_order: 3
---

## Selecting Multiple Rows

These methods are the most frequently used methods within the package. They provide the ability to get data coming from a query as a row set into dotNET class structure.   

| Method      | Description |
| ----------- | ----------- |
| ExecuteToListOf<T>(cmd)   | This is an alias for Execute(cmd).ToListOf<T>  |
| [Execute(cmd).ToListOf<T>](#executecmdtolistofltt)   | Executes a data reader and passes conversion to `ToListOf<T>` |
| [Execute(cmd).ToSingleNamedColumnList\<T>](#executecmdtosinglenamedcolumnlistt)| executes a single Column to a List of values  |
| [ExecuteReader(cmd)](#executereadercmd)  | executes and returns the raw data reader |
| [DataTable(cmd)](#datatablecmd)  | executes and returns a DataTable  |
| [ExecuteObjectList(cmd)](#executeobjectlistcmd)  | executes and returns a DataTable  |


### ExecuteToListOf<T>(cmd) 
This is an alias for `Execute(cmd).ToListOf<T>`
```csharp
     ExecuteToListOf<ProductSummary>(cmd);
     // is the same as 
     Execute(cmd).ToListOf<ProductSummary>();
```

### Execute(cmd).ToListOf<T>
The `ToListOf<T>` will get a typed `IList<T>` by reading from a table.

The following is an example of executing the result of a query from the Adventure works Schema into the ProductSummary class.
```csharp
    public IList<ProductSummary> GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```

The following is an example of executing the result of a query from the Adventure works Schema into the ProductSummary class. using the Async Method
```csharp
        public Task<IList<ProductSummary>> GetAllProductSummaryAsync()
        {
            var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
            var cmd = CreateTextCommand(sql);
            return ExecuteAsync(cmd).ToListOf<ProductSummary>();
        }
```


Any data that comes back as a Data reader can be executed into List. For Example, the code below would execute the result of the sp_who SQL server stored procedure into class SpWhoResult

```csharp
    public class SpWhoResult
    {
        public short spid { get; init; }
        public short ecid { get; init; }
        public string status { get; init; }
        public string loginame { get; init; }
        public string hostname { get; init; }
        public string blk { get; init; }
        public string dbname { get; init; }
        public string cmd { get; init; }
        public int request_id { get; init; }
    }

    public IList<SpWhoResult> ExecSpWho()
    {
        var cmd = CreateStoredProcedureCommand("sp_Who");
        return Execute(cmd).ToListOf<SpWhoResult>();
    }
```


### Execute(cmd).ToSingleNamedColumnList\<T>
There are some times you only need to select a single column value and want to pass that into a list of values. In this case, you can use `Execute(cmd).ToSingleNamedColumnList<T>` passing in the type for `T` and the name of the column. For example, we can get a list of all the product names from the `[Production].[Product]` table and return that as a list of strings:

```csharp
    public IList<string> GetAllProductNames()
    {
        var sql = "Select Name From [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToSingleNamedColumnList<string>("Name");
    }
```

### ExecuteReader(cmd)

The Execute Reader will return an Open IDataReader, this will expose the raw IDataReader. When executing this method the calling code will be responsible for closing the reader when done.  


### DataTable(cmd)

The Execute DataTable is useful for getting an offline version of the data table from the server. This method is executed using the DbDataAdapter which has been around since .NET 1. It has been well tested however many of the main providers treat this as maintenance-only code. The big disadvantage of this is there is no Native Async Support.  As such,  this is one of the methods in the Blocks that does not have an Async option. Whilst some components say they support async for DbDataAdapter be wary as most async versions of the DbDataAdapter typically hide a synchronous call in an asynchronous method leading to blocking and race conditions.

```csharp
    public DataTable GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return DataTable(cmd);
    }
```

### ExecuteObjectList(cmd)
The ExecuteObjectList is a way of executing the result to a list of expando objects. This is very convenient for rapid prototyping but not recommended for anything beyond that.    

```csharp
    public IList<dynamic> GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return ExecuteObjectList(cmd);
    }
```