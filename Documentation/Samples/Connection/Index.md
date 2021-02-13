### Connections

One of the first tasks you will need to do is setup a connection to a database. The CA Data Access blocks follows the .NET convention of using named configured connection strings. The connection strings encode the parameters needed to make the connection to the database such as database server, database name, network protocol, Credentials etc. Internally the core DataAccess block will ask to resolve a connectionStringKey to a connection string value.  This is done calling the IDataAccessKeyToConnectionStringResolver interface. 

``` csharp
public interface IDataAccessKeyToConnectionStringResolver
{
    string GetConnectionString(string connectionStringKey);
}
```

The blocks only have a single concrete implementation of the IDataAccessKeyToConnectionStringResolver which is the HardCodedConnectionStringsResolver.  The AppDotConfigConnectionStringsResolver and JsonConfigConnectionStringsResolver have been pulled out of blocks as the configuration is More of a Hosting app concern. Having these concrete implementations meant the blocks were pulling in both configuration models and bloating the project dependencies.  


1. If you are using web.config or app.config typically .Net 1 -  4.8 [see using  AppDotConfigConnectionStringsResolver ](./UsingAppDotConfigConnectionStringsResolver.html)
2. If you using config.json typically .Net Core  [see using JsonConfigConnectionStringsResolver.html](./UsingJsonConfigConnectionStringsResolver.html)

Of course you can also use a custom ConnectionStringsResolver [see implementing a CustomConnectionStringResolver](./CustomConnectionStringResolver.html)