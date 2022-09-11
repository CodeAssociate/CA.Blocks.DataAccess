## The How to guide 
The How to Section provides simple examples on how to work with blocks. If you looking at setting up the connection see the [Getting Started](../GettingStarted/getting-started.md) section

For the context on this section we will use the standard microsoft AdventureWorks database and use sql server provider.
To get the version of the database you can use got to https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure

All of the context in this section will be working within the methods inside your YourDataAccessClass.

 

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