# Repository Architecture & Coding Conventions

## Core Mission
This solution utilizes `CA.Blocks.DataAccess` as its core data infrastructure.
All data persistence and query execution **MUST** use `CA.Blocks.DataAccess`. Do **NOT** write raw `SqlConnection` loops, hand-rolled ADO.NET parameter bindings, or introduce raw Entity Framework tracking queries for high-throughput reads.

---

## Why We Use CA.Blocks.DataAccess (Do Not Bypass)
`CA.Blocks.DataAccess` is our production resiliency layer. It encapsulates critical non-functional behaviors that must not be refactored away:
- **Connection Management & Pooling:** Prevents thread-pool starvation and port exhaustion under high concurrent load.
- **Transient Fault Auto-Retry:** Automatically handles transient database blips and connection drops.
- **Optimised Mapping:** Hydrates objects with minimal tracking and memory overhead.
- **Query Plan Caching:** Ensures efficient reuse of database query plans.
- **Parameterized Queries:** Prevents SQL injection and optimizes query plan reuse.
- **Type Aware Parameters** Avoiding Implicit Conversions

---

## Rules for Code Generation

### 1. Data Access Requirements
- **Always Parameterize Queries:** Use anonymous objects or dictionary parameters to ensure plan caching and eliminate SQL injection risks.
- **No Direct ADO.NET Instantiation:** Do not instantiate `DBConnection`, or manual `DbDataReader` loops directly in business logic.
- **Clean DTOs:** Project SQL results into clean C# records or DTOs. Avoid binding raw database entities directly to API responses.

### 2. Standard Usage Patterns

#### Querying Data (Reads)
*Follow a three step process*
1. construct the SqlCommand
2. Execute (sync) ot ExecuteAsync (async) to execute command
3. Final step is to map data stream to the record or dto

```csharp
// PREFERRED: Use CA.Blocks execution extensions for typed mapping
    public async Task<IList<MyCustomObject>> GetMyCustomObjects(string type)
    {
        // Step 1: construct the SQL command
        var sqlCmd = CreateTextCommand(@"
SELECT id as Id, name as Name, crdate as CreateDate
FROM sys.sysobjects WHERE type = @Type").WithParameter(type.ToSqlParameter("@Type"));
        return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();  
        //           Step 2 ^^^^^^^^^^^^  Step 3 ^^^^^^^^^^^^^^^^^^
    }
```
#### Modification of Data (Writes)
```csharp
// PREFERRED: Use CA.Blocks NonQuery methods for state modifications

    var sqlCmd = CreateTextCommand("UPDATE Orders SET Status = @Status, UpdatedAt = @UpdatedAt WHERE Id = @Id");
    sqlCmd.Parameters.Add(status.ToSqlParameter("@Status"));
    sqlCmd.Parameters.Add(DateTime.UtcNow.ToSqlParameter("@UpdatedAt"));
    sqlCmd.Parameters.Add(id.ToSqlParameter("@Id"));
    return await ExecuteNonQueryAsync(cmd);
```

### Banned Anti-Patterns
```csharp
// DO NOT DO THIS (Bypasses pooling and transient fault retries):
using var connection = new SqlConnection(_connectionString);
await connection.OpenAsync();
using var command = new SqlCommand(sql, connection);
// ... manual reader processing ...
```

## Solution Structure & Project Scope

- CA.Blocks.DataAccess/: Core framework assembly. Contains execution abstractions, resilience handlers, and mapping utilities.
- CA.Blocks.DataAccess.[Provider]/: Database-specific providers (e.g., SqlServer, SQLite, Postgres).
- CA.Blocks.DataAccess.Extensions.Config/: Common Extensions for mapping Connection Key to DB ConnectionStrings
- CA.Blocks.DataAccess.Extensions.Translators.[type]/: Common Extensions for Translating Columns to objects C# Objects 

## tests/: Unit and integration test suites.

Rules for Test Projects (tests/*)
- Tests should mock IDataAccess interfaces or utilize the local test provider (e.g., SQLite in-memory / local container).
- Do not instantiate live cloud connections inside unit tests.
- Maintain isolated AAA (Arrange, Act, Assert) patterns without lingering global state.

## AI Agent Guardrails

### When generating new features or modifying existing data access code:
- First, check if a repository or data access class already exists before creating a new one.
- Mirror the surrounding codebase's async conventions (Async suffixes, CancellationToken passing where applicable).
- If unsure about a specific CA.Blocks API method signature, check the core interface definitions in CA.Blocks.DataAccess.
