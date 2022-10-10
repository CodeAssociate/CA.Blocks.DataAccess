### Execute Non Query 

THe Execute Non Query is for commands that do not return any results.  These are generally grouped into three sets of statements DDL (Data Definition Language)  the DML (Data Manipulation Language) and the DCL (Data Control Language).

#### DDL (Data Definition Language)
 Data Definition Language actually consists of the SQL commands that can be used to define the database schema. It simply deals with descriptions of the database schema and is used to create and modify the structure of database objects in the database. DDL is a set of SQL commands used to create, modify, and delete database structures but not the data.  Examples include:

* CREATE
* DROP
* ALTER

#### DCL (Data Control Language)
The Data Control Language includes commands that deal with the rights, permissions, and other controls of the database system. Examples include:
* GRANT
* REVOKE

#### DML (Data Manipulation Language)

The Data Manipulation Language are the SQL commands that deal with the manipulation of data present in the database. 
Examples of DML commands include: 

* INSERT 
* UPDATE
* DELETE 


As far as the blocks are concerned all of these  of these groups can be execute through the ExecuteNonQuery command, The only slight difference in the return value. For a DML statement the Execute Non Query returns an int, representing the number of rows affected by the successful completion of the command. So in the query affected  20 rows you will get 20. For the DDL and DCL statements the return will be -1. The -1 represents no data was returned, -1 is used as it is different from zero records returned.

Any syntax error or other DB error from the database will we raise a DbException.


The example below will return 1 as only one row is created
```C#
    public int CreateNewProductCategory(string name)
    {
        var sql = "Insert into [Production].[ProductCategory] (Name, rowguid, ModifiedDate) values (@name, NEWID(), GetDate())";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```

IN the example below we are deleting a DeleteProductCategory by name, the name has a unique index on so if the Product Category exists as it deleted the function will return 1, if no Product Category exists the result will be 0. 

```C#
    public int DeleteProductCategory(string name)
    {
        var sql = "Delete from [Production].[ProductCategory] where Name = @name";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```


Example Executing a DDL ensuring the return is -1 as it is a DDL statement
```C#
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

More Typically you can execute the ExecuteNonQuery as a void and deal with any exceptions
```C#
        public void CreateTableExample2()
        {
            var sql = @"
If exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MyTable')
BEGIN
	drop  table MyTable 
END;
Create Table MyTable (Id int not null, Name VarChar(10) not null);";
            var cmd = CreateTextCommand(sql);
            return ExecuteNonQuery(cmd);
        }
```