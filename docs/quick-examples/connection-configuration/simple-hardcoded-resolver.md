---
layout: default
title: Simple Connection String Resolver
nav_exclude: true
---

### Simple Connection String Data Access Config

This is a useful option for demo examples where you want to hardcode the connection string. Good for testing and demos. 

To use this you can just simple up a new SimpleConnectionStringDataAccessConfig()

``` csharp
    public class YourDataAccessClass : SqlServerDataAccess
    {
        public YourDataAccessClass() : 
            base( new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
        {
        }
        
        public async Task<IList<SpWhoResult>> ExecSpWho()
        {
            var cmd = CreateStoredProcedureCommand("sp_Who");
            return ExecuteAsync(cmd).ToListOf<SpWhoResult>();
        }
    }
```

