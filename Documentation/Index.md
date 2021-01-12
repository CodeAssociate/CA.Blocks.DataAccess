## Introduction

The CA.Blocks.DataAccess is a lightweight .NET library designed specifically for Onion and CQRS type architectures to extract and work with relational databases. The base premise is reducing the object–relational impedance mismatch that exists between the relational world and the object world of .NET. The design focuses on making the datatype translations more seamless, moving from a dataset and tables structures into objects and from .NET types into SQL parameters.


### Benefits

1. Direct working with the SQL layer. If you like controlling exactly what the SQL does this s a good framework. 
2. Custom transactors allowing you to use the generic converters, or implementing you own converters for performance. 
3. Easy setup and Go with providers for SQL light and SQL server.


### What it is not

The CA.Blocks.DataAccess are not trying to be a an ORM like Entity framework for NHibernate and Hide the SQL.  The base classes have never been designed to be any form of ORM.  In fact using the blocks you will be working directly with the underlying SQL. There is no magic projection for queries or magic save classes to entities automatically. 

Using the blocks allows full control of the SQL. If you don’t what to work with SQL then this is not the library for you.  The Blocks allow you to work directly with the SQL code whilst providing a good structure to reduce the impedance mismatch between the two technologies allowing the developer to move from the C# types into SQL parameters and then SQL into the C# types.


### History

CA.Blocks.DataAccess grew out of projects I been working on since 2003. It is well tested.  It has evolved with the frameworks as they have been updated.  After trying to switch to Entity framework and n-Hibernate I came back to the CA.Blocks.DataAccess for the simple reason it was lightweight and far more predictable.  I found that whilst the Entity framework and n-Hibernate excel at CRUD type applications I was doing a lot of workarounds and conversions trying use and them in either CQRS or layered onion type Architectures.  I have seen many code bases use Entity framework for data access in a Repository Pattern, any in most cases you end up writing more code in the object world to avoid working to a few SQL statements.  A tell-tale sign is you using reflection to copy objects across boundaries.

### Protected by default 

Whilst all the core methods will allow processing of some sort SQL, the design is protected by default.  Using the blocks there is no direct way to execute a SQL statement from the calling code. As the developer you may be tempted to expose this to avoid writing you own access methods by making the protected public.  Working directly with the SQL means you are responsible for the SQL generated this means you are responsible for injection attacks.  The simplest way to avoid injection attacks is not executing any SQL directly.