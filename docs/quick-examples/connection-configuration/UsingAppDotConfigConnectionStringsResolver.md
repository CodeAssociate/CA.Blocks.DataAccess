---
layout: default
title: AppDotConfigConnectionStringsResolver
nav_exclude: true
---

### AppDotConfigConnectionStringsResolver

This is a implementation of IDataAccessKeyToConnectionStringResolver that uses the Microsoft System.Configuration.ConfigurationManager class.  This is common with the .NET 1 to 4.8 frameworks. It will be using the App.config or web.config ConnectionStrings setting.

Example config in app.config using a local sqlserver db

``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="exampleName" connectionString="Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"/>
  </connectionStrings>
</configuration>
```

First you will need to add a reference to the package System.Configuration.ConfigurationManager 
Create a class in your hosing application called AppDotConfigConnectionStringsResolver, to resolve the Key to the connection string.  


``` csharp
public class AppDotConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
{
    /// <summary>
    /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
    /// </summary>
    /// <param name="connectionStringKey">The Connection string known to the code</param>
    /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
    public string GetConnectionString(string connectionStringKey)
    {
        return ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
    }
}
```

To use this in from the blocks we need to join the config up notice we are using the AppDotConfigConnectionStringsResolver copied from above

``` csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig( 
                new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
                new AppDotConfigConnectionStringsResolver())
        )
        {
        }
    ...
}

```

With this setup the class MyDataAccess is ready to have data access methods written. 
In the code example above we are using the AppDotConfigConnectionStringsResolver to resolve the Named connection value  of "exampleName" to the connection string of  "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true". 