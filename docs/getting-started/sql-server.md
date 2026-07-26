layout: default
title: Getting Started with Sql server
nav_exclude: true
parent: Quick Start

# Sql server 

1. Install nuget Package for [Sql Server](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess)  
````bash
>  dotnet add package CA.Blocks.SQLServerDataAccess
````

2. Configure your [connection string](./../../quick-examples/connection-configuration/index.md) 

3. Create you DataAccess class 
*Here we use the SimpleConnectionStringDataAccessConfig resolver for simplicity*

Example:

````csharp
// model class
public class MyCustomObject
{
  public required int Id {get; init;}
  public required string Name {get; init;}
  public required DateTime CreateDate {get; init;}
}
// data access classs
public class YourDataAccessClass : SqlServerDataAccess
{
    public YourDataAccessClass() : 
        base( new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
    {
    }
    
    public async Task<IList<MyCustomObject>> GetMyCustomObjects(string type)
    {
        // step 1 construct the sqlcommand
        var sqlCmd = CreateTextCommand(@"
SELECT id as Id, name as Name, crdate as CreateDate
FROM sys.sysobjects WHERE type = @Type").WithParameter(type.ToSqlParameter("@Type"));
        return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();  
        //           Step 2 ^^^^^^^^^^^^  Step 3 ^^^^^^^^^^^^^^^^^^
    }
}
````
There are a few notes about this example:
1) You can see blocks will support modern syntax in you model supporting the required keywork and init setters 
2) Once you have setup you class to inherit from SqlServerDataAccess the methods in your data access class follow the same pattern
   ![Execute Piepline](./../_assets/ExectionPipelinepng.jpg)
   1) first step is construct the sqlcommand  (here we build a sql statement to get data from sys object where the type matched in input parameter)
   2) second step is to execute the command  ( here we are ExecuteAsync(sqlCmd) )
   3) final step is what to do with stream of data that comes back. (.ToListOf<MyCustomObject>() here we converting the data reader into the .NET objects as the mapping is 1-1 it is easy)



