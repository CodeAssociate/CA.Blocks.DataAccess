## Welcome to CA.Blocks.DataAccess Help Guide

This site is for developers who want to learn how to use the CA.Blocks.DataAccess a micro ORM.

## What is CA.Blocks.DataAccess 
The CA.Blocks.DataAccess is a lightweight .NET library build on top of ADO.NET designed specifically for Onion and CQRS type architectures working working with relational databases. It's core functionality focuses on reducing the object–relational impedance mismatch that exists between the relational world and the object world of .NET. Developers in the .NET world don't want to spend hours writing code to map results from ADO.NET data readers to instances of those objects, or focusing on basic repeated connection details.   CA.Blocks.DataAccess was written by Kevin Bosch and has been released as open source project under the MIT licence. 

## Is CA.Blocks.DataAccess an ORM? 
 The short answer is no and it is not trying to be one. CA.Blocks.DataAccess falls into a family of tools known as micro-ORMs. These tools perform only a subset of the functionality of full-blown Object Relations Mappers, such as Entity Framework Core or NHibernate. Features vary by product. The following table provides a general idea of the capabilities that you can expect to find in a micro ORM compared to an ORM:


| Function                           | Micro ORM | ORM |
| -----------                        | --------- |---- |
| Map queries to objects             | ✅ | ✅ |
| Connection Management              | Some | ✅ |
| Caching results                    | ❌ | ✅ |
| Change tracking                    | ❌ | ✅ |
| SQL generation                     | ❌ | ✅ |
| Lazy loading                       | ❌ | ✅ |
| Unit of work support               | ❌ | ✅ |
| Database migrations                | ❌| ✅ |

The debate is never ending on the merits of ORM's. In general Micro ORM's usually perform faster than a ORM they generally have no expectation about the schema of your database, whilst the projections are loosely coupled there is no direct coupling to the underlying tables. 

To get the most out of any Micro ORM you will have to good Understanding of SQL, Row Sets, and parameters. The CA.Blocks.DataAccess is no different. If you don’t want to work with SQL then this is not the library for you. The vast majority of performance problems with the CA.Blocks.DataAccess boil down to the underlying SQL, parameters or row sets transferred.The Blocks allow you to work directly with the SQL code whilst providing a good structure to *reduce* the *impedance mismatch* between the two technologies allowing a developer to move from the C# types into SQL parameters and then SQL into the C# types.

## What does the CA.Blocks.DataAccess do
The blocks is build on ADO.NET, Whilst ADO.NET is a great technology, it is very low level and verbose, it is dealing with connections, commands and readers, while they are very fast the interfaces lead to a lot of boiler plate code.

If we consider the task of executing the result SQL server sp_who procedure into the following class:

```Csharp
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
```

Here is the standard ADO.NET code for retrieving data from a database and materialising it as a list of SpWhoResult objects:

```Csharp
    public IList<SpWhoResult> ExecSpWhoUsingAdonet()
    {
        var result = new List<SpWhoResult>();
        using (var connection = new SqlConnection("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
        {
            connection.Open();
            using (var command = new SqlCommand("Exec sp_who", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new SpWhoResult
                        {
                            spid = reader.GetInt16(reader.GetOrdinal("spid")),
                            ecid = reader.GetInt16(reader.GetOrdinal("ecid")),
                            status = reader.GetString(reader.GetOrdinal("status")),
                            loginame = reader.GetString(reader.GetOrdinal("loginame")),
                            hostname = reader.GetString(reader.GetOrdinal("hostname")),
                            blk = reader.GetString(reader.GetOrdinal("blk")),
                            dbname = reader.IsDBNull(reader.GetOrdinal("dbname")) ? null : reader.GetString(reader.GetOrdinal("dbname")),
                            cmd = reader.GetString(reader.GetOrdinal("cmd")),
                            request_id = reader.GetInt32(reader.GetOrdinal("request_id"))
                        };
                        result.Add(item);
                    }
                }
            }
            connection.Close();
        }
        return result;
    }
```

Using the Blocks we can reduce the code above to the code below:

```Csharp
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
By design the blocks wraps your data access method into class which  is inherited from a base provider. The Connection is setup in the constructor.  The blocks will support inline use as above as well as dependency injection.  

Once setup the typical data access method will involve 
1) Constructing a command
2) Executing the command 
3) Mapping the command to the object results ie the O and M of ORM - Object Mapping.

The underlying connection is fully managed we 


## Benefits

1. The bulk of the conversion to and from ADO structures is abstracted using translators
2. The translators are extendable so you can use you own converters The design allows for a flexible configuration, mingling core functions with your own customisations.    
3. Direct working with the SQL layer. If you like controlling exactly what the SQL does this is a good framework, as you are 100% in the control of the SQL.
4. Easy setup and Go with providers for SQL light and SQL server, and MySQL.
5. Works with multiple concurrent Async requests.


## History
CA.Blocks.DataAccess has been release as open source by Kevin Bosch. Whilst it was only release on Nuget around 2016 Kevin has been using blocks since 2003, they are well used and tested under different conditions. They have evolved as the .NET framework as evolved.  After trying to switch to Entity framework and n-Hibernate I came back to the CA.Blocks.DataAccess as my defacto for the simple reason it was lightweight and far more predictable with far more control.  In most cases I was fighting with the ORM frameworks. 

 I found that whilst the Entity framework and n-Hibernate excel at CRUD type applications I was doing a lot of workarounds and conversions trying use and them in either CQRS, layered onion, or repository type Architectures.  I have seen many code bases use Entity framework for data access in a Repository Pattern, any most cases you end up writing more code in the object world to avoid working to a few SQL statements.  A tell-tale sign is you using reflection or mapping to copy objects of similar structures across boundaries.  You simply replacing one impedance mismatch with another.  
