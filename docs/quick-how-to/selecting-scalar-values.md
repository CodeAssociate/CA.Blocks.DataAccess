---
layout: default
title: Selecting Scalar Values
description: "Selecting Scalar Values"
parent: How to
nav_order: 2
---

## Selecting Scalar Values

A scalar value refers to a single value, for example, a string or a number. So the underlying query will return a single value that will need to be converted. 
CA.Blocks provides two methods for selecting scalar values along with their asynchronous counterparts.


| Method      | Description |
| ----------- | ----------- |
| [ExecuteScalarAs<T>(cmd)](#executescalarast)   | Returns an object cast as type `T`|
| [ExecuteScalarAsAsync<T>(cmd)](#executescalarasasynct)   | Returns an object cast as type `T` async|
| [ExecuteScalarWithConvertAs<T>(cmd)](#executescalarwithconvertast)   | Returns an object converted to type `T`|
| [ExecuteScalarWithConvertAsAsync<T>(cmd)](#executescalarwithconvertasasynct)   | Returns an object converted to type `T` async|
| [ExecuteScalar(cmd)](#executescalar)     | Returns an object|



### ExecuteScalarAs<T>
The vast majority of the time you are going to know the return type; in this case, you can use `ExecuteScalarAs<T>`. This is the fastest method to call; however, the object is cast to the expected type, so you need to match the return type with the type returned by the data source.

In the example below, we will return an integer value as a count of `[Production].[Product]`. So once we have created the command, we call `ExecuteScalarAs<int>(cmd)`; this will get the value as an integer and cast the result value as an integer. 
``` C#
    public int GetProductionProductCount()
    {
        var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
        return ExecuteScalarAs<int>(cmd);
    }
```
### ExecuteScalarAsAsync<T>
Async version of `ExecuteScalarAs<T>`

``` C#
    public Task<int> GetProductionProductCountAsync()
    {
        var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
        return ExecuteScalarAsAsync<int>(cmd);
    }
```


### ExecuteScalarWithConvertAs<T>

There are times when the result type from the source system is not the desired type. In the example below, the type coming back from the source system is a byte. We may want to return the type as a string. In this case, we can use the `ExecuteScalarWithConvertAs<string>` function. This will get the value from the system as a byte but will convert the value to a string.

``` C#
        public string GetValueThatMustBeConvertedToString()
        {
            // Here we are getting a value as a byte from the server but returning the value as a string
            var cmd = CreateTextCommand("Select Cast(123 as tinyint) as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<string>(cmd);
        }
```
As an example of this in reverse where the source type is a string but you need the data as a byte:

``` C#
        public byte GetValueThatMustBeConvertedToByte()
        {
            // Here we are getting a value as a string from the server but returning the value as a byte
            var cmd = CreateTextCommand("Select '123' as ExampleOfConvert");
            return ExecuteScalarWithConvertAs<byte>(cmd);
        }
```

- Note: you will get a conversion exception if the conversion is not possible.
```C#
     public byte GetValueThatMustBeConvertedToByt_Exception()
        {
            // Here we are getting a value as a string from the server but returning the value as a byte. 
            // The string value "1234" cannot be converted to a byte.
            var cmd = CreateTextCommand("Select '1234' as ExampleOfConvert");
            // This will throw a conversion exception
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