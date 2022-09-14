## Connections

One of the first tasks you will need to do is setup a connection to a database. The CA Data Access blocks follows the .NET convention of using named configured connection strings. The connection strings encode the parameters needed to make the connection to the database such as database server, database name, network protocol, Credentials etc. Internally the core DataAccess block will ask to resolve a connectionStringKey to a connection string value.  This is done calling the IDataAccessKeyToConnectionStringResolver interface. 

``` csharp
public interface IDataAccessKeyToConnectionStringResolver
{
    string GetConnectionString(string connectionStringKey);
}
```

The blocks only have a single concrete implementation of the IDataAccessKeyToConnectionStringResolver which is the HardCodedConnectionStringsResolver.  The AppDotConfigConnectionStringsResolver and JsonConfigConnectionStringsResolver have been pulled out of blocks as the configuration is more of a hosting app concern. Having these concrete implementations meant the blocks were pulling in both configuration models and bloating the project dependencies.  

### Using app.config 
If you are using web.config or app.config typically .Net 1 -  4.8 [see using  AppDotConfigConnectionStringsResolver ](./UsingAppDotConfigConnectionStringsResolver.html)


### Using appsettings.json
2. If you using appsettings.json or config.json which is typically done with .Net Core  [see using JsonConfigConnectionStringsResolver.html](./UsingJsonConfigConnectionStringsResolver.html)

### Using a Custom  ConnectionStringsResolver
3. You can use custom ConnectionStringsResolver [see implementing a CustomConnectionStringResolver](./CustomConnectionStringResolver.html)


### The hard coded version 
Of course if you happy to use a connection string hard coded in you app you can use the HardCodedConnectionStringsResolver, the block wrap this up with a SimpleConnectionStringDataAccessConfig.

```Csharp
    public class YourDataAccess : SqlServerDataAccess
    {
        public YourDataAccess() :
            base(new SimpleConnectionStringDataAccessConfig(
                "Connection String string goes here"))
        {

        }
        // your methods go here
    }
```

There is  no magic in the SimpleConnectionStringDataAccessConfig it simply the default hardcoded implementation. 
``` Csharp
    public class SimpleConnectionStringDataAccessConfig : DataAccessConfig
    {
        public SimpleConnectionStringDataAccessConfig(string connectionString) : 
            base("NotUsed", new DataAccessConfigOptions { ConnectionStringKey = "NotUsed" },
                new HardCodedConnectionStringsResolver(connectionString))
        {
        }
    }
```