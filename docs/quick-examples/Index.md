---
layout: default
title: Samples
nav_exclude: true
---

## Connections

One of the first tasks you will need to do is set up a connection to a database. The CA Data Access blocks follow the .NET convention of using named configured connection strings. The connection strings encode the parameters needed to make the connection to the database, such as database server, database name, network protocol, credentials, etc. Internally, the core DataAccess block will ask to resolve a `connectionStringKey` to a connection string value. This is done by calling the `IDataAccessKeyToConnectionStringResolver` interface. 

``` csharp
public interface IDataAccessKeyToConnectionStringResolver
{
    string GetConnectionString(string connectionStringKey);
}
```

The blocks only have a single concrete implementation of the `IDataAccessKeyToConnectionStringResolver`, which is the `HardCodedConnectionStringsResolver`. The `AppDotConfigConnectionStringsResolver` and `JsonConfigGetConnectionStringResolver` have been pulled out of the blocks, as configuration is more of a hosting app concern. Having these concrete implementations meant the blocks were pulling in both configuration models and bloating project dependencies.  

### Using app.config 
If you are using `web.config` or `app.config` (typically .NET Framework 1.1 - 4.8), [see using AppDotConfigConnectionStringsResolver](./connection-configuration/UsingAppDotConfigConnectionStringsResolver.md).


### Using appsettings.json
If you are using `appsettings.json` or `config.json` (typically .NET Core / .NET 5+), [see using JsonConfigConnectionStringsResolver](./connection-configuration/UsingJsonConfigConnectionStringsResolver.md).

### Using a custom ConnectionStringsResolver
You can use a custom `ConnectionStringsResolver`, [see implementing a CustomConnectionStringResolver](./connection-configuration/CustomConnectionStringResolver.md).


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