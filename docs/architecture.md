# Architecture & Design

`CA.Blocks.DataAccess` is designed with high performance and clean architecture in mind. It provides a lightweight abstraction over ADO.NET, focusing on speed and predictability.

## Core Design Principles

### 1. "Protected by Default"
The library encourages a design where database operations are encapsulated within repository-like classes. It avoids exposing raw database connections or leakable resources to the business layer.

### 2. Async-First
All database operations are natively asynchronous, supporting `Task` and `CancellationToken` throughout the stack.

### 3. Lightweight Abstraction
Unlike heavy ORMs, `CA.Blocks.DataAccess` stays close to ADO.NET. This ensures:
- **Minimal overhead**: Near-native execution speed.
- **Predictable SQL**: You have full control over the SQL being executed.
- **Efficient Parameter Mapping**: Optimized mapping that avoids common performance pitfalls.

## Components

### DataAccessCore
The base class for all database-specific implementations. It handles connection management, command execution, and mapping results to objects.

### Connection String Resolvers
Interfaces like `IDataAccessKeyToConnectionStringResolver` allow for flexible connection management, whether using `appsettings.json`, environment variables, or custom logic.

### Command Builders
The framework provides fluent ways to create and configure database commands, including support for stored procedures and inline SQL.

## Patterns Supported

- **Repository Pattern**: Easily create repositories by inheriting from provider-specific base classes.
- **Unit of Work**: Support for transactions and coordinated operations across multiple commands.
