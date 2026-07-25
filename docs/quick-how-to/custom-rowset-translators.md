---
layout: default
title: Custom Row Translators
description: "Custom Row Translators"
parent: How To
nav_order: 8
---
{: .no_toc .text-delta }
## Custom Row Translators Overview

{:toc_levels="3"}

1. TOC
{:toc}

---

A core part of the CA.Blocks.DataAccess functionality is reducing the object-relational impedance mismatch that exists between the relational world and the object world of .NET.   In the relational world, the data structures revolve around sets of tables with each table having rows and columns. Each intersection of a row with a column has a cell value. Cells come from a finite number of simple data types, like strings, numbers dates. The tables are all linked with primary and foreign keys.  In the object world, the data structures are far richer with objects having properties, a property can be any value including other objects. The properties themselves reflect the relationships between objects, as such, there is no real concept of foreign keys.    

The Row Translators have the responsibility of mapping the Table structure which is rows, columns and cells into the class structure with properties.  The focus on the Row Translators is at the structure level.  See the column translators for the cell mappings. 


### One-to-one mapping

In this case, we going to look at the case where the table structure is aligned with the class structure 
With a  SQL Table Structure as 
```sql
    Create TABLE MyTable (
        [Id]  int not null,
        [Name] nvarchar(64) not null,
        [Status] tinyint not null,
        [Quantity] decimal(18,2) null,
        [Modified] Datetime2(7) null
    )
```
The Table data need to be mapped into the  .NET Class Structure
```csharp
    public class MyClass
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public byte Status { get; init; }
        public decimal? Quantity { get; init; }
        public DateTime? Modified { get; init; }
    }
```

This setup, is a simple 1-1 mapping you can simply call execute passing in the command object. The mapping is done using the ToListOf method which will work with the  IDataReader reader object returning the list of MyClass objects as follows:

```csharp
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var cmd = CreateTextCommand("Select * From MyTable");
        return Execute(cmd).ToListOf<MyClass>();
    }
```

In this setup,  the command will be executed. The command, in this case, is 'Select * From MyTable'. Once executed this will come back as a DataReader. The Reader can then read the rows, getting the columns as the command was set up there is knowledge of the expected columns returned ie the code knowns and expects there will be a column called 'Name' in the reader. 

With with command you can almost read what the code is going to do in the translator if we break down the statement 'Execute(cmd).ToListOf<MyClass>();'

We are saying: execute this command "Select * From MyTable" into a list of `MyClass` objects. In this example, you are not getting back a table structure with rows, columns, and cells, but are getting back an `IList<MyClass>`—this has been converted into the .NET world of objects.

#### Notes
1) The Names have to match and are case sensitive. 
2) The auto conversion will pick the data column converter based on the type defined in the property of the in the class. 


### Mapping with when the structures do not align
While this is nice and easy, we need to provide flexibility as not all conversion with be 1-1.  Lets consider  the example when the Structures do not align

The SQL Table Structure
```sql
    Create TABLE MyTable (
        [MyTableId]  int not null,
        [MyTableName] nvarchar(64) not null,
        [Status] tinyint not null,
        [Quantity] decimal(18,2) null,
        [Modified] Datetime2(7) null
    )
```
The .NET Class Structure
```csharp
    public class MyClass
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Status { get; init; }
        public decimal? Quantity { get; init; }
        public DateTime? ModifiedAt { get; init; }
    }
```

To make use of automatic mapping there are three options:
1) Alias the query in SQL to make it look like the object
2) Provide mapping attributes to the class in .NET
3) Implement a Custom Mapping Function

#### using SQL to alias 
```csharp
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select MyTableId as Id, MyTableName as Name, Cast([Status] as Int) as [Status] , Quantity, Modified as ModifiedAt from MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>();
    }
```

```
In this case have aligned the names in SQL to Match the Target Class
MyTableId as Id 
MyTableName as Name
Cast([Status] as Int) as [Status] // Here we are keeping Same Name but making sure it is the correct dataType
Modified as ModifiedAt
```

In this example we have gone back to the server and make the sever return something that does map 1-1.


#### Provide mapping attributes to the class in .NET
The second Option you have to to provide the markup in the target class

```csharp
    public class MyClass
    {
        [DbColToSourceName("MyTableId")]
        public int Id { get; init; }
        [DbColToSourceName("MyTableName")]
        public string Name { get; init; }
        [DbColToTypeConverter(typeof(IntDbColToTypeConverter))]
        public int Status { get; init; }
        public decimal? Quantity { get; init; }
        [DbColToSourceName("Modified")]
        public DateTime? ModifiedAt { get; init; }
    }
```
In this case, we have simply turned the mapping around, providing the mapping info on the .NET side. With this in place, we can execute the query.

```csharp
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>();
    }
```

#### The custom mapping

The Most powerful and most flexible option is to use a custom function for the mapping

This can be either be a lambda or function 

Using a lambda
```csharp
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>(reader => new MyClass
        {
            Id = reader.AsInt("MyTableId"),
            Name = reader.AsString("MyTableName"),
            Status = reader.AsInt("Status"),
            Quantity = reader.AsNullDecimal("Quantity"),
            ModifiedAt = reader.AsDateTime("Modified")
        }
        );
    }
```

Using a function.  The Key advantage for function is the you can reuse the conversion in other places.
```csharp
    private MyClass MyCustomConvert(IDataReader reader)
    {
        return new MyClass
        {
            Id = reader.AsInt("MyTableId"),
            Name = reader.AsString("MyTableName"),
            Status = reader.AsInt("Status"),
            Quantity = reader.AsNullDecimal("Quantity"),
            ModifiedAt = reader.AsDateTime("Modified")
        }
    }

    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>(MyCustomConvert);
    }
```

