### JsonConfigConnectionStringsResolver

This is an example implementation of IDataAccessKeyToConnectionStringResolver that uses the Microsoft.Extensions.ConfigurationConfigurationManager class.  
This is common with the .NET Core frameworks and designed using the dependency injection pattern. 

Example using Json and with config in appsettings.json and a ConnectionString named exampleName

``` JavaScript 
{
    "ConnectionStrings": {
        "exampleName": "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"
  }
}

```
First you will need to add a reference to the package Microsoft.Extensions.Configuration
Create a class in your code where you data access classes will live called JsonConfigConnectionStringsResolver, to resolve the Key to the connection string.  

``` csharp
/// <summary>
/// Uses the JsonConfig 
/// </summary>
public class JsonConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
{
    private readonly IConfiguration _configuration;

    public JsonConfigConnectionStringsResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString(string connectionStringKey)
    {
        // this is use the IConfiguration to get the configured value for connectionStringKey/
        return _configuration.GetConnectionString(connectionStringKey);
    }
}
```

To use this in from the blocks we need to join the config up notice we are using the JsonConfigConnectionStringsResolver from above

``` csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess(IConfiguration configuration) : base (
            new DataAccessConfig("configName", 
            new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
            new JsonConfigConnectionStringsResolver(configuration))
        )
        {
        }
    ...
}

```

With this setup the class MyDataAccess is ready to have data access methods written. 
In the code example above we are using the  JsonConfigConnectionStringsResolver to resolve the Named connection value  of "exampleName" to the connection string of  "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true". 
