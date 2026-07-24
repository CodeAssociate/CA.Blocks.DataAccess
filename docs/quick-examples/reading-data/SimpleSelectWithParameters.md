### Samples 

#### Simple Select With Parameters

In this example, we are going to use the data from the local SQL Server, selecting the data from the `sysobjects` table and executing the results into the .NET class called `ExampleSysObjects`; this time, we will take in the `xType` as a parameter to filter by.

The Target Class
``` csharp
public class ExampleSysObjects
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string XType { get; set; }
    public DateTime CreateDate { get; set; }
}
```


``` csharp
public class ExampleReadDataAsExecuteListOf : SqlServerDataAccess
{
    public ExampleReadDataAsExecuteListOf() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    // Option 1 using cmd.Parameters.Add.
    public IList<ExampleSysObjects> ReadSysObjectsOfType(string xtype)
    {
        var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype");
        cmd.Parameters.Add(xtype.ToSqlParameter("@xtype"));
        return ExecuteToListOf<ExampleSysObjects>(cmd);
    }

    // Option 2 using WithParameters for Many Parameters
    public IList<ExampleSysObjects> ReadSysObjectsOfType2(string xtype)
    {
        var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype")
            .WithParameters(new List<SqlParameter> {xtype.ToSqlParameter("@xtype")});
        return ExecuteToListOf<ExampleSysObjects>(cmd);
    }
    // Option 3 using WithParameter for Single Parameter
    public IList<ExampleSysObjects> ReadSysObjectsOfType3(string xtype)
    {
        var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype")
            .WithParameter(xtype.ToSqlParameter("@xtype"));
        return ExecuteToListOf<ExampleSysObjects>(cmd);
    }

    public ExampleSysObjects GetSysObjectById(int Id)
    {
        var cmd = CreateTextCommand("Select 1 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where Id = @Id")
            .WithParameter(Id.ToSqlParameter("@Id"));
        return ExecuteTo<ExampleSysObjects>(cmd);
    }
}
```

It is recommended to use parameterized queries. A parameterized query (aka a prepared statement) is a means of pre-compiling a SQL statement so that all you need to supply are the "parameters" into the statement for it to be executed. The primary benefit is that it is a robust way of avoiding SQL injection attacks. It has the side benefit of performance, as the database can pre-compile and cache a prepared version for execution.

Above you see three different examples of setting the parameters and two data types. You can either add the parameters to the command object before execution, i.e.:
```
 cmd.Parameters.Add(xtype.ToSqlParameter("@xtype"));
```
Or use the  .WithParameters extension method.
```
 .WithParameters(new List<SqlParameter> {xtype.ToSqlParameter("@xtype")});
```
```
 .WithParameter(xtype.ToSqlParameter("@xtype"));
```


[see full list of SQL server ToSqlParameter extensions](https://www.codeassociate.com/Blocks/DataAccess/Api/class/CA.Blocks.SQLServerDataAccess.SqlServerParameterExtensions.html)
 