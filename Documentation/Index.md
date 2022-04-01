> The CA.Blocks.DataAccess is a lightweight .NET library designed specifically for Onion and CQRS type architectures to working with relational databases. The base premise is reducing the object–relational impedance mismatch that exists between the relational world and the object world of .NET. The design focuses on making the datatype translations more seamless, moving from DataReaders, dataset and table type structures into objects, and from .NET types into SQL parameters.


### Benefits

1. The bulk of the conversion to and from ADO structures is abstracted using translators, 1-1 mappings are a simple as ExecuteToListOf<>
2. Custom translators allowing you to use the generic converters, or implementing you own converters for performance. The design allows for a flexible configuration, mingling core function with your own customisations.    
3. Direct working with the SQL layer. If you like controlling exactly what the SQL does this is a good framework, as you are 100% in the control of the SQL.
4. Easy setup and Go with providers for SQL light and SQL server, and MySQL.
5. Works with multiple concurrent Async requests. (try doing that with EF)


### What it is not

The CA.Blocks.DataAccess is not trying to be a an ORM like Entity framework for NHibernate and hiding the SQL.  The base classes have never been designed to be any form of ORM, as such you will find no DB context or Unit of work pattern with SaveChanges. In fact using the blocks you will be working directly with the underlying SQL datastore. There is no magic projection for queries or magic save to have changes to committed back to entities in the DB. Whilst there are helper functions you are 100% in control the database interactions.

Using the blocks allows full control of the SQL. And full concurrent Async support If you don’t want to work with SQL then this is not the library to used.  The Blocks allow you to work directly with the SQL code whilst providing a good structure to *reduce* the *impedance mismatch* between the two technologies allowing a developer to move from the C# types into SQL parameters and then SQL into the C# types.


### History

CA.Blocks.DataAccess grew out of projects I been working on since 2003, they are well used and tested under different conditions. They have evolved as the .NET framework as evolved.  After trying to switch to Entity framework and n-Hibernate I came back to the CA.Blocks.DataAccess as my defacto for the simple reason it was lightweight and far more predictable with far more control.   I found that whilst the Entity framework and n-Hibernate excel at CRUD type applications I was doing a lot of workarounds and conversions trying use and them in either CQRS, layered onion, and repository  type Architectures.  I have seen many code bases use Entity framework for data access in a Repository Pattern, any in most cases you end up writing more code in the object world to avoid working to a few SQL statements.  A tell-tale sign is you using reflection or mapping to copy objects of similar structures across boundaries. 