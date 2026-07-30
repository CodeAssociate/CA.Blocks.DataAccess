---
layout: default
title: Design
parent: Architecture
nav_order: 1
---

## Design
The CA.Blocks.DataAccess is designed as a micro-ORM for relational databases. Its core functionality focuses on reducing the object-relational impedance mismatch that exists between the relational world and the object world of objects in .NET.
It was designed to work with onion / layered and CQRS-type architectures and can work with or without dependency injection. The blocks are built on top of ADO.NET; the core layer is implemented within `CA.Blocks.DataAccess`. This layer has no dependency on any provider; each provider is implemented as a specialization of the abstract core. These are all independent assemblies, such that each of the providers can be isolated. If you are using MySQL, you do not need to pull in the SQL Server dependencies and vice versa.


At a high level, the code simply inherits from the provider you want to use, injecting the configuration. Once you have that class in place, it is a case of creating your data access methods.

Below is a simple example that will connect to the local SQL Server using a trusted connection. It then exposes two methods:
1. **ExecSpWho** - this will execute the sp_who stored procedure and return the results in the POCO class called SpWhoResult.
2. **GetSysObjectsOfType** - this will execute the query with a parameter and return the results in a class called SysObjectsResult.

```csharp
public class YourDataAccessClass : SqlServerDataAccess
{
    public YourDataAccessClass() : base( 
        new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
    {
    }

    public async Task<IList<SpWhoResult>> ExecSpWhoAsync()
    {
        var cmd = CreateStoredProcedureCommand("sp_Who");
        return await ExecuteAsync(cmd).ToListOf<SpWhoResult>();
    }

    public async Task<IList<SysObjectsResult>> GetSysObjectsOfType(string xtype)
    {
        var cmd = CreateTextCommand("Select * from sysobjects where xtype = @xtype")
            .WithParameter(xtype.ToSqlParameter("@xtype"));
        return await ExecuteAsync(cmd).ToListOf<SysObjectsResult>();
    }
}
```

At the ADO.NET level, you deal with three core constructs.
1. The Connection
2. The Command
3. The Reader

* The blocks manage the connection for you as such your code is not concerned with establishing, opening executing then closing the connection.
* The blocks provide helper methods to create and set up the command.  The premise is that your code can build the command to be executed and then pass that command in for execution. The Blocks manage the process of creating and wiring up the connection to the command for execution.  There is full support for Sync and Async operations if the underlying provider supports Async operations.
* Finally, the blocks provide a rich mapping layer that converts the results from the reader back into the object world.

## Protected by default

Whilst all the core methods will allow processing of some sort SQL, the design is protected by default, even at the provider level.  Using the blocks there is no direct way to execute a SQL statement from the calling code. As the developer, you may be tempted to expose this to avoid writing your access methods by making the protected methods public.  Working directly with the SQL means as a developer you are responsible for the SQL generated which means responsibility for injection attacks.  The simplest way to avoid injection attacks is not to execute any SQL that is not 100% controlled by the code and parameterized. The developer is responsible for generating the SQL to be executed and this will be controlled in the DataAccess Layer ie your class.

## The Assemblies

1. The Model - `CA.Blocks.DataAccess.Model` used for client access in multi-tier architectures. With 100% separation, there is no need to expose the blocks beyond the assembly in which they are consumed.
2. The Core Abstract Data Access Logic - `CA.Blocks.DataAccess` is used for abstract and shared non-provider-specific code.
3. The specific implementation - e.g., `CA.Blocks.SQLServerDataAccess`, `CA.Blocks.SqliteDataAccess`, or `CA.Blocks.MySQLDataAccess`.


### The Model

The *model* represents the core design elements that you will need a client to specify. The client might not have access to the data access class as such the model is implemented in an independent assembly and will have no dependencies. An example is the PagingRequest class. The paging request is a common element that is specified by the client and passed into the specific implementation to execute. The paging class can then be shared on the client application allowing it to specify the paging request without having a reference to the DataAccess or any of the provider classes.  This would be typical in N Tier architectures and repository patterns where the code has no idea of what is behind the interface.  The Client will have no dependencies on the Data Access classes. The CA.Blocks.DataAccess.Model has no dependencies. This becomes very useful with technologies such as Blazor WebAssembly.

### The Core Abstract code

The code in `CA.Blocks.DataAccess` is abstract and common among all providers. The assembly will have no dependencies on any specific provider. This assembly handles the connection, execution, and translation; all of these elements are in the `System.Data` namespace. It will work entirely at the `System.Data` level. This class is abstract, so it cannot be used by itself. You will not find any provider-specific references at this level.

### The specific implementation
This code hooks in the specific provider implementation. So if you connect to a Microsoft SQL Server database, you will reference `CA.Blocks.SQLServerDataAccess`. This, in turn, will bring in `CA.Blocks.DataAccess` and `CA.Blocks.DataAccess.Model`, in addition to `Microsoft.Data.SqlClient`.

When using the DataAccess block to write a Data Access class, you only need to install the specific provider you need. For example:

To install for SQL Server:
```powershell
PM> Install-Package CA.Blocks.SQLServerDataAccess -Version x.x.x
```

To install for Microsoft.Data.Sqlite:
```powershell
PM> Install-Package CA.Blocks.SqliteDataAccess -Version x.x.x
```

To install for MySQL:
```powershell
PM> Install-Package CA.Blocks.MySQLDataAccess -Version x.x.x
```
