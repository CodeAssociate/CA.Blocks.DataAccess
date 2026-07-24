
CA.Blocks.DataAccess has been published to NuGet. First thing that you need to decide is which provider we want to use. 

For SQL server https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/

```
PM> Install-Package CA.Blocks.SQLServerDataAccess -Version x.x.x.
```

For SQL Microsoft.Data.Sqlite https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/

```
PM> Install-Package CA.Blocks.SqliteDataAccess -Version x.x.x
```


For SQL MySQL https://www.nuget.org/packages/CA.Blocks.MySQLDataAccess/

```
PM> Install-Package CA.Blocks.MySQLDataAccess -Version x.x.x
```

The second thing you need to do is set up a connection string, see [Connection String Examples](../quick-examples/connection-configuration/Index.md)

Then you will be ready to work with the DataAccess Class

### Template example accessing SQL server  

In this example, we are going to use the data from the local SQL Server, selecting the data from the `sysobjects` table and executing the results into the .NET class `ExampleSysObjects` below:

``` csharp
public class ExampleSysObjects
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

    public IList<ExampleSysObjects> ReadSysObjectsOfType(string xtype)
    {
        var cmd = CreateTextCommand("Select top 10 id as Id, name as Name, xtype as XType, crdate as CreateDate from sysobjects where xtype = @xtype").WithParameter(xtype.ToSqlParameter("@xtype"));
        return ExecuteToListOf<ExampleSysObjects>(cmd);
    }
}
```
Notes:
1. The class inherits from `SqlServerDataAccess`, which is the SQL Server provider. 
2. The example above is using the provided `HardCodedConnectionStringsResolver`, this is provided for quick prototyping, samples and testing code allowing connection to be specified in line with the code. It is recommended you use an external connection string when working on something that is to be published, see [Connection String Examples](../quick-examples/connection-configuration/Index.md)
3. The `ReadSysObjectsOfType` method represents your data access method; the only input parameter exposed is `xtype`, and the return type will be an `IList<ExampleSysObjects>`.
4. The `CreateTextCommand` will return an interface to the SQL Server implementation of the command.
5. The SQL is constructed internally as a parameterized query; this is a developer responsibility.
6. The conversion of a .NET string to a SQL parameter is done in `.WithParameter(xtype.ToSqlParameter("@xtype"))`; you can also use the connection object directly, i.e., `cmd.Parameters.Add(xtype.ToSqlParameter("@xtype"))`.
7. The `ToSqlParameter` is a convention used for taking a .NET type into a SQL Server parameter. All .NET value types will have implementations of `ToSqlParameter()`.
8. The `cmd` is then passed into the `ExecuteToListOf` method, which returns the data as an `IList<ExampleSysObjects>`. As we have 1:1 mapping, the conversion is handled 100% by the blocks. 
9. The property names are case-sensitive, so in this example, we have aliased the columns on the query side, i.e., `id as Id`. This `Id` is the property name on the target object. You only have to do this if you are using 100% automatic conversions.

Consuming this class: 
``` csharp

[Test]
public void ExecuteToListOfDev()
{
    var target = new ExampleReadDataAsExecuteListOf(); 
    var executeResult = target.ReadSysObjectsOfType("U"); 

    foreach (var o in executeResult)
    {
        TestContext.WriteLine($"{o.Id},{o.Name},{o.XType},{o.CreateDate}");
    }
}
```
Notes:
1. You construct the instance of the DataAccess class `ExampleReadDataAsExecuteListOf`.
2. You call the method `ReadSysObjectsOfType("U")`. The only methods you see are public ones from `System.Object` and the `ReadSysObjectsOfType`. This is by design: the guts of the DataAccess class is **protected by default**. The instance of the DataAccess can access the method, but the calling client only sees what is exposed. The calling code cannot call `ExecuteToListOf`.
<p align="center">
    <img src="_assets/ProtectedByDefaultExample.png" alt="ProtectedByDefault" />
</p>
3. The result of the execution is the filled `IList<ExampleSysObjects>`. All types have been converted from the SQL world into the .NET world.
4. Using the result is like using any other class in .NET. In this case, we are dumping the result to the test console:

The dump result
```
-463397375,trace_xe_action_map,U ,30/04/2016 12:44:47 AM
-319884821,trace_xe_event_map,U ,30/04/2016 12:44:46 AM
117575457,spt_fallback_db,U ,8/04/2003 9:18:01 AM
133575514,spt_fallback_dev,U ,8/04/2003 9:18:02 AM
149575571,spt_fallback_usg,U ,8/04/2003 9:18:04 AM
1483152329,spt_monitor,U ,30/04/2016 12:46:37 AM
1787153412,MSreplication_options,U ,30/04/2016 12:47:59 AM
```