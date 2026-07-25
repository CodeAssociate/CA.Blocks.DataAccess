---
layout: default
title: Custom Connection String Resolver
nav_exclude: true
---

### CustomConnectionStringResolver

Although the app.config and appsettings.json will cater to a large number of cases there are times when you might need custom logic to resolve the connection string.  This is common in applications that work with sharded databases. This is easily achieved by implementing the  IDataAccessKeyToConnectionStringResolver interface. 

When you setup a Data access connection there are two key elements to need to supply
1. The configuration name to use when using the application, you will need this is your application can connect to multiple databases name of the 
2. Is this class that implements the IDataAccessKeyToConnectionStringResolver, this class will be called with the configuration name to resolve the the connection string, you can use any logic you need to resolve this. 

Example custom resolver 
In the example below we will create a theoretical resolver that returns a matched sql host based on the name of the local machine and uses the connectionStringKey as the database name. As stated the this is a simple theoretical example showing how you can have custom code to resolve e connection string.

``` csharp
public class ExampleConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
{

  public string GetConnectionString(string connectionStringKey)
  {
        var result = $"Server={Environment.MachineName}-sql;Database={connectionStringKey};Trusted_Connection=True;"
        return result; 
  }
}
```

To use this in from the blocks we need to join the config up. This is done in the this is done in the DataAccessConfigOptions by used by using the ExampleConnectionStringResolver in the constructor of the DataAccess class

``` csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "TheDatabaseName" }, new ExampleConnectionStringResolver())
        )
        {
        }
    ...
}

```

With this setup the class MyDataAccess is ready to have data access methods written. 
In the code example above we are using the  ExampleConnectionStringResolver  when executed from a machine called "MyMachineName" it will result in trying to use the connection string 

"Server=MyMachineName-sql;Database=TheDatabaseName;Trusted_Connection=True;"

