### Execute Non Query 

THe Execute Non Query is for for commands that are not intended to return resultsets i.e. INSERT, UPDATE and DELETE, commands. IN addition to the DML Commands.  The EExecute Non Query  returns an int, representing the number of rows affected by the successful completion of the command.


THe Example below will return 1 as only one row is created
```C#
    public int CreateNewProductCategory(string name)
    {
        var sql = "Insert into [Production].[ProductCategory] (Name, rowguid, ModifiedDate) values (@name, NEWID(), GetDate())";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```

IN the example below we are deleting a DeleteProductCategory by name, the name has a unique index on so if the project exists as it deleted the function will return 1, if no ProductCategory exists the result will be 0. 

```C#
    public int DeleteProductCategory(string name)
    {
        var sql = "Delete from [Production].[ProductCategory] where Name = @name";
        var cmd = CreateTextCommand(sql).WithParameter(name.ToSqlParameter("@name"));
        return ExecuteNonQuery(cmd);
    }
```

