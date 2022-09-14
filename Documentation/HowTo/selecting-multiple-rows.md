## Selecting Multiple Rows

--TODO 
DataTable
ExecuteToListOf
Execute
ExecuteReader


| Method      | Description |
| ----------- | ----------- |
| ExecuteToListOf<T>(cmd)   | returns an object derived from the data reader |
| [Execute(cmd).ToListOf&lt;T>](#executecmdtolistofltt)   | executes a data reader and passed conversion to ToListOf<> |


### Execute(cmd).ToListOf&lt;T>
The ExecuteTo will get the First or default of Type T. 

```C#
    public IList<ProductSummary> GetAllProductSummary()
    {
        var sql = @"Select ProductID, Name, ProductNumber, ReorderPoint, StandardCost, rowguid, ModifiedDate  
From  [Production].[Product]";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<ProductSummary>();
    }

```