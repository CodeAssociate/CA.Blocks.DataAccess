### JsonConfigConnectionStringsResolver

This is a implementation of IDataAccessKeyToConnectionStringResolver that uses the Microsoft.Extensions.ConfigurationConfigurationManager class.  This is common with the .NET Core frameworks and designed using the dependency injection pattern.

Example config in appsettings.json 

``` json 
{
    "ConnectionStrings": {
        "exampleName": "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"
  }
}

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