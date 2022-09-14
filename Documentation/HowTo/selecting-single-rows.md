## Selecting Single Rows

Selecting single rows from a database is a common task. There are two primary driving cases:
* Firstly selecting data involved with a unique index such as a primary key, where there can be zero or one rows returned. 
* Secondly selecting the top row from a set typically with an specified order , for example selecting the person with the most sales, or selecting a persons most recent sale, In these cases there many be zero, one or more rows returned. 

Depending on how you want to work with the data that you retrieve there are a number of options when working with single row data. These options focus on how to you what to deal with the cases where there are zero, rows, one row and many rows. 
The default method is ExecuteTo&lt;T>(cmd). This provides the default implementation of FirstOrDefault. 




| Method      | Description | Exception
| ----------- | ----------- | ------|
| [ExecuteTo&lt;T>(cmd)](#executetot) | This is a alias method mapping to Execute(cmd).FirstOrDefault&lt;T> | |
| [Execute(cmd).ToFirstOrDefault&lt;T>](#executecmdtofirstordefaultt)   | Returns the first of one or more rows as an instance of the type specified by the T or default if no results are returned | |
| [Execute(cmd).ToFirst&lt;T>](#executecmdtofirstt)   | Returns the first of one or more rows as an instance of the type specified by the T. | DataException when there are no rows in the returned set |
| [Execute(cmd).ToSingleOrDefault&lt;T>](#executecmdtosingleordefaultt)   |Returns the single row as an instance of the type specified by the T or default if no results are returned, You use this when you expect zero or one row | DataException when there is more than one row in the returned set for example selecting data by primary key|
| [Execute(cmd).ToSingle&lt;T>](#executecmdtosinglet)   | Returns the single row as an instance of the type specified by the T. You use this when you expect one and only one row. use this when there must be one and only row returned | DataException when there is more than one row in the returned set , DataException when there are no rows in the returned set  |
| [ExecuteDataRow(cmd)](#executedatarowcmd) | Use to return a DataRow using a data adaptor  | This has works with the SingleOrDefault behaviour |
| [ExecuteObject(cmd)](#executeobjectcmd) | Use to return a dynamic object | |


In examples below we will be working with the Product table, and returning ProductSummary the projectId Is the Primary Key 

### ExecuteTo&lt;T>

```C#
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return ExecuteTo<ProductSummary>(cmd);
    }
```
The ExecuteTo&lt;T> is simply short form for Execute(cmd).ToFirstOrDefault<T>;

### Execute(cmd).ToFirstOrDefault&lt;T> 
```C#
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToFirstOrDefault<ProductSummary>();
    }
```

### Execute(cmd).ToFirst&lt;T> 




```C#
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute(cmd).ToFirst<ProductSummary>();
    }
```

#### Tips with the Execute(cmd).ToFirst&lt;T>
* Can you used this method when they may one more many records returned. 
* If if there are many you can use restrict data on the database server using top x to get better performance  




### Execute(cmd).ToSingleOrDefault&lt;T>  
```C#
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute<cmd>.ToSingleOrDefault<ProductSummary>(cmd);
    }
```

#### Tips with the Execute(cmd).ToSingleOrDefault&lt;T>  
* Can you used this method when there is zero one ot many records returned. 
* If if there are many you can use restrict data on the database server using top x to get better performance  

### Execute(cmd).ToSingle<T>
```C#
    public ProductSummary GetProductSummary(int productId)
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]
where ProductID = @productId";
        var cmd = CreateTextCommand(sql).WithParameter(productId.ToSqlParameter("@productId"));
        return Execute<cmd>.ToSingle<ProductSummary>(cmd);
    }
```
#### Tips with the Execute(cmd).ToSingle&lt;T>
* The execute single is design to work you you are working with a Primary or unique key on a data set.
* If you after raw performance the the ToFirst is faster as it skips the check for the second row 
* If there are many row you can use restrict data on the database server using top x to get better performance  

### ExecuteDataRow(cmd)

This method will return a DataRow, the data row can be passed into the translator.  This method is executed using a DbDataAdapter as such there is not Async support. 


### ExecuteObject(cmd) 

This will execute to to dynamic object.  This is useful for quick prototyping but has no type safety. 

### Tips 
If you using this method it is best to understand the the underlying sql. When selecting via a unique key with no joins there can be zero or one rows returned. however when data is involved with joins or data is involved selecting sets to get the best performance you need to using the SQL top or limit syntax.  
* If you using First, or FirstOrDefault you can use Top 1  or LIMIT 1 depending on the database. 
* If you are using Single or SingleOrDefault and what to check for no second data you can use Top 2 or LIMIT 2 depending on the database. 
* Using Single with Top 1 will yield the same behaviour as First