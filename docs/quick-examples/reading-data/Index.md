## Samples

All samples given will use a local sql server database only the DataAccess Methods will be shown 

``` csharp
public class ExampleReadDataAsExecuteListOf : SqlServerDataAccess
{
    public ExampleReadDataAsExecuteListOf() : base(
        new DataAccessConfig("SampleConfig", new DataAccessConfigOptions { ConnectionStringKey = "notused" },
            new HardCodedConnectionStringsResolver("Server=(localdb)\\MSSQLLocalDB;Integrated Security = true"))
    )
    {

    }

    //... Methods in the samples
}
```


1. [Simple Select](./SimpleSelect.md)
2. [Simple Select With Parameters](./SimpleSelectWithParameters.md)
3. [Simple Single Row](./SimpleSingleRow.md)
4. [Simple Single Value](./SimpleSingleValue.md)
5. [Execute Stored Procedure](./ExecuteStoredProcedure.md)