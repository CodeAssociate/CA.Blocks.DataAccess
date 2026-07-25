---
layout: default
title: execute Stored procedures
description: "execute Stored procedures"
parent: How to
nav_order: 7
---

### execute Stored procedures

Working with stored procedures is very easy with the blocks.  You set the procedure name in the CreateStoredProcedureCommand, then execute it like any other command.


```csharp
    public class SpWhoResult
    {
        public short spid { get; init; }
        public short ecid { get; init; }
        public string status { get; init; }
        public string loginame { get; init; }
        public string hostname { get; init; }
        public string blk { get; init; }
        public string dbname { get; init; }
        public string cmd { get; init; }
        public int request_id { get; init; }
    }

    public IList<SpWhoResult> ExecSpWho()
    {
        var cmd = CreateStoredProcedureCommand("sp_Who");
        return Execute(cmd).ToListOf<SpWhoResult>();
    }
```