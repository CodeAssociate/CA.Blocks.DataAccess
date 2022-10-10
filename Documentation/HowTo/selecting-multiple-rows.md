## Selecting Multiple Rows

These methods the most frequently used methods within the package. The provide the ability to get data coming from a query as a row set into .NET class structure.   

| Method      | Description |
| ----------- | ----------- |
| ExecuteToListOf\<T>(cmd)   | This is alias to Execute(cmd).ToListOf\<T>  |
| [Execute(cmd).ToListOf\<T>](#executecmdtolistofltt)   | executes a data reader and passed conversion to ToListOf<> |
| [Execute(cmd).ToSingleNamedColumnList\<T>](#executecmdtosinglenamedcolumnlistt)| executes a single Column to a List of values  |
| [ExecuteReader(cmd)](#executereadercmd)  | executes and returns the raw data reader |
| [DataTable(cmd)](#datatablecmd)  | executes and returns a DataTable  |
| [ExecuteObjectList(cmd)](#executeobjectlistcmd)  | executes and returns a DataTable  |


<div style="text-align:center;">
<span style="min-height:128px;display:inline-flex;align-items:center;border: 1px solid aqua;background-color:white;" >
    <img src="../_assets/table.svg" alt="Table" width=128 /> 
    ==➤
    <img src="../_assets/class.svg" alt="Table" width=128 />
</span>
</div>

### ExecuteToListOf\<T>(cmd) 
This is alias to Execute(cmd).ToListOf\<T>  
```C#
     ExecuteToListOf<ProductSummary>(cmd);
     // is the same as 
     Execute(cmd).ToListOf<ProductSummary>();
```


### Execute(cmd).ToListOf\<T>
The ExecuteTo will get the First or default of Type T.  This is one of the  most common methods as reading from a table and returning a Types IList of \<T>

The following is an example executing the result of a query from the Adventure works Schema into the ProductSummary class.
```C#
    public IList<ProductSummary> GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```

The following is an example executing the result of a query from the Adventure works Schema into the ProductSummary class. using the Async Method
```C#
        public Task<IList<ProductSummary>> GetAllProductSummaryAsync()
        {
            var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
            var cmd = CreateTextCommand(sql);
            return ExecuteAsync(cmd).ToListOf<ProductSummary>();
        }
```


Any data that comes back as a Data reader from can be can executed into List.
For Example the code below below would execute the result of the sp_who SQL server stored procedure into class SpWhoResult

```C#
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
There are come times you only need to select a single Column value and what to pass that into e list of values. In This case you can use the Execute(cmd).ToSingleNamedColumnList\<T> passing in the type for T and the name of the Column.

For Example we can get a list of all the product Names from the Production.Product table and return that as a list of strings

```C#
    public IList<string> GetAllProductNames()
    {
        var sql = "Select Name From [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToSingleNamedColumnList<string>("Name");
    }
```

### ExecuteReader(cmd)

The Execute Reader will return a Open IDataReader, this will expose the raw IDataReader. When executing this method the calling code will be responsible for closing the reader when done.  


### DataTable(cmd)

The Execute DataTable is useful for getting a offline version of the data table from the server. This method is executed using the DbDataAdapter which has been around since .NET 1. It has been well tested however many of the main providers treat this as maintenance only code. The big disadvantage of this is there is no Native Async Support.  So this is one of the methods in the Blocks that does not  have a Async option. Also be very wary of and async versions of the DbDataAdapter as type typically hide an synchronous call in a asynchronous method leading to race conditions.

```C#
    public DataTable GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return DataTable(cmd);
    }
```

### ExecuteObjectList(cmd)
The ExecuteObjectList is a way of executing the result to a list of expando objects. This is very convenient for rapid prototyping but not recommended any anything beyond that.    

```C#
    public IList<dynamic> GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return ExecuteObjectList(cmd);
    }
```