### AppDotConfigConnectionStringsResolver

This is a implementation of IDataAccessKeyToConnectionStringResolver that uses the Microsoft System.Configuration.ConfigurationManager class.  This is common with the .NET 1-4.8 frameworks. It will be using the App.config or web.config ConnectionStrings setting.

Example config in app.config using a local sqlserver db

``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="exampleName" connectionString="Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"/>
  </connectionStrings>
</configuration>
```

To use this in from the blocks we need to join the config up

``` csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig("configName", 
            new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
            new AppDotConfigConnectionStringsResolver())
        )
        {
        }
    ...
}

```

With this setup the class MyDataAccess is ready to have data access methods written. 
In the code example above we are using the  AppDotConfigConnectionStringsResolver to resolve the Named connection value  of "exampleName" to the connection string of  "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true". 