# Package Reference

`CA.Blocks.DataAccess` is a modular ecosystem. You only install the "blocks" you need for your specific project.

## Core Package
The foundation of the entire library. It contains the base classes, interfaces, and built-in resolvers.

| Package | Description |
| :--- | :--- |
| `CA.Blocks.DataAccess` | Core engine, `DataAccessCore`, and basic translators. |

---

## 1. Database Providers
Install the provider for your specific database engine.

| Package | Database | Base Class |
| :--- | :--- | :--- |
| `CA.Blocks.SQLServerDataAccess` | SQL Server | `SqlServerDataAccess` |
| `CA.Blocks.SqliteDataAccess` | SQLite | `SqliteDataAccess` |
| `CA.Blocks.PostgreSQLDataAccess` | PostgreSQL | `PostgresDataAccess` |
| `CA.Blocks.MySQLDataAccess` | MySQL | `MySqlDataAccess` |
| `CA.Blocks.OdbcDataAccess` | ODBC | `OdbcDataAccess` |

---

## 2. Configuration Extensions
Extend how the library resolves connection strings from different sources.

| Package | Source | Resolver Class |
| :--- | :--- | :--- |
| `CA.Blocks.DataAccess.Extensions.Config.Json` | `appsettings.json` | `JsonConfigGetConnectionStringResolver` |

---

## 3. Translator Extensions
Add support for specialized data types or custom serialization formats.

| Package | Supported Types | Description |
| :--- | :--- | :--- |
| `CA.Blocks.DataAccess.Extensions.Translators.Json` | `complex objects`, `lists` | Maps JSON database columns to C# objects. |
| `CA.Blocks.DataAccess.Extensions.Translators.NUlid` | `Ulid` | Maps database columns to the NUlid type. |
