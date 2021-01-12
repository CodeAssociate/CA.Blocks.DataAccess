### Connections

One of the first tasks you will need to do is setup a connection to a database. The CA Data Access blocks follow the .NET convention of using named configured connection strings. The connection strings encode the parameters needed to make the connection to the database such as database server, database name, network protocol, Credentials etc. Internally the core DataAccess block will ask to resolve a connectionStringKey to a connection string value.  This is done calling the IDataAccessKeyToConnectionStringResolver interface. 

``` csharp
public interface IDataAccessKeyToConnectionStringResolver
{
    string GetConnectionString(string connectionStringKey);
}
```

The blocks has made a couple of concrete implementations

1. If you are using web.config or app.config [see using  AppDotConfigConnectionStringsResolver ](./UsingAppDotConfigConnectionStringsResolver.html)
2. If you using config.json [see using JsonConfigConnectionStringsResolver.html](./UsingJsonConfigConnectionStringsResolver.html)

Of course you can also use a custom ConnectionStringsResolver [see implementing a CustomConnectionStringResolver](./CustomConnectionStringResolver.html)