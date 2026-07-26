---
layout: default
title: Getting Started with PostgreSQL
nav_exclude: true
parent: Quick Start
---

## Getting started with PostgreSQL 

1. Install NuGet package for [PostgreSQL](https://www.nuget.org/packages/CA.Blocks.PostgreSQLDataAccess)  
```bash
>  dotnet add package CA.Blocks.PostgreSQLDataAccess
```

2. Configure your [connection string](./../quick-examples/connection-configuration/index.md)  

3. Create your DataAccess class 
*Here we use the SimpleConnectionStringDataAccessConfig resolver for simplicity*

Example:

```csharp
// model class
 public class MyCustomObject
  {
      public required string TableName {get; init;}
      public required string Owner {get; init;}
      public required bool HasIndexes {get; init;}
  }
// data access class
public class YourDataAccessClass : PostgresDataAccess
{
    public YourDataAccessClass() : 
        base( new SimpleConnectionStringDataAccessConfig("Host=localhost;Database=postgres;Username=postgres;Password=password"))
    {
    }
    
    public async Task<IList<MyCustomObject>> GetMyCustomObjects()
    {
        // Step 1: construct the SQL command
        var sqlCmd = CreateTextCommand(@"
SELECT tablename as TableName, tableowner as Owner, hasindexes as HasIndexes FROM pg_catalog.pg_tables");
        return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();  
        //           Step 2 ^^^^^^^^^^^^  Step 3 ^^^^^^^^^^^^^^^^^^
    }
}
```
There are a few notes about this example:

1. You can see Blocks will support modern syntax in your model supporting the required keyword and init setters
2. Once you have set up your class to inherit from PostgresDataAccess, the methods in your data access class follow the same pattern

![Execution Pipeline](./../_assets/ExectionPipelinepng.jpg)
- first step is to construct the SqlCommand (here we build a SQL statement to get data from my_table where the type matches the input parameter)
- second step is to execute the command (here we are calling ExecuteAsync(sqlCmd))
- final step is what to do with the stream of data that comes back. (.ToListOf<MyCustomObject>() here we are converting the data reader into .NET objects; as the mapping is 1-1 it is easy)
