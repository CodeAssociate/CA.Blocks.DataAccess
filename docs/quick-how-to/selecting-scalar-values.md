---
layout: default
title: Selecting Scalar Values
description: "Selecting Scalar Values"
parent: How to
nav_order: 1
---

{: .no_toc .text-delta }
## Scalar Methods Overview

{:toc_levels="3"}

1. TOC
{:toc}

---
A scalar value refers to a single value, for example, a string or a number. So the underlying query will return a single value that will need to be converted. 
CA.Blocks provides two methods for selecting scalar values along with their asynchronous counterparts.

### ExecuteScalarAs\<T\>
*Returns an object cast as type `T`*

The vast majority of the time you are going to know the return type; in this case, you can use `ExecuteScalarAs<T>`. This is the fastest method to call; however, the object is cast to the expected type, so you need to match the return type with the type returned by the data source.

In the example below, we will return an integer value as a count of `[Production].[Product]`. So once we have created the command, we call `ExecuteScalarAs<int>(cmd)`; this will get the value as an integer and cast the result value as an integer. 
```csharp
public int GetProductionProductCount()
{
    var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
    return ExecuteScalarAs<int>(cmd);
}
```
### ExecuteScalarAsAsync\<T\>
*Returns an object cast as type T async*

Async version of `ExecuteScalarAs<T>`

```csharp
public async Task<int> GetProductionProductCountAsync()
{
    var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
    return await ExecuteScalarAsAsync<int>(cmd);
}
```

### ExecuteScalarWithConvertAs\<T\>
*Returns an object converted to type T*

There are times when the result type from the source system is not the desired type. In the example below, the type coming back from the source system is a byte. We may want to return the type as a string. In this case, we can use the `ExecuteScalarWithConvertAs<string>` function. This will get the value from the system as a byte but will convert the value to a string.

```csharp
public string GetValueThatMustBeConvertedToString()
{
    // Here we are getting a value as a byte from the server but returning the value as a string
    var cmd = CreateTextCommand("Select Cast(123 as tinyint) as ExampleOfConvert");
    return ExecuteScalarWithConvertAs<string>(cmd);
}
```
As an example of this in reverse where the source type is a string but you need the data as a byte:

```csharp
public byte GetValueThatMustBeConvertedToByte()
{
    // Here we are getting a value as a string from the server but returning the value as a byte
    var cmd = CreateTextCommand("Select '123' as ExampleOfConvert");
    return ExecuteScalarWithConvertAs<byte>(cmd);
}
```

- Note: you will get a conversion exception if the conversion is not possible.
```csharp
 public byte GetValueThatMustBeConvertedToByt_Exception()
{
    // Here we are getting a value as a string from the server but returning the value as a byte. 
    // The string value "1234" cannot be converted to a byte.
    var cmd = CreateTextCommand("Select '1234' as ExampleOfConvert");
    // This will throw a conversion exception
    return ExecuteScalarWithConvertAs<byte>(cmd);
}
```
### ExecuteScalarWithConvertAsAsync\<T\>
*Returns an object converted to type T async*

```csharp
public async Task<string> GetValueThatMustBeConvertedToString()
{
    // Here we are getting a value as a byte from the server but returning the value as a string
    var cmd = CreateTextCommand("Select Cast(123 as tinyint) as ExampleOfConvert");
    return await ExecuteScalarWithConvertAsAsync<string>(cmd);
}
```

### ExecuteScalar
*Returns an object*

The ExecuteScalar will return the value directly as an object this case you can deal with the conversion as needed.  This method is simply managing the connection leaving the code to deal with the conversion.
```csharp
public object GetSysObjectsCount()
{
    var cmd = CreateTextCommand("select count(*) from [Production].[Product]");
    return ExecuteScalar(cmd);
}
```