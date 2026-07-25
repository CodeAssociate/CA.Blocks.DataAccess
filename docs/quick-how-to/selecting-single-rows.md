---
layout: default
title: Select Single Object
description: "Select Single Object"
parent: How To
nav_order: 2
---

{: .no_toc .text-delta }
## Selecting Single Rows

{:toc_levels="3"}

1. TOC
{:toc}

---

Selecting single rows from a database is a common task. There are two primary driving cases:
* Firstly, selecting data involved with a unique index such as a primary key, where there can be zero or one row returned. 
* Secondly, selecting the top row from a database with a specified order, for example, selecting the person with the most sales, or selecting a person's most recent sale. In these cases, there may be zero, one or more rows returned. 

Depending on how you want to work with the data that you retrieve, there are several options when working with single-row data. These options focus on how you want to deal with the cases where there are zero rows, one row, or many rows. 
The default method is `ExecuteTo<T>(cmd)`. This provides the default implementation of `FirstOrDefault`. 


The examples below: we will be working with the Product table, and returning ProductSummary, The ProductID is the Primary Key to the Product Table.

### ExecuteTo\<T\>
*This is an alias method mapping to Execute(cmd).ToFirstOrDefault<T>()*

```csharp
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return ExecuteTo<ProductSummary>(cmd);
    }
```
The `ExecuteTo<T>` is simply short form for `Execute(cmd).ToFirstOrDefault<T>();`

### Execute(cmd).ToFirstOrDefault\<T\> 
*Returns the first of one or more rows as an instance of the type specified by T or default if no results are returned*
```csharp
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToFirstOrDefault<ProductSummary>();
    }
```

### Execute(cmd).ToFirst\<T\> 
*Returns the first of one or more rows as an instance of the type specified by T. | DataException when there are no rows in the returned set*

```csharp
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToFirst<ProductSummary>();
    }
```

#### Tips with the `Execute(cmd).ToFirst\<T\>`
* Use this method when there are one or many records returned from the database.
* If there are many records you can get the database to restrict data on the server using top x,  This results in better performance  

### Execute(cmd).ToSingleOrDefault\<T\>  
*Returns the single row as an instance of the type specified by T or default if no results are returned. You use this when you expect zero or one row | DataException when there is more than one row in the returned set (for example selecting data by primary key)*
```csharp
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToSingleOrDefault<ProductSummary>();
    }
```

#### Tips with the `Execute(cmd).ToSingleOrDefault<T>`
* Use this method when there is zero or one record returned from the database.
* If there are many you can use restricted data on the database server using top x to get better performance  


### Execute(cmd).ToSingle\<T\>
*Returns the single row as an instance of the type specified by T. You use this when you expect one and only one row. | DataException when there is more than one row in the returned set, or when there are no rows in the returned set*
```csharp
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToSingle<ProductSummary>();
    }
```
#### Tips with the `Execute(cmd).ToSingle<T>`
* The execute single is designed to work when you are working with a primary or unique key on a data set.
* If you are after the best performance the `ToFirst` is faster as it skips the check for the second row. 
* If there are many rows you can use restricted data on the database server using top x to get better performance  

### ExecuteDataRow(cmd)
*Used to return a DataRow using a data adapter | This works with the SingleOrDefault behavior*
This method will return a DataRow, the data row can be passed into the translator.  This method is executed using a DbDataAdapter as such there is no Async support. 


### ExecuteObject(cmd) 
*Use to return a dynamic object*

This will execute to a dynamic object. This is useful for quick prototyping but provides no "type safety". 

### Tips 
If you are using this method it is best to understand the underlying SQL. When selecting via a unique key with no joins, there can be zero or one row returned. However, when data is involved with joins or data is involved in selecting sets to get the best performance, you need to use the SQL "TOP" or "LIMIT" syntax.  
* If you are using `First` or `FirstOrDefault`, you can use `TOP 1` or `LIMIT 1` depending on the database. 
* If you are using `Single` or `SingleOrDefault` and want to check for no second data row, you can use `TOP 2` or `LIMIT 2` depending on the database. 
* Using `Single` with `TOP 1` will yield the same behavior as `First`.