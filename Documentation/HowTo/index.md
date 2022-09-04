## The How to guide 
The How to Section provides simple examples on how to work with blocks. If you looking at setting up the connection see the [Getting Started](../GettingStarted/getting-started.md) section

All of the context in this section will be working within the methods inside your YourDataAccessClass.


```Csharp
    public class YourDataAccessClass : SqlServerDataAccess
    {
        public YourDataAccessClass() : base( new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))
        {}
        
        // The focus of the how to articles are the  methods that are here

        // Selecting Selecting Scalar Value 
    }
```

* [Selecting Scalar Values](selecting-scalar-values.md)