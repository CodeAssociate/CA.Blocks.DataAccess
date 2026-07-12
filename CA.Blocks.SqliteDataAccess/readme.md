[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/)
- [Source Code](https://dev.azure.com/RavinEnterprises/CA.Blocks/_git/CA.Blocks.DataAccess)

This Package is a extension to the DataAccess Blocks targeting the Sqllite provider

Quick example

poco object
``` C#
    public class sqliteMaster
    {
        public string name { get; set; }
        public string type { get; set; }
        public int rootpage { get; set; }
        public string sql { get; set; }
    }

```
The Data access class 
``` C#

    public class YourDataAccessClass : SqliteDataAccess.SqliteDataAccess
    {
        public YourDataAccessClass(): base(new SimpleConnectionStringDataAccessConfig("Data Source=.\\fileName.db"))
        {
        }

        public IList<sqliteMaster> GetSqlliteMasterObjects()
        {
	        var cmd = CreateTextCommand("Select * from sqlite_master");
	        return Execute(cmd).ToListOf<sqliteMaster>();
		}

        public async Task<IList<sqliteMaster>> GetSqlliteMasterObjects()
        {
	        var cmd = CreateTextCommand("Select * from sqlite_master");
	        return await ExecuteAsync(cmd).ToListOf<sqliteMaster>();
		}

    }
```

Calling the data access class 
``` C#

    var instance = new YourDataAccessClass();
    var results  = instance.GetSqlliteMasterObjects();
    foreach (var o in results)
    {
        Console.WriteLine($"{o.name},{o.type},{o.rootpage},{o.sql}");
    }

```
