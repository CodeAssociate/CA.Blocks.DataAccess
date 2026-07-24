# CA.Blocks.DataAccess
A lightweight, high-performance C# data access layer designed to simplify repository patterns for  database operations in .NET applications.

---
## ⚡ Quick Start

Get up and running in your .NET project in under 2 minutes:

### 1. Install Package
Choose your database

#### SQL Server
```bash
dotnet add package CA.Blocks.SQLServerDataAccess
```
#### Sqlite
```bash
dotnet add package CA.Blocks.SqliteDataAccess 
```
#### Postgres
```bash
dotnet add package CA.Blocks.PostgreSQLDataAccess  
```
#### MySQL
```bash
dotnet add package CA.Blocks.MySQLDataAccess  
```

#### Odbc
```bash
dotnet add package CA.Blocks.OdbcDataAccess   
```

### 2. Set your connection string 

#### Using `appsettings.json`
```bash
dotnet add CA.Blocks.DataAccess.Extensions.Config.Json
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
```Csharp
    public class MyDataAccess : SqlServerDataAccess
    {
        public MyDataAccess(IConfiguration configuration) : base (
                new DataAccessConfig( 
                    new DataAccessConfigOptions { ConnectionStringKey = "exampleName" }, 
                    new JsonConfigConnectionStringsResolver(configuration))
            )
            {
            }
        ...
    }
```
In this example we using the SqlServerDataAccess module The DI is setup to read the appsettings.json file looking for a connection string key exampleName that will resolve to your connection string. 

#### Using `app.config`
```bash xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="exampleName" connectionString="Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"/>
  </connectionStrings>
</configuration>
```
#### Using Role your own 

```Charp
public class ExampleConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
{

  public string GetConnectionString(string connectionStringKey)
  {
        var result = $"Server={Environment.MachineName}-sql;Database={connectionStringKey};Trusted_Connection=True;"
        return result; 
  }
}
```


### 3. 3. Write your code
#### SQL Server
```bash
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "YourDBKey" }, new YourResolverFromrAbove())
        )
        {
        }
    ...
}
-- using `appsettings.json` with the default key
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess() : base (
            new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "default" }, new JsonConfigConnectionStringsResolver())
        )
        {
        }
    ...
    // your code goes here
    ...
}
```



📖 Documentation & Guides
We in the Process of providing  detailed guides  to learn more about setting up and extending the library:
Topics Comming soon:

🚀 Quick Start Guide — Step-by-step setup and basic usage.

⚙️ Configuration & DI Options — Connection strings, lifetime options, and extensions.

🏗️ Architecture & Design — Overview of the Repository & Unit of Work patterns used.

🧪 Unit Testing Guide — How to mock repositories and write fast unit tests.

💡 Key Features
Zero Boilerplate: Clean, generic repository implementation out of the box.

Async-First: Native async/await support across all database operations.

DI-Ready: Simple extension methods for native .NET Dependency Injection.

Testable: Built against clean interfaces (IRepository, IUnitOfWork) for effortless mocking.

🤝 Contributing & Source Code
Source Code & Issues: [GitHub Repository](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

Report a Bug: Submit an [Issue](https://github.com/CodeAssociate/CA.Blocks.DataAccess/issues)
