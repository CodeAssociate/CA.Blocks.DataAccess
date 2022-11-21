## Selecting Scalar Values

A scalar value refers to a single value. For example, string number. So the underlying query will return a single value that will need to be converted. 
CA.Blocks provide two methods for selecting scalar values along with their asynchronous counterparts.


| Method      | Description |
| ----------- | ----------- |
| [ExecuteScalarAs&lt;T>(cmd)](#executescalarast)   | returns an object cast as type T|
| [ExecuteScalarAsAsync&lt;T>(cmd)](#executescalarasasynct)   | returns an object cast as type T async|
| [ExecuteScalarWithConvertAs&lt;T>(cmd)](#executescalarwithconvertast)   | returns an object converted to type T|
| ExecuteScalarWithConvertAsAsync&lt;T>(cmd)   | returns an object converted to type T|
| [ExecuteScalar(cmd)](#executescalar)     | returns an object|



### ExecuteScalarAs&lt;T>
The vast majority of the time you are going to know the return type, in this case, you can use the ExecuteScalarAs<T> this is the fastest method to call however the object is as cast as an expected type as such you need to match the return type with the type given from the data source

In the example below we will return an integer value as a count of the [Production].[Product] so once we have created the command we call ExecuteScalarAs<int>(cmd) this will get the values as an integer and cast the result value as an integer. 
``` C#
    public int GetProductionProductCount()
    {
        var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
        return ExecuteScalarAs<int>(cmd);
    }
```
### ExecuteScalarAsAsync&lt;T>
Async version of ExecuteScalarAs

``` C#
    public Task<int> GetProductionProductCountAsync()
    {
        var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
        return ExecuteScalarAsAsync<int>(cmd);
    }
```


### ExecuteScalarWithConvertAs&lt;T>

There are times when the result type from the source system is not the desired type. In the example below the type coming back from the source system is a byte. We may what to return the type as a string. In this case, we can use the ExecuteScalarWithConvertAs<string> function.  This will get the value from the system as a byte but will convert the value to a string.

``` C#
        public string GetValueThatMustBeConvertedToString()
        {
            // Here we getting a values as a byte from the server but returning the values as a string
            var cmd = CreateTextCommand("Select Cast(123 as tinyint) as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<string>(cmd);
        }
```
As an example of this in reverse where the source type in a string but you need the data as a byte.

``` C#
        public byte GetValueThatMustBeConvertedToByte()
        {
            // Here we getting a values as a string from the server but returning the values as a byte
            var cmd = CreateTextCommand("Select '123' as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<byte>(cmd);
        }
```

- Note you will get a convert exception if the conversion is not possible 
```C#
     public byte GetValueThatMustBeConvertedToByt_Exception()
        {
            // Here we getting a values as a byte from the server but returning the values as a string the string value cannot be converted to a byte.
            var cmd = CreateTextCommand("Select '1234' as ExampleOfConvert");
            // this will 
            return ExecuteScalarWithConvertAs<byte>(cmd);
            
        }
```

### ExecuteScalar
The ExecuteScalar will return the value directly as an object this case you can deal with the conversion as needed.  This method is simply managing the connection leaving the code to deal with the conversion.
```C#
    public object GetSysObjectsCount()
    {
        var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
        return ExecuteScalar(cmd);
    }
```