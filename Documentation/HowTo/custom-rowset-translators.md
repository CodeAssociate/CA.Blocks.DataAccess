### Custom Row Translators  

A core part of the CA.Blocks.DataAccess functionality is reducing the object–relational impedance mismatch that exists between the relational world and the object world of .NET.   In the relational world the data structures revolve around sets of tables. These are setup each table having rows and columns, each intersection of a row with a column has a cell value. Cells come from a finite number of simple data types, like strings, numbers dates. The tables are all linked with primary and foreign keys.  In the object world the data structures are far more rich with objects having properties, a property can be any value including other objects. The properties themselves reflect the relationships between objects, as such ther is no real concept of foreign keys.    

The Row Translators have the responsibility of mapping the Table structure which is rows, columns and cells into the class structure of class, and properties.  The focus on the Row Translators is at the structure level.  See the column translators for the cell mappings. 


<div style="text-align:center;">
<span style="min-height:128px;display:inline-flex;align-items:center;border: 1px solid aqua;background-color:white;" >
    <img src="../_assets/Table.svg" alt="Table" width=128 /> 
    ==➤
    <img src="../_assets/Class.svg" alt="class" width=128 />
</span>
</div>

### One to one mapping

In this case we going to look at the case where the table structure is aligned with the class structure 
The SQL Table Structure
```sql
    Create TABLE MyTable (
        [Id]  int not null,
        [Name] nvarchar(64) not null,
        [Status] tinyint not null,
        [Quantity] decimal(18,2) null,
        [Modified] Datetime2(7) null
    )
```
The .NET Class Structure
``` Csharp
    public class MyClass
    {
        public int Id  {get; init;}
        public int Name  {get; init;}
        public byte Status  {get; init;}
        public decimal? Quantity  {get; init;}
        public DateTime? Modified  {get; init;}
    }
```

Using the Blocks as this is a 1-1 mapping with matching dataTypes we can simply call execute the command and Translate the object direct from the reader to the class.  

```C#
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var cmd = CreateTextCommand("Select * From MyTable");
        return Execute(cmd).ToListOf<MyClass>();
    }
```
This setups the command that will be executed. The command in the case is 'Select * From MyTable'  this will come back from the source as a DataReader The Reader can then read the rows, getting the columns as the command was setup there is knowledge of the expected columns returned ie the code knowns and expects there will be a column called 'Name' in the reader.  

with with command you can almost read what the code is going to do in the translator if if we break down the statement 'Execute(cmd).ToListOf<MyClass>();'

We saying execute this command "Select * From MyTable", to a List Of MyClass objects, in this case you not getting back a Table structure with rows and columns and cells  byt are getting back a IList of MyClass this has been converted into the .NET world ov objects.

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
``` Csharp
    public class MyClass
    {
        public int Id  {get; init;}
        public int Name  {get; init;}
        public int Status  {get; init;}
        public decimal? Quantity  {get; init;}
        public DateTime? ModifiedAt  {get; init;}
    }
```

To make use of automatic mapping there are three options:
1) Alias the query in SQL to make it look like the object
2) Provide mapping attributes to the class in .NET
3) Implement a Custom Mapping Function

#### using SQL to alias 
```C#
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

``` C#
    public class MyClass
    {
        [DbColToSourceName("MyTableId")]
        public int Id  {get; init;}
        [DbColToSourceName("MyTableName")]
        public int Name  {get; init;}
        [DbColToTypeConverter(typeof(IntDbColToTypeConverter))]
        public int Status  {get; init;}
        public decimal? Quantity  {get; init;}
        [DbColToSourceName("Modified")]
        public DateTime? ModifiedAt  {get; init;}
    }
```
In this case we have simply turned te mapping around providing the mapping info on the .NET side. With This in place we can execute the query.

```C#
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>();
    }
```

#### The custom mapping

The Most powerful and most flexible option is to use a custom function for the mapping

This can be either be a lamba or function 

Using a lamba
```C#
    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>(reader => new MyClass
        {
            Id = reader.AsInt("MyTableId"),
            Name = reader.AsString("MyTableName"),
            Status = reader.AsInt("Status"),
            Quantity = reader.AsNullQuantity("Quantity"),
            ModifiedAt = reader.ASDateTime("Modified")
        }
        );
    }
```

Using a function.  The Key advantage for function is the you can reuse the conversion in other places.
```C#
    private MyClass MyCustomConvert(DataReader reader)
    {
        return new MyClass
        {
            Id = reader.AsInt("MyTableId"),
            Name = reader.AsString("MyTableName"),
            Status = reader.AsInt("Status"),
            Quantity = reader.AsNullQuantity("Quantity"),
            ModifiedAt = reader.ASDateTime("Modified")
        }
    }

    public IList<MyClass> GetMyClassFromMyTable()
    {
        var sql = @"Select * From MyTable";
        var cmd = CreateTextCommand(sql);
        return Execute(cmd).ToListOf<MyClass>(MyCustomConvert);
    }
```

