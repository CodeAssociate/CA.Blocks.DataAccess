## Introduction

The CA.Blocks.DataAccess is a lightweight .NET library designed specifically for Onion and CQRS type architectures to extract and work with relational databases. The base premise is reducing the object–relational impedance mismatch that exists between the relational world and the object world in .NET.   The design focuses on making the datatype translations more seamless, moving from a dataset and tables structures into objects. 


### Benefits

1. Direct working with the SQL layer. If you link controlling exactly what the SQL does this s a good framework.
2. Custom transactors allowing you to use the generic converters, or implementing you own converters for performance,
3. Easy setup and Go with providers for SQL light and SQL server.


### What it is not

The CA.Blocks.DataAccess are not trying to be a an ORM like Entity framework for NHibernate and Hide the SQL  The base classes have never been designed to be any form of ORM.  In fact using the blocks you will be working directly with the underlying SQL. If you don’t what to work with SQL then this is not the library for you.  The Blocks allow you to work directly with the SQL code whilst providing a good translation lib allowing you to move from the C# types in the SQL and the SQL into the C# types.


### History

CA.Blocks.DataAccess grew out of projects I been working on since 2003.   It has evolved with the frameworks as they have been updated.  After trying to switch to Entity framework and n-Hibernate I came back to the CA.Blocks.DataAccess for the simple reason it was  lightweight and far more predictable.   I found that whilst the Entity framework and n-Hibernate excel at CRUD type applications I was doing a lot of workarounds and conversions trying use and them in either CQRS and layered onion type Architectures.  A tell-tale sign is  you using reflection to copy objects across boundaries.
The blocks have been used in may of the projects I have been involved with. 
