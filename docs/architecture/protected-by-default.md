---
layout: default
title: Protected By Default
parent: Architecture
nav_order: 2
---

## Protected by default

If you look at the core design of the blocks your database methods will reside within a class. So it will look something like 

```csharp
public class YourDataAccessClass : SqlServerDataAccess
{
    public YourDataAccessClass() : base( IDataAccessConfig  )
    {
    }
    //... your methods go here
}
```
At this point, you are working with the context of a class; you can then write your data access method within that class.

```csharp
public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
{
    var sql = "Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like @searchTerm";
    var cmd = CreateTextCommand(sql).WithParameter(searchTerm.ToSqlParameter("@searchTerm"));
    return Execute(cmd).ToListOf<ProductSummary>();
}
```

So in the example above, we are searching for all products with a name like the search term. 

There are a number of key design elements here:
1) We don't trust the data in searchTerm. As such the searchTerm is parameterized. "Name like @searchTerm"
2) The creation of the command is done within the function; in this case, we are creating a text command to send to the database. The command is the object that contains the SQL text.
3) We call `Execute`, passing the command. Both the `CreateTextCommand` and the `Execute` method are **protected by default**. As such, the calling code can only call `GetProductSummaryContainingName`; it cannot call `Execute`. 

This design offers you, as a developer, a degree of protection:

### What can go wrong if you don't use parameters

consider the same code without parameters
```csharp
public IList<ProductSummary> GetProductSummaryContainingName(string searchTerm)
{
    var sql = $"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate From [Production].[Product] Where Name like '{searchTerm}'";
    var cmd = CreateTextCommand(sql);
    return Execute(cmd).ToListOf<ProductSummary>();
}
```

1) Calling with expected data example search for %Bikes%
```csharp
var result = _adventureWorksDataAccess.GetProductSummaryContainingName("%Bike%");
```
This does exactly what the parameterized version does.

2) Calling with expected data example search for '%Bike's%'
```csharp
var result = _adventureWorksDataAccess.GetProductSummaryContainingName("%Bike's%");
```
Here we start to get problems the parameters in parameterized version you will have the following SQL executed which will execute as expected  
```sql
Declare @SearchTerm varchar(64) = '%Bike''s%'
Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where name like @SearchTerm
```
In the non parameterized version you will have the following SQL executed
```sql
Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where name like '%Bike's%'
``` 
Here the server will generate a SQL error.

3) Lets Inject:
Calling with expected data example search for ''; SHUTDOWN  WITH NOWAIT;'
```csharp
     var result = _adventureWorksDataAccess.GetProductSummaryContainingName("'; SHUTDOWN  WITH NOWAIT;");
```
With the parameterized version, this will run:
```sql
Declare @SearchTerm varchar(64) = '; SHUTDOWN  WITH NOWAIT;'
Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where name like @SearchTerm
```
This will execute the search looking for all the products with `'; SHUTDOWN WITH NOWAIT;'`. An odd search term, but no damage done.

However with the no parameterized version you will have the following SQL executed which will execute as expected  


In the non parameterized version you will have the following SQL executed
```sql
Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where name like ''; SHUTDOWN  WITH NOWAIT;
```

And if you are running a SQL connection with a high-privilege account, you will be running around trying to work out why the server stopped responding.

 ### Expose the execute methods at your peril

 Using the blocks, there is no direct way to execute a SQL statement from the calling code. As the developer, you may be tempted to expose this to avoid writing your own access methods by making the protected methods public. Working directly with the SQL means that, as a developer, you are responsible for the SQL generated; this means responsibility for injection attacks. The simplest way to avoid injection attacks is not to execute any SQL that is not 100% controlled by the code and parameterized. The developer is responsible for generating the SQL to be executed, and this will be controlled in the DataAccess layer, i.e., your class. As the developer, you are fully responsible here; the blocks are simply providing the tool around the protection. 
