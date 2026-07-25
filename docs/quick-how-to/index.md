---
layout: default
title: How To
nav_order: 2
description: "the how-to section provides simple examples of how to work with the blocks"
has_children: true
---

## The How to guide 
The how-to section provides simple examples of how to work with the blocks. If you are looking for how to set up a connection, see the [Getting Started](../getting-started/getting-started.md) section.

For the context of this section, we will use the standard Microsoft AdventureWorks database schema and SQL Server.
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
* [Selecting Single Rows](selecting-single-rows.md)
* [Selecting Multiple Rows](selecting-multiple-rows.md)
* [Selecting Multiple Sets](selecting-multiple-sets.md)
* [Execute Non-Query](execute-non-query.md)
* [Working with Parameters](working-with-parameters.md)
* [Custom Row Translators](custom-rowset-translators.md)
* [Custom Column Translators](custom-column-translators.md)