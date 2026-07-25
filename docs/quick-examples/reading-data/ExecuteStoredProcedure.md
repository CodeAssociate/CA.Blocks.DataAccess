---
layout: default
title:  Execute a Stored Procedure
nav_exclude: true
---

### Samples 

#### Execute a Stored Procedure

The CA Data Access Blocks can execute SQL Server stored procedures. A stored procedure in SQL Server is a group of one or more Transact-SQL statements that execute on the server.

The main benefits of using stored procedures include:
1. Reduced network traffic
2. Isolated security, as you can grant permissions on the stored procedure without permission on the underlying tables or views.
3. Arguably increased performance as the query plan is cached.  (This one is subjective as you can write bad tSQL procedures given the limited option in TSQL and the cache plans with parameterised queries are very fast, the main performance benefits when comparing parameterised queries  and Stored procedures is Reduced network traffic  )


The basic setup of a stored procedure is calling the CreateStoredProcedureCommand, this sets up a  stored Procedure Command and sets the name of the stored procedure to the input value. In the example below we are setting the stored procedure to execute the results into a System.DataTable

``` csharp
public DataTable ExecuteSpwhoWithNoReturnValue()
{
    var cmd = CreateStoredProcedureCommand("sp_who");
    return  ExecuteDataTable(cmd);
}
```
If the Stored procedures produces a tabular result the result can be executed directly into objects just as standard queries. example using the same call we can the results of stored procedures into the Class below 

``` csharp
public class SpWhoResult
{
    public int spid { get; set; }
    public string status { get; set; }
    public string loginame { get; set; }
    public string hostname { get; set; }
    public string blk { get; set; }
    public string dbname { get; set; }
    public string cmd { get; set; }
    public int request_id { get; set; }
}

public IList<SpWhoResult> ExecuteSpwhoWithNoReturnValue()
{
    var cmd = CreateStoredProcedureCommand("sp_who");
    return  ExecuteToListOf<SpWhoResult>(cmd);
}
```

In addition, stored procedures can return results through parameters. There are:

1. Standard input parameters
2. Output Parameters 
3. Input Output Parameters
4. Return Values.



### Standard input parameters

Standard input parameters are like any other command parameters
to execute the following SQL 

``` sql
execute sp_who 'sa' 
```

Use
``` csharp
public  IList<SpWhoResult> ExecuteSpWhoWithParameter()
{
    string loginName = "sa";
    var cmd = CreateStoredProcedureCommand("sp_who").WithParameter(loginName.ToSqlParameter("@loginame"));
    return ExecuteToListOf<SpWhoResult>(cmd);
}
```


### Output parameters
Output parameters are variables that will be set as part of the Stored procedure execution and returned via the parameters object
Example:

Given the following stored procedure
``` sql
CREATE PROCEDURE CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output @TestOutputValue INT OUTPUT AS
BEGIN
    SELECT @TestOutputValue = 123
END
```
You can use the following SQL to call that code
``` sql
Declare @TestOutputValue as INT

execute CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output  @TestOutputValue OUTPUT

Select @TestOutputValue
```

The result value is simply be 123. 



To call this in C#, you need to define a parameter to execute into. The simplest way is setting up a normal input parameter and converting the parameter to an output. In the example below, we will be using an `intOutput` to set up a parameter, converting it to an output value. Once the command has been executed, you read the value of the `sqlOutputParamParam`:
``` csharp
public int Execute_CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output()
{
    int intOutput = 0;
    var sqlOutputParamParam = intOutput.ToSqlParameter("@TestOutputValue").AsOutput();
    var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output").WithParameter(sqlOutputParamParam);
    ExecuteNonQuery(cmd);
    return sqlOutputParamParam.ToValue<int>();
}
```
The result value is simply be 123. 

### InputOutput parameters
InputOutput parameters are variables that will be passed in set as part of the Stored procedure execution and returned via the parameters object.  Form the C# perspective they behave much like output parameters:

Example:

Given the following stored procedure
``` sql
CREATE PROCEDURE CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput @TestOutputValue INT OUTPUT AS
BEGIN
    SELECT @TestOutputValue = @TestOutputValue * 2
END
```
You can use the following SQL to call that code
``` sql
Declare @TestOutputValue as INT
Select @TestOutputValue = 123

execute CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput  @TestOutputValue OUTPUT

Select @TestOutputValue
```

The result value wil be 246.  


Like output parameters, to call this in C#, you need to define a parameter to execute into. The simplest way is setting up a normal input parameter and converting the parameter to an `InputOutput`. In the example below, we will be using an `intInput` to set up a parameter; in doing this, the input value is set. Converting to an `InputOutput` value allows us to set up the return result. Once the command has been executed, you read the value of the `sqlInOutParam`:
``` csharp
public int CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput()
{
    int intInput = 123;
    var sqlInOutParam = intInput.ToSqlParameter("@TestOutputValue").AsInputOutput();
    var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput").WithParameter(sqlInOutParam);
    ExecuteNonQuery(cmd);
    return sqlInOutParam.ToValue<int>();
}
```
The result value will be 246.


### Return Value
Return values can be used within stored procedures to provide the stored procedure execution status to the calling program. The return value is always an integer. You can create your own meaning around the return values. By default, the successful execution of a stored procedure will return 0.

Example

``` csharp
public int ExecuteSpWhoWithReturnValue()
{
    var cmd = CreateStoredProcedureCommand("sp_who").WithReturnResult();
    ExecuteDataTable(cmd);
    return cmd.GetReturnResult();
}

```
This will return 0 as a successful execution
