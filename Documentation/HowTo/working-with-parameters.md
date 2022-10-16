## Working with parameters 
The CA.Blocks.Database allows you to work directly with SQL 
Working with the parameters is one of the key defense against SQL injection attacks. see [SQL Injection attacks](../Design/sql-injection-attacks.md)


### Using parameters

Anything that comes in as a parameter to a function should be turned into SQL parameter for execution. Doing so will provide protection against the sql injection attacks

THe parameters are provided at a provider level they can be used by simply calling the type.ToSqlParameter(sqlParameterName)

parameters are names at the SQL provider level as @ParameterName
Then in code you take the .NET type and call the ToSqlParameter method to assign the .NET parameter to the SQL Parameter

Simple Example adding the Parameter to the command

```C#
    public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
    {
        var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
        var cmd = CreateTextCommand(sql);
        cmd.Parameters.Add(searchTerm.ToSqlParameter("@searchTerm"));
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```
Here we have parameter called @searchTerm to assign the value to command we take the .NET string value searchTerm and call 
```C#
    var sqlParameterValue = searchTerm.ToSqlParameter("@searchTerm")
``` 
This returns sqlParameter that can be added to the SQL Command parameter values. 


Example 2 using the cmd with WithParameter

```C#
    public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
    {
        var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
        var cmd = CreateTextCommand(sql).WithParameter(searchTerm.ToSqlParameter("@searchTerm"));
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```


Out of the box the blocks Support the the base .net value types will null support. So you can call ToSqlParameter on (bool, byte, byte[],  Datetime, string, short, int, long, guid, float, decimal, timeSpan, sbyte) 

<!-- TODO INPUT OUTPUT ANd CUSTOM -->