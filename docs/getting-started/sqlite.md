---
layout: default
title: Getting Started with Sqlite
nav_exclude: true
parent: Quick Start
---

## Getting started with Sqlite 

1. Install NuGet package for [Sqlite](https://www.nuget.org/packages/CA.Blocks.SqliteDataAccess)  
```bash
>  dotnet add package CA.Blocks.SqliteDataAccess
```

2. Configure your [connection string](./../quick-examples/connection-configuration/index.md)  

3. Create your DataAccess class 
*Here we use the SimpleConnectionStringDataAccessConfig resolver for simplicity*

Example:

```csharp
// model class
public class MyCustomObject
{
  public required int Id {get; init;}
  public required string Name {get; init;}
  public required DateTime CreateDate {get; init;}
}
// data access class
public class YourDataAccessClass : SqliteDataAccess
{
    public YourDataAccessClass() : 
        base( new SimpleConnectionStringDataAccessConfig("Data Source=MyDatabase.db;Cache=Shared"))
    {
    }
    
    public async Task<IList<MyCustomObject>> GetMyCustomObjects(string type)
    {
        // Step 1: construct the SQL command
        var sqlCmd = CreateTextCommand(@"
SELECT id as Id, name as Name, created_at as CreateDate
FROM MyTable WHERE type = @Type").WithParameter(type.ToSqlParameter("@Type"));
        return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();  
        //           Step 2 ^^^^^^^^^^^^  Step 3 ^^^^^^^^^^^^^^^^^^
    }
}
```
There are a few notes about this example:

1. You can see Blocks will support modern syntax in your model supporting the required keyword and init setters
2. Once you have set up your class to inherit from SqliteDataAccess, the methods in your data access class follow the same pattern

![Execution Pipeline](./../_assets/ExectionPipelinepng.jpg)
- first step is to construct the SqlCommand (here we build a SQL statement to get data from MyTable where the type matches the input parameter)
- second step is to execute the command (here we are calling ExecuteAsync(sqlCmd))
- final step is what to do with the stream of data that comes back. (.ToListOf<MyCustomObject>() here we are converting the data reader into .NET objects; as the mapping is 1-1 it is easy)
