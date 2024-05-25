[![NuGet version (CA.Blocks.DataAccess)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package SQL Server](https://www.nuget.org/packages/CA.Blocks.SQLServerDataAccess/)
- [NuGet Package My SQL](https://www.nuget.org/packages/CA.Blocks.MySQLDataAccess/)
- [NuGet Package SQlite](https://www.nuget.org/packages/CA.Blocks.SQLLiteDataAccess/)

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
    var target = new NullJsonDbColToTypeConverter<IYourObject>(new JsonSerializerOptions{ PropertyNameCaseInsensitive = true});
    var r = target.GetDataValue(dataRow, "col");

    // r will be a type of IYourObject? and will can be null is null in the DB

```


Register as a Translator
``` C# 
   DefaultDbColToTypeProvider.DefaultInstance.Add(new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));

```

In .NET standard you have to use concrete types, with .net core 6+ you can use Interface Types