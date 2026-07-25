---
layout: default
title: Using Environment Variables
nav_exclude: true
---

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

