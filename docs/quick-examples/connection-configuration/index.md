---
layout: default
title: Setting up connection strings
nav_exclude: true
parent: Quick Start
---

## Connections

One of the first tasks you will need to do is set up a connection to a database. The CA.Blocks.DataAccess  follow the .NET convention of using named configured connection strings. and uses Dependency injection to do the resolution. The connection strings encode the parameters needed to make the connection to the database, such as database server, database name, network protocol, credentials, etc. Internally, the core DataAccess block will ask to resolve a `connectionStringKey` to a connection string value. This is done by calling the `IDataAccessKeyToConnectionStringResolver` interface. 


``` csharp
public interface IDataAccessKeyToConnectionStringResolver
{
    string GetConnectionString(string connectionStringKey);
}
```

As this is an interface suppoting dependecy injection it provides a pluggable architecture and allowing for a mix and match approach. The Blocks have three two built in providers and an Extension for the most common method
1) Appsettings- this is the most common for new .net core applications this uses an extention package to pull in the .net core configuration packages. The block use the extention to avoid pulling in the json config is not need.
2) Environment Variables - this is common for infrastructure running the cloud where the config is injected on the cloud start up. those values typically come from key vaults and are assessible via the .NET core runtime. 
3) In Code - the quick a dirty hard coded solution to get code up and running quickly this is used extensively through the documentation 

| Method                                                                                                      | Resolver Class                                | Extension Package                                         |
|:------------------------------------------------------------------------------------------------------------|:----------------------------------------------|:----------------------------------------------------------|
| [**appsettings.json**](./json-resolver.md)                                                                  | `JsonConfigGetConnectionStringResolver`       | `CA.Blocks.DataAccess.Extensions.Config.Json`             |
| [**Environment Variables**](./environment-variables-resolver.md)                                            | `EnvironmentVariableConnectionStringResolver` | (Built-in)                                                |
| [**In Code**](./simple-hardcoded-resolver.md)                    | `HardCodedConnectionStringsResolver`          | (Built-in)                                                |
| [**app.config**](./app-config-resolver.md)                       | Custom using ConfigurationManager             | (your code)                                               |
| [**custom**](./custom-resolver.md)                               | `IDataAccessKeyToConnectionStringResolver`    | (your code)                                               |



The blocks only have a single concrete implementation of the `IDataAccessKeyToConnectionStringResolver`, which is the `HardCodedConnectionStringsResolver`. The `AppDotConfigConnectionStringsResolver` and `JsonConfigGetConnectionStringResolver` have been pulled out of the blocks, as configuration is more of a hosting app concern. Having these concrete implementations meant the blocks were pulling in both configuration models and bloating project dependencies.