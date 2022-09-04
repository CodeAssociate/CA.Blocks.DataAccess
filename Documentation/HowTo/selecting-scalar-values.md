## Selecting Scalar Values

A scalar value refers to a single value. For example, string number. So the underlying query will return a single value that will be need to be converted. 
CA.Blocks provides two methods for selecting scalar values along with their asynchronous counterparts.


| Method      | Description |
| ----------- | ----------- |
| [ExecuteScalarAs&lt;T>(cmd)](#executescalarast)   | returns an object cast as type T|
| [ExecuteScalarAsAsync&lt;T>(cmd)](#executescalarasasynct)   | returns an object cast as type T async|
| [ExecuteScalarWithConvertAs&lt;T>(cmd)](#executescalarwithconvertast)   | returns an object converted to type T|
| [ExecuteScalar(cmd)](#executescalar)     | returns an object|



### ExecuteScalarAs&lt;T>
The vast majority of the time you are going to know return type in the in this case you can use the ExecuteScalarAs<T> this is the fastest method to call however the object is as cast as a expected type as such you need to match the return type with the type given from the data source

In the example below we will return a integer value as a count of the sysobjects so once we have created the command we call ExecuteScalarAs<int>(cmd) this will get the values as a integer ans cast the result value as a integer. 
``` C#
    public int GetSysObjectsCountWithType()
    {
        var cmd = CreateTextCommand("Select count(*) from sysobjects");
        return ExecuteScalarAs<int>(cmd);
    }
```
### ExecuteScalarAsAsync&lt;T>
Async version of ExecuteScalarAs

``` C#
    public Task<int> GetSysObjectsCountWithType()
    {
        var cmd = CreateTextCommand("Select count(*) from sysobjects");
        return ExecuteScalarAsAsync<int>(cmd);
    }
```


### ExecuteScalarWithConvertAs&lt;T>

There times where the result type from the source system is not the desired type. In the example below they type coming back form the source system is a byte. We may what to return they type as string. In this case we can we can use the ExecuteScalarWithConvertAs<string> function.  This will get the value from the system as a byte but will convert the value to string  

``` C#
    public string GetValueThatMustBeConverted()
    {
        var cmd = CreateTextCommand("Select Cast(123 as tinyint) as col");
        return ExecuteScalarWithConvertAs<string>(cmd);
    }
```

### ExecuteScalar
The ExecuteScalar will return the value directly as a object this case you can deal with the conversion as needed.  This method is simple managing the connection leaving the code to deal with the conversion.
```C#
    public object GetSysObjectsCount()
    {
        var cmd = CreateTextCommand("Select count(*) from Sysobjects");
        return ExecuteScalar(cmd);
    }
```