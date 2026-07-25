---
layout: default
title: Home
nav_order: 1
description: "Overview and Quick Start for CA.Blocks.DataAccess."
---

# CA.Blocks.DataAccess
Built to help developers write clean data access code faster, CA.Blocks.DataAccess removes the friction of setting up custom repositories and database context boilerplate. It integrates seamlessly with .NET Dependency Injection, giving you an expressive, testable foundation for your database queries out of the box.


![ado Boiler plate](docs/_assets/impedance_bridge.jpg)
![ado Boiler plate](docs/_assets/AD_Boilerplate.jpg)
![using the blocks abstract and distilled and clean data access](docs/_assets/Blocks_Distilled.jpg)


---
## ⚡ Quick Start

Get up and running in your .NET project in under 2 minutes:

### 1. Install Package
Choose the package for your database:

| Database | NuGet Package | Install Command |
| :--- | :--- | :--- |
| **SQL Server** | `CA.Blocks.SQLServerDataAccess` | `dotnet add package CA.Blocks.SQLServerDataAccess` |
| **SQLite** | `CA.Blocks.SqliteDataAccess` | `dotnet add package CA.Blocks.SqliteDataAccess` |
| **PostgreSQL** | `CA.Blocks.PostgreSQLDataAccess` | `dotnet add package CA.Blocks.PostgreSQLDataAccess` |
| **MySQL** | `CA.Blocks.MySQLDataAccess` | `dotnet add package CA.Blocks.MySQLDataAccess` |
| **ODBC** | `CA.Blocks.OdbcDataAccess` | `dotnet add package CA.Blocks.OdbcDataAccess` |

### 2. Set your connection strings 

Choose how to resolve your connection strings:

| Method | Resolver Class | Extension Package |
| :--- | :--- | :--- |
| **appsettings.json** | `JsonConfigGetConnectionStringResolver` | `CA.Blocks.DataAccess.Extensions.Config.Json` |
| **Environment Variables** | `EnvironmentVariableConnectionStringResolver` | (Built-in) |
| **app.config** | Custom implementation | (Built-in) |
| **Roll your own** | `IDataAccessKeyToConnectionStringResolver` | (Built-in) |

#### Using `appsettings.json`
```bash
dotnet add package CA.Blocks.DataAccess.Extensions.Config.Json
```
Then 
```bash json
{
    "ConnectionStrings": {
    "exampleName": "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"
    }
}
```
Then use
```csharp
    public class MyDataAccess : SqlServerDataAccess
    {
        public MyDataAccess(IConfiguration configuration) : base (
                new DataAccessConfig( 
                    new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
                    new JsonConfigGetConnectionStringResolver(configuration))
            )
            {
            }
        ...
    }
```
In this example, we are using the `SqlServerDataAccess` module. The DI is set up to read the `appsettings.json` file looking for a connection string key 'exampleName' that will resolve to your connection string. 

#### Using `app.config`
```bash xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="exampleName" connectionString="Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"/>
  </connectionStrings>
</configuration>
```

#### Using Environment Variables
This is useful for cloud-native applications and containers where connection strings are often stored as environment variables.

```csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new EnvironmentVariableDataAccessConfig("MyDbConnection")
        )
        {
        }
    ...
}
```

#### Using Roll your own 

```csharp
public class ExampleConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
{

  public string GetConnectionString(string connectionStringKey)
  {
        var result = $"Server={Environment.MachineName}-sql;Database={connectionStringKey};Trusted_Connection=True;";
        return result; 
  }
}
```


### 3. Write your code
#### SQL Server
```csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess(IDataAccessKeyToConnectionStringResolver resolver) : base (
            new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "YourDBKey" }, resolver)
        )
        {
        }
    ...
}
```

#### Using `appsettings.json` with the default key
```csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess(IConfiguration configuration) : base (
            new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "default" }, new JsonConfigGetConnectionStringResolver(configuration))
        )
        {
        }
    ...
    // your code goes here
    ...
}
```



📖 Documentation & Guides

Learn more about setting up and extending the library:

🚀 [Quick Start Guide](quick-start.md) — Step-by-step setup and basic usage.

🏗️ [Architecture & Design](architecture.md) — How the "Pluggable Building Blocks" work together.

📦 [Package Reference](packages.md) — A complete list of all NuGet packages and extensions.

⚙️ [Configuration & DI Options](getting-started/dependency-injection.md) — Connection strings, lifetime options, and extensions.

🧪 Unit Testing Guide — How to mock repositories and write fast unit tests. (Coming Soon)

💡 Key Features
Zero Boilerplate: Clean, generic repository implementation out of the box.

Async-First: Native async/await support across all database operations.

DI-Ready: Simple extension methods for native .NET Dependency Injection.

Testable: Built against clean interfaces for effortless mocking.

🤝 Contributing & Source Code
Source Code & Issues: [GitHub Repository](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

Report a Bug: Submit an [Issue](https://github.com/CodeAssociate/CA.Blocks.DataAccess/issues)
