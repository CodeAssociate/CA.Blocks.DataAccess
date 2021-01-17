### Samples 

#### Simple Single Value

In this example we going to use the data from the local SQL server selecting the meta data data from the sysobjects table and executing the results into single return type.  


``` csharp
public class ExampleReadDataSingleValue: SqlServerDataAccess
{
    public ExampleReadDataSingleValue() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    public int GetSysObjectsCount()
    {
        var cmd = CreateTextCommand("Select count(*) from Sysobjects");
        return ExecuteScalarAs<int>(cmd);
    }

    public int? GetValueThatMightBeNull()
    {
        var cmd = CreateTextCommand("Select id from Sysobjects where 1=2"); // zero rows
        return ExecuteScalarAs<int?>(cmd);
    }

    public int? GetValueThatMightBeNull2()
    {
        var cmd = CreateTextCommand("Select null as col"); // 1 row value null
        return ExecuteScalarAs<int?>(cmd);
    }

    public int GetValueThatMustBeConverted()
    {
        var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
        return ExecuteScalarWithConvertAs<int>(cmd);
    }
}


```

There are four cases when executing the value to a Scalar values:
1. When you execute the result and know there will be one and only one value on a known type. If you know you working with a int you can use 
```
ExecuteScalarAs<int> 
```
Or is you know you working with DateTime you can use 
```
ExecuteScalarAs<DateTime>
```

2. Where you execute and the database returns zero rows. In this you will the default value of T. so int? will be null however int will be 0. 
3. Where you execute and the database returns a null value.  In this you will the default value of T. so int? will be null however int will be 0. 
4. When you know the value is one type in the database as you need to convert it to another value. example going from tinyint to int you can use
```
ExecuteScalarWithConvertAs<int>
```
When doing the conversion you will need to deal with conversion errors , ie tinyint to int will always work, but int to byte will not always work.


