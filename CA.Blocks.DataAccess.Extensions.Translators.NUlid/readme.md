[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package ](https://www.nuget.org/packages/CA.Blocks.DataAccess.Extensions.Translators.NUlid)
- [Source Code](https://dev.azure.com/RavinEnterprises/CA.Blocks/_git/CA.Blocks.DataAccess)

A Package that provides support for the NUlid dataType

You will  need to Register the converter for the Blocks pick it up automatically

``` C#
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;


// On startUp in your project
DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());

``` 

The Storage of a NUlid can be either a text value of 26 characters in length or binary(16) 
when working with parameters
``` C#
myUlidValue = new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V")
// if sotring as myUlid char(26) use 
cmd.Parameters.Add(myUlidValue.AsString().ToSqlParameter("@myUlidValue"));

// if storing myUlid as binary(16) use 
cmd.Parameters.Add(myUlidValue.AsByteArray().ToSqlParameter("@myUlidValue"));
//myUlid binary(16)
```
