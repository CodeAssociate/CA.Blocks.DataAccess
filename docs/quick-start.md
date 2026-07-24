# Quick Start Guide

This guide will help you get started with `CA.Blocks.DataAccess` in your .NET application.

## 1. Install the appropriate package

Choose the package for your database:

| Database | NuGet Package | Install Command |
| :--- | :--- | :--- |
| **SQL Server** | `CA.Blocks.SQLServerDataAccess` | `dotnet add package CA.Blocks.SQLServerDataAccess` |
| **SQLite** | `CA.Blocks.SqliteDataAccess` | `dotnet add package CA.Blocks.SqliteDataAccess` |
| **PostgreSQL** | `CA.Blocks.PostgreSQLDataAccess` | `dotnet add package CA.Blocks.PostgreSQLDataAccess` |
| **MySQL** | `CA.Blocks.MySQLDataAccess` | `dotnet add package CA.Blocks.MySQLDataAccess` |
| **ODBC** | `CA.Blocks.OdbcDataAccess` | `dotnet add package CA.Blocks.OdbcDataAccess` |

## 2. Configure Your Data Access Class

Inherit from the provider-specific base class (e.g., `SqlServerDataAccess`) and choose a connection string resolver:

| Method | Resolver Class | Extension Package |
| :--- | :--- | :--- |
| **appsettings.json** | `JsonConfigGetConnectionStringResolver` | `CA.Blocks.DataAccess.Extensions.Config.Json` |
| **Environment Variables** | `EnvironmentVariableConnectionStringResolver` | (Built-in) |
| **Custom** | `IDataAccessKeyToConnectionStringResolver` | (Built-in) |

### Example (using `appsettings.json`)

```csharp
public class MyDataAccess : SqlServerDataAccess
{
    public MyDataAccess(IConfiguration configuration) : base (
        new DataAccessConfig(
            new DataAccessConfigOptions { ConnectionStringKey = "MyDbConnection" },
            new JsonConfigGetConnectionStringResolver(configuration))
    )
    {
    }

    public async Task<IEnumerable<MyModel>> GetAllAsync()
    {
        var cmd = CreateCommand("SELECT * FROM MyTable");
        return await ExecuteAsync(cmd).ToListOf<MyModel>();
    }
}
```

## 3. Register with Dependency Injection

Register your data access class in `Program.cs`:

```csharp
builder.Services.AddScoped<MyDataAccess>();
```

For more detailed information, please refer to the specific provider documentation.
