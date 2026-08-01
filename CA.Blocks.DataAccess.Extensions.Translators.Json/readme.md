[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/)
- [Source Code](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

This Package is a extension to the DataAccess Blocks, it will pull in the System.Text.Json to allow reading of Json Data Directory from a Column

To use: 
Direct Usage .NET standard a null value will be returned as Default(YourObject)
``` C#
    var target = new JsonDbColToTypeConverter<YourObject>();
    var r = target.GetDataValue(dataRow, "col");

    // r will be a type of YourObject

    //or if you have we what to use the custom JsonSerializerOptions example PropertyNameCaseInsensitive
    var target = new JsonDbColToTypeConverter<YourObject>(new JsonSerializerOptions{ PropertyNameCaseInsensitive = true});
    var r = target.GetDataValue(dataRow, "col");
```

Direct Usage .NET 6 + can be as above by also detail with null values and interfaces
``` C#
    var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<MyObject>>
                (
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                    () => new List<MyObject>()
                );

    var r = target.GetDataValue(dataRow, "col");

    // r will be a type of IReadOnlyList<MyObject> if the db is empty backing object will be List<MyObject> but returned as IReadOnlyList<MyObject>
```


Register as a Translator
``` C# 
   DefaultDbColToTypeProvider.DefaultInstance.Add(new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));

```

In .NET standard you have to use concrete types, with .net core 6+ you can use Interface Types