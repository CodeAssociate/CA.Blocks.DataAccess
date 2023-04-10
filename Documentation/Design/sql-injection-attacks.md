## SQL injection

One of the key responsibilities that the CA.Blocks.DataAccess passes on to you as the developer is that of protecting against SQL injection attacks.  The framework exposes the full power of the underlying database through the execution of SQL, this SQL is 100% written by the developer with no restrictions. Using the blocks there is no direct way to execute a SQL statement from the calling code without writing a custom data access method.  Whilst the core design is protected by default, and all functions that execute the SQL commands are protected.  It is the custom DataAccess methods that invoke those protected methods that need to be written with SQL injection in mind. As the developer you may be tempted to expose this to avoid writing your own access methods by making the protected methods public. Doing this will open your code up to be injected.  Working directly with the SQL means as a developer you are responsible for the SQL generated this means responsibility for injection attacks. The simplest way to avoid injection attacks is not executing any SQL that is not 100% controlled by the code and using parameterized SQL everywhere

With full access to the underlying database means you as the developer are responsible for exposing what is executed.

### What is SQL Injection

SQL Injection is a technique that results in unauthorised SQL commands being executed against the database. SQL injection occurs as a result of taking input and concatenating it with SQL statements that form part of an application's code base. The user input has been crafted specifically to alter the SQL that the program's designer intended to execute.

#### SQL Injection By Example

The simplest way to illustrate this by way of an Example of a injectable SQL, consider the following method which is prone to SQL injection.

```C#
    public DataTable SQLInjectionExample_Bad(string lastName)
    {
        var sql = $"Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like '{lastName}'";
        var cmd = CreateTextCommand(sql);
        return ExecuteDataTable(cmd);
    }
```
 
 This is a made up example using the Adventure works schema. We have a method with a restriction of only getting data where the City = 'Bothell' but allowing the user to search by last name using the sql like wild card syntax. 

There is a problem here in that the code is building a SQL statement from untrusted input. To demonstrate how sql injection can be used we look at executing this method with different parameters :

```C#
result = SQLInjectionExample_Bad("N%")
```
This is what was designed for and it works with the expected input:
```SQL
Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like 'N%' 
```
from a functionality point on view this does what is expected.


Now let search for D'arbo ( Maryam D'arbo is the Bond girl in The Living Daylights) 

```C#
result = SQLInjectionExample_Bad("D'arbo")
```
This results in a syntax error 
```SQL
Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like 'D'arbo' 
```
If you expose errors from the database, this is the start of you hell, as you have just shown the world that there an Unclosed quotation mark after the character string resulting in an  incorrect syntax near 'arbo'. It you have just told the world you have injectable code. 


The hacker that wants to steel data will simply inject "' or  ''='"
```C#
result = SQLInjectionExample_Bad("' or  ''='")
```

There we terminate parameter with ' then add an or ''=' and leave the original ' in place the result is    

```SQL
Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like '' or  ''=''
```
In this statement we have bypassed the security check for City = 'Bothell'  and we have got everything in the table where ''=''. The query has just dumped all rows including ones that were restricted.

The Hacker that just wants to be nasty can shutdown you server:
```C#
result = SQLInjectionExample_Bad("'; SHUTDOWN WITH NOWAIT ; select '")
```
💥 if the account that has admin rights on your connection the SQL server is now shutting down. This is why Least privilege is important. 
```SQL
Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like ''; SHUTDOWN WITH NOWAIT ; select '' 
```

If we break this down 
```SQL
'; SHUTDOWN WITH NOWAIT ; select '
```
the hacker can add anything they like between the ; ; and if you have permissions the database will execute that statement.   

As such you never build and execute the SQL that contains anything that comes as a parameter. You should Parameterise all variables.


### Parameterise all variables 
When you pass ad hoc or user supplied values to your SQL command at run time, it is important to use parameters to represent them in order to prevent the possibility of your application being exposed to SQL injection attacks.

Example of Bad SQL with building SQL from strings
```C#
    public DataTable SQLInjectionExample_Bad(string lastName)
    {
        var sql = $"Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like '{lastName}'";
        var cmd = CreateTextCommand(sql);
        return ExecuteDataTable(cmd);
    }
```
Taking the same example but this time using a Parameterised query
```C#
    public DataTable SQLInjectionExample_WithInjectionProjection(string lastName)
    {
        var sql = $"Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like @lastName";
        var cmd = CreateTextCommand(sql).WithParameter(lastName.ToSqlParameter("@lastName"));
        return ExecuteDataTable(cmd);
    }
```
This is safer as the lastName will be treated as a string so will execute on the database as

```SQL
Declare @lastName varchar(max) 
select @lastName = '''; SHUTDOWN WITH NOWAIT ; select '''
Select * from [HumanResources].[vEmployee] where City = 'Bothell' and LastName like @lastName
```
This will safely return zero rows with no injection.  When they try to use D'arbo they will only results if D'arbo  exists else nothing. 


### Allowlist
There are some designs for better or worse that will require section on the SQL that cannot be placed into a parameter, an example is if you are building a SQL statement that can get data from a dynamic table. 

Example this is not allowed:
``` SQL
Select * from @MyTable  
```

In this case you cannot put the table data name as a parameter, however what you can do is provide a valid allowlist of values. You will also escape the name.  The allow list can be sourced externally example in the above statement you can query the database for all valid table names. Then only allow the query if the parameter is within those values:

As an example below we have a query that can execute a dynamic table into a DataTable.  The tableName is prone to a injection attack and can you cannot use parameters, So what we do here is establish allowlist of table names.  In this example we connect to the INFORMATION_SCHEMA.TABLES and pull all the know tables within a give schema.  we then check the value pass in matches the allowed list if it passed we allow the construct the sql that is prone to SQL injection as we have validated the input against an allowed list. 

```C#
    // This method is private it provide the allow list, this list can be hard coded or dynamic 
    private IList<string> GetAllowedListFor(string schema)
    {
        var sql = $"Select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = @schema";
        var cmd = CreateTextCommand(sql).WithParameter(schema.ToSqlParameter("@schema"));
        return Execute(cmd).ToSingleNamedColumnList<string>("TABLE_NAME");
    }

    // Example of execute a sql statement that cannot be parameterised but using a allow list to prevent an injection
    public DataTable SelectDynamicTableFromSalesSchema(string tableName)
    {
        var schema = "Sales";
        // we need to validate the tableName first against allowList.
        var allowedList = GetAllowedListFor(schema);
        if (allowedList.Any( x=> x == tableName))
        {
            var sql = $"Select * from [{schema}].[{tableName}]";
            var cmd = CreateTextCommand(sql);
            return ExecuteDataTable(cmd);
        }
        else
        {
            throw new DataException($"{tableName} is not allowed");
        }
    }
```



 

### Least privilege
Whilst the use of parameterised queries and whitelists provide good defence, the strongest defence comes from working with the database and providing an account with the least privilege access . The specifics of each database can change, however there are some common restrictions:
It is common for most databases to split the type of statement at a high level, typically there will be 

1. DML - Data Modification Language, e.g.  Select, Update, Insert and Delete type statements
2. DDL - Data Modification language, e.g. Create, Alter, Drop Rename and Truncate type statements  
3. DCL - Data Control Language, e.g.  Grant Revoke and Audit type statements

In most cases the permissions for the account used to access the data at the application level can be safely restricted to the DML Statements.  As such the application is denied access to all DDL and DCL statements. Doing this can reduce the attack surface dramatically as even with a successful injection the schema and permissions cannot be modified by the application.

Working with immutable data, the database can safely deny permission to the Delete and Update Statement, as the application should never execute such statements

Working with data that is read-only the database should only grant select permission.  It is very common for most master type data to be read-only from the applications perspective. 

Working with the database this concept can be extended to data horizontally through views or security grants.  

Working with Least privilege together with full SQL parameterised queries provides a robust data access model. 

