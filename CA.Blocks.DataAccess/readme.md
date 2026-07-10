[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [Source Code](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

The CA.Blocks.DataAccess is designed as a micro-ORM for relational databases. Its core functionality focuses on reducing the object-relational impedance mismatch that exists between the relational world and the object world of objects in .NET. It was designed to work with onion / layered and CQRS-type architectures and can work with or without dependency injection. The blocks are built on top of ADO.NET the core layer is implemented within CA.Blocks.DataAccess. This layer has no dependence on any provider, each provider is implemented as implementation on the abstract core. These are all independent assemblies such that each of the providers can be isolated. If you using MySQL you do not need to pull in the SQL server dependencies and visa versa.

The blocks have built support for the following databases
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/)
- [NuGet Package SqlServer](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/)
- [NuGet Package MySql](https://www.nuget.org/packages/CA.Blocks.MySQLDataAccess/)
- [NuGet Package Odbc](https://www.nuget.org/packages/CA.Blocks.OdbcDataAccess/)


If there no direct provider  you can create use a OleDB connection  the example
below will make a connection to a Jet database the format used for Ms Access
```
	public class AccessDb : AbstractedDbDataAccessConnector<OleDbConnection, OleDbDataAdapter, OleDbCommand>
	{
		// https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ole-db-schema-collections 

		public AccessDb(string fileName, string password) : base(
			new SimpleConnectionStringDataAccessConfig($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileName};Jet OLEDB:System Database=system.mdw;Jet OLEDB:Database Password={password};"))
		{

		}
		
		public IList<string> GetTableNames()
		{
			DataTable dt = GetSchema(CommonOleDbCollectionNames.Tables);
			return dt.CreateDataReader().ToSingleNamedColumnList<string>("TABLE_NAME")
				.Where( x => !x.StartsWith("MSys") )  // <-- this is an access concern 
				.ToList();
		}

		public IList<string> GetColumnNames()
		{
			DataTable dt = GetSchema(CommonOleDbCollectionNames.Columns);
			return dt.CreateDataReader().ToSingleNamedColumnList<string>("TABLE_NAME")
				.Where(x => !x.StartsWith("MSys"))  // <-- this is an access concern 
				.ToList();
		}

		// create your data access methods as normal here
		
	}
```

