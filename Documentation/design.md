
## Design
The CA.Blocks.DataAccess is designed as a micro-ORMs for relational databases. It's core functionality focuses on reducing the object–relational impedance mismatch that exists between the relational world and the object world of objects in .NET.
It was was designed to work with onion / layered and and CQRS type architectures that can work with with or without dependency injection.  The blocks are build onto of ADO.NET then expose each provider.  These are all independent assemblies such that each of the providers can be isolate. It you using MySQL you do not  need to pull in the SQL server dependencies and visa versa.

At a high level you can create a class that inherits from the provider you what to use.  Once you have that class in place it is  a case of creating your data access methods. 

Below is a very simple example that that will connect to the local sql sever use a trusted connectio. it then exposes two methods 
1. ExecSpWho - this will execute the sp_who stored procedure and return the results int he POCO class called SpWhoResult.
2. GetSysObjectsOfType - this will execute the query with a parameter and return the results in a class called SysObjectsResult. 

```CSharp
    public class YourDataAccessClass : SqlServerDataAccess
    {
        public YourDataAccessClass() : base( 
            new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
        {
        }

        public IList<SpWhoResult> ExecSpWho()
        {
            var cmd = CreateStoredProcedureCommand("sp_Who");
            return Execute(cmd).ToListOf<SpWhoResult>();
        }

        public IList<SysObjectsResult> GetSysObjectsOfType(string xtype)
        {
            var cmd = CreateTextCommand("Select * from sysobjects where xtype = @xtype")
                .WithParameter(xtype.ToSqlParameter("@xtype"));
            return Execute(cmd).ToListOf<SysObjectsResult>();
        }
    }
```

At the ADO.NET level you dealing with three core constructs. 
1. The Connection
2. The Command
3. The Reader

* The blocks manage the connection for you as such your code is not concerned with establishing, opening executing then closing the connection. 
* The blocks provide helper methods to create and setup the command.  The premised is that your code can build the command to be executed then pass than command in for execution. The Blocks managed getting the connection wired up to the command for execute then execute the result. There is full support for Sync and Async operations if the underlying  provider supports Async operations. 
* The blocks provide a mapping layer to convert the result form the reader back into the object world. 

## Protected by default 

Whilst all the core methods will allow processing of some sort SQL, the design is protected by default, even at the provider level.  Using the blocks there is no direct way to execute a SQL statement from the calling code. As the developer you may be tempted to expose this to avoid writing you own access methods by making the protected methods public.  Working directly with the SQL means as a developer you are responsible for the SQL generated this means responsibility for injection attacks.  The simplest way to avoid injection attacks is not executing any SQL that is not 100% controlled by the code and parameterized. The developer is responsible for generating the SQL to be executed and this will be controlled in the DataAccess Layer ie your class.

## The Assemblies  

1. The Model - CA.Blocks.DataAccess.Model used for client access in multi tier architectures.
2. The Core Abstract Data Access Logic - CA.Blocks.DataAccess used for abstract and shared non provider specific code.  
3. The specific implementation - eg    CA.Blocks.SQLServerDataAccess or CA.Blocks.SQLLiteDataAccess or CA.Blocks.MySQLDataAccess

<p align="center">
    <img src="_assets/DesignCA.Blocks.DataAccess.png" alt="Design CA.Blocks.DataAccess" />
</p>

### The Model

The *model* represents the core design elements that you will need a client to specify. The client might not have access to the data access class as such the model is implemented in an independent assembly and will have no dependencies. An example is the PagingRequest class. The paging request is a common element that is specified on the client and passed into the The specific implementation to execute. The paging class can then be shared on the client application allowing it to specify the paging request without having to have a reference to the  DataAccess or any of the provider classes.  This would be typical in N Tier architectures and repository patterns were the code has no idea of what is behind the interface.  The Client will have no dependencies on the Data Access classes. The CA.Blocks.DataAccess.Model has no dependencies. 

### The Core Abstract code

The code in the CA.Blocks.DataAccess is abstract and common among all providers.  The assembly will have no dependencies on any specific provider. This assembly handles the connection, execution and translation, all of these elements are in System.Data namespace. It will work at the System.Data level you will not find any specific reference to to any provider at this level.

### The specific implementation 
This code hooks in the specific provider implementation. So if you connecting to a Microsoft SQL server database  you will reference CA.Blocks.SQLServerDataAccess.  This in turn will bring in  CA.Blocks.DataAccess and CA.Blocks.DataAccess.Model in addition to Microsoft.Data.SqlClient. 

When using the DataAccess block to write a Data Access Class you only need to install the specific provider you need. For Examples:

To install for SQL server
```bash
PM> Install-Package CA.Blocks.SQLServerDataAccess -Version x.x.x
```

To install for Microsoft.Data.Sqlite
```bash
PM> Install-Package CA.Blocks.SQLLiteDataAccess -Version x.x.x
```

To install for MySqlConnector
```bash
PM> Install-Package CA.Blocks.MySQLDataAccess -Version x.x.x
```
See [Getting Started](GettingStarted)