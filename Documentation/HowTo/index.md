## The How to guide 
The How to Section provides simple examples of how to work with blocks. If you looking at setting up the connection see the [Getting Started](../GettingStarted/getting-started.md) section

For the context of this section, we will use the standard Microsoft AdventureWorks database schema and use using SQL server.
To get the version of the database you can use go to https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure

All of the content in this section will be working within the methods inside the example AdventureWorksDataAccess.

 

```Csharp
    public class AdventureWorksDataAccess : SqlServerDataAccess
    {
        public AdventureWorksDataAccess() :
            base(new SimpleConnectionStringDataAccessConfig(
                "Server=(local);Database=AdventureWorks2019;Integrated Security=SSPI;TrustServerCertificate=True"))
        {

        }

    }
```

* [Selecting Scalar Values](selecting-scalar-values.md)
* [Selecting Rows Values](selecting-single-rows.md)
* [Selecting multiple Values](selecting-multiple-rows.md)