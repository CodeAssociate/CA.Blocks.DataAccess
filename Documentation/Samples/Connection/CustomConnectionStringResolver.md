### CustomConnectionStringResolver

Although the app.config and appsettings.json will cater for a large number of cases there are times when you might need custom logic to resolve the connection string.  This common in application that have sharded  databases. This is easily achieved by implementing the  IDataAccessKeyToConnectionStringResolver interface. 

Example custom resolver

``` csharp
public class ExampleConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
{
  public string GetConnectionString(string connectionStringKey)
  {
        // you can use the connectionStringKey to do lookup from the underlying source
        return /*place your code here to get the connectionString*/ "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true";
  }
}
```

To use this in from the blocks we need to join the config up this is done by used by using the ExampleConnectionStringResolver in the constructor of the DataAccess class

``` csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig("configName", 
            new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
            new ExampleConnectionStringResolver())
        )
        {
        }
    ...
}

```

With this setup the class MyDataAccess is ready to have data access methods written. 
In the code example above we are using the  ExampleConnectionStringResolver to resolve the Named connection value  of anyvalue  to the connection string of  "Server=(localdb)\\MSSQLLocalDB;Integrated Security = true". 