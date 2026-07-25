---
layout: default
title: howto
nav_exclude: true
---

## Working with parameters 
The CA.Blocks.DataAccess allows you to work directly with SQL 
Working with parameters is one of the key defenses against SQL injection attacks, see [SQL Injection attacks](../architecture/sql-injection-attacks.md).


### Using parameters

Anything that comes in as a parameter to a function should be turned into a SQL parameter for execution. Doing so will protect against SQL injection attacks.

The parameters are provided at a provider level they can be used by simply calling the type.ToSqlParameter(sqlParameterName)

Parameters are named at the SQL provider level as `@ParameterName`.
Then in your code, you take the .NET type and call the `ToSqlParameter` method to assign the .NET parameter to the SQL parameter.

Below is a simple example of adding the `searchTerm` parameter to the command:

```C#
    public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
    {
        var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
        var cmd = CreateTextCommand(sql);
        cmd.Parameters.Add(searchTerm.ToSqlParameter("@searchTerm"));
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```
Given the above, in the SQL we specify the parameters by name, i.e., `Where Name like @searchTerm`. To assign a value to `@searchTerm` in the SQL, we do this via the command; we take the .NET string value `searchTerm` and call:
```C#
    var sqlParameterValue = searchTerm.ToSqlParameter("@searchTerm")
``` 
This returns a SqlParameter that can be added to the SQL Command parameter values. 


Example 2 using the cmd with WithParameter

```C#
    public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
    {
        var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
        var cmd = CreateTextCommand(sql).WithParameter(searchTerm.ToSqlParameter("@searchTerm"));
        return Execute(cmd).ToListOf<ProductSummary>();
    }
```

Out of the box, the blocks support the base .net value types with null support. So you can call ToSqlParameter on (bool, byte, byte[],  Datetime, string, short, int, long, guid, float, decimal, timeSpan, sbyte) 

<!-- TODO INPUT OUTPUT ANd CUSTOM -->