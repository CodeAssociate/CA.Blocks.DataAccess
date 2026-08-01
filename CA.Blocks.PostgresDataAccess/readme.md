[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/)
- [Source Code](https://github.com/CodeAssociate/CA.Blocks.DataAccess)


This Package is a extension to the DataAccess Blocks targeting the postgres provider

Quick example

poco object
``` C#
    public class PgTables
    {
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public string tableowner { get; set; }
        public string? tablespace { get; set; }
        public bool hasindexes { get; set; }
        public bool hasrules { get; set; }
        public bool hastriggers { get; set; }
        public bool rowsecurity { get; set; }
    }

```
The Data access class 
``` C#
    public class ReadDataTableDataAccess : PostgresDataAccess
    { 
        public ReadDataTableDataAccess() :
            base(new SimpleConnectionStringDataAccessConfig("YourConnectionString"))
        {

        }


        public async Task<IList<PgTables>> GetInformationSchema()
        {
            var cmd = CreateTextCommand("select * from pg_catalog.pg_tables");
            return await ExecuteAsync(cmd).ToListOf<PgTables>();
        }
    }
    
```

Calling the data access class 
``` C#

    var target = new ReadDataTableDataAccess();
    var executeResult = await target.GetInformationSchema();
    foreach (var item in executeResult)
    {
        Console.WriteLine($"{item.schemaname}.{item.tablename} owned by {item.tableowner} (hasindexes={item.hasindexes},hastriggers={item.hastriggers})");
    }

```
