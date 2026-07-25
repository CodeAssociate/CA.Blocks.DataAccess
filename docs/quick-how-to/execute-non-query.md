---
layout: default
title: Execute Non-Query
description: "Execute Non-Query "
parent: How To
nav_order: 5
---

### Execute Non-Query 

The Execute Non-Query is for commands that do not return any results.  These are generally grouped into three sets of statements DDL (Data Definition Language) DML (Data Manipulation Language) and DCL (Data Control Language).

#### DDL (Data Definition Language)
 Data Definition Language consists of the SQL commands that can be used to define the database schema. It simply deals with descriptions of the database schema and is used to create and modify the structure of database objects in the database. DDL is a set of SQL commands used to create, modify, and delete database structures but not the data.  Examples include:

* CREATE
* DROP
* ALTER

#### DCL (Data Control Language)
The Data Control Language includes commands that deal with the rights, permissions, and other controls of the database system. Examples include:
* GRANT
* REVOKE

#### DML (Data Manipulation Language)

The Data Manipulation Language is the SQL command that deals with the manipulation of data present in the database. 
Examples of DML commands include: 

* INSERT 
* UPDATE
* DELETE 


As far as the blocks are concerned, all of these groups can be executed through the `ExecuteNonQuery` command. The only slight difference is the return value. For a DML statement, the Execute Non-Query returns an `int`, representing the number of rows affected by the successful completion of the command. So if the query affected 20 rows, you will get 20. For the DDL and DCL statements, the return will be -1. The -1 represents that no data was returned; -1 is used as it is different from zero records returned.

Any syntax error or other DB error from the database will raise a DbException.


The example below will return 1 if only one row is created:
```csharp
    public int CreateNewProductCategory(string name)
    {
        var sql = "Insert into [Production].[ProductCategory] (Name, rowguid, ModifiedDate) values (@name, NEWID(), GetDate())";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```

In the example below, the method `DeleteProductCategory` will delete a row by name. As the name has a unique index, if the product category exists and is deleted, the function will return 1; if no product category exists, the result will be 0. 

```csharp
    public int DeleteProductCategory(string name)
    {
        var sql = "Delete from [Production].[ProductCategory] where Name = @name";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```


Example executing a DDL, ensuring the return is -1 as it is a DDL statement:
```csharp
        public bool CreateTableExample1()
        {
            var sql = @"
If exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MyTable')
BEGIN
	drop  table MyTable 
END;
Create Table MyTable (Id int not null, Name varchar(10) not null);";
            var cmd = CreateTextCommand(sql);
            return ExecuteNonQuery(cmd) == -1;
        }
```

More typically, you can execute the `ExecuteNonQuery` as a void and deal with any exceptions:
```csharp
        public void CreateTableExample2()
        {
            var sql = @"
If exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MyTable')
BEGIN
	drop  table MyTable 
END;
Create Table MyTable (Id int not null, Name VarChar(10) not null);";
            var cmd = CreateTextCommand(sql);
            ExecuteNonQuery(cmd);
        }
```