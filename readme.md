# CA.Blocks.DataAccess

[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package SQL Server](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/)
- [NuGet Package My SQL](https://www.nuget.org/packages/CA.Blocks.MySQLDataAccess/)
- [NuGet Package SQlite](https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/) 


### Dapper-level speed with advanced SQL translation control and optimized parameter mapping.

`CA.Blocks.DataAccess` is a production-proven, high-performance data access framework for .NET applications. Built to bridge the critical gap between complex Object-Relational Mappers (ORMs) like Entity Framework and raw SQL micro-ORMs like Dapper, it gives developers absolute control over database interaction without sacrificing development velocity.

> 📊 **Proven at Scale:** Trusted across the .NET ecosystem with over **76,000+ downloads** on NuGet.

---

## 💡 Looking to Optimize Your .NET Database Performance?
I am the author of this framework and an independent consultant specializing in diagnosing and fixing severe database performance bottlenecks in .NET applications (especially scaling past Entity Framework limitations). 

# TODO form to book a discovery call.

If your database CPU is spiking, your APIs are lagging, or your batch processes are locking tables, let's chat.
👉 ** [Book a 15-Minute Technical Discovery Call](https://CodeAssociate.github.io/CA.Blocks.DataAccess/Index.html)


## 🚀 Why CA.Blocks.DataAccess?

Entity Framework Core is fantastic for rapid prototyping, but it frequently treats the database as a black box—often generating massive, unoptimized SQL statements, cartesian explosions, or redundant memory allocations during high-throughput operations. Dapper solves the speed issue but forces developers back into manual parameter mapping and raw strings

`CA.Blocks.DataAccess` solves this by offering:
*   **Dapper-Level Execution Speed:** Minimal abstraction layer means near-native ADO.NET execution performance and ultra-low heap allocations.
*   **Advanced Parameter Mapping & Support:** Seamless, high-performance binding for complex commands and stored procedures without the type-coercion pitfalls that bypass database indexes.
*   **Predictable SQL Translators:** Better command/query translation control so you know exactly what is executing on your server before it hits production.

---

## 🛠️ Project Status & Roadmap

This repository is **actively maintained by the author** and serves as a core engine for high-performance .NET consulting architecture.

*   **Community Policy:** To maintain strict architectural integrity, lean performance overhead, and focus on production stability, **we are not accepting external Pull Requests (PRs) at this time.** Bug reports and architectural discussions via GitHub Issues are highly welcome.

### Current 2026 Roadmap:
- [ ] Drop legacy `.NET Standard` support to unlock native `.NET 8/9` zero-allocation memory optimizations (like `Span<T>`).
- [ ] Migrate the full integration testing matrix from NUnit to `xUnit` + `.NET Aspire` for modern, containerized cloud-native orchestration.
- [ ] Publish core architectural recipes for high-throughput batch synchronization.

---

## 📖 Quick Start

Quick Start SQL server example:

```csharp

    public class SpWhoResult
    {
        public short spid { get; init; }
        public short ecid { get; init; }
        public string status { get; init; }
        public string loginame { get; init; }
        public string hostname { get; init; }
        public string blk { get; init; }
        public string? dbname { get; init; }
        public string cmd { get; init; }
        public int request_id { get; init; }
    }

    public class YourDataAccessClass : SqlServerDataAccess
    {
        public YourDataAccessClass() : base( new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
        {}
        
        public IList<SpWhoResult> ExecSpWho()
        {
            var cmd = CreateStoredProcedureCommand("sp_Who");
            return Execute(cmd).ToListOf<SpWhoResult>();
        }
    }
```