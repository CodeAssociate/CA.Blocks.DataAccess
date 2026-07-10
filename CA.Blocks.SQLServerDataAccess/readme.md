[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/)
- [Source Code](https://dev.azure.com/RavinEnterprises/CA.Blocks/_git/CA.Blocks.DataAccess)


This Package is a extension to the DataAccess Blocks targeting the Sqllite provider

Quick example

poco object
``` C#
    public class ExampleSysObject
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string XType { get; set; }
        public DateTime CreateDate { get; set; }
    }

```
The Data access class 
``` C#
    public class YourDataAccessClass : SqlServerDataAccess
    {
        public YourDataAccessClass() : 
            base( new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security = SSPI; TrustServerCertificate=True"))            
        {

        }

       internal string ReadSysObjectsOfTypeSql => @"
SELECT  TOP 10 id as Id, name as Name, xtype as XType, crdate as CreateDate 
FROM  sysobjects 
WHERE xtype = @xtype";


		public IList<ExampleSysObject> ReadSysObjectsOfType(string xtype)
        {
            var cmd = CreateTextCommand(ReadSysObjectsOfTypeSql)
	            .WithParameter(xtype.ToSqlParameter("@xtype"));
            return Execute(cmd).ToListOf<ExampleSysObject>();
        }

        public async Task<IList<ExampleSysObject>> ReadSysObjectsOfTypeAsync(string xtype)
        {
            var cmd = CreateTextCommand(ReadSysObjectsOfTypeSql)
	            .WithParameter(xtype.ToSqlParameter("@xtype"));
            return await ExecuteAsync(cmd).ToListOf<ExampleSysObject>();
        }

        public async Task<IList<ExampleSysObject>> ReadSysObjectsOfTypeWithSqlBuilderAsync(string xtype)
        {
            var sqlBuilder = new SafeSqlBuilder();
            sqlBuilder.AddSql($"SELECT  TOP 10 id as Id, name as Name, xtype as XType, crdate as CreateDate FROM sysobjects WHERE xtype = {xtype:@xtype}");
            // this will build a valid sql statement with full parementer support. 
            // the sql will be "SELECT  TOP 10 id as Id, name as Name, xtype as XType, crdate as CreateDate FROM sysobjects WHERE xtype = @xtype}" with the xtype passed in as a parameter

            return await ExecuteAsync(sqlBuilder.BuildSqlCommand()).ToListOf<ExampleSysObject>();
        }

    }
    
```

Calling the data access class 
``` C#

    var instance = new YourDataAccessClass();
    var results  = instance.ReadSysObjectsOfType("U");
    foreach (var o in results)
    {
        Console.WriteLine($"{o.Id},{o.Name},{o.Type},{o.CreateDate}");
    }

```
