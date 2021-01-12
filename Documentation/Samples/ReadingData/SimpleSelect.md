### Samples 

#### Simple Select

In this example we going to use the data from the local SQL server selecting the data from the sysobjects table and executing the results into the .NET class called ExampleSysObject

``` csharp
public class ExampleSysObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string XType { get; set; }
    public DateTime CreateDate { get; set; }
}
```

Template code using SQL server. 
``` csharp
public class ExampleReadDataAsExecuteListOf : SqlServerDataAccess
{
    public ExampleReadDataAsExecuteListOf() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    public IList<ExampleSysObject> ReadSysObjects()
    {
    var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects");
    return ExecuteToListOf<ExampleSysObject>(cmd);
    }
}
```