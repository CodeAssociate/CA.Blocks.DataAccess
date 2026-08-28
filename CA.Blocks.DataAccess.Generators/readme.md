[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess.Generators?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess.Generators/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-purple)[![NuGet version (CA.Blocks.DataAccess.Generators)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.Generators.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess.Generators)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package ](https://www.nuget.org/packages/CA.Blocks.DataAccess.Extensions.Translators.NUlid)
- [Source Code](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

A Package that provides support for AOT Code Generator for CA.Blocks.DataAccess.
Add a Package Reference to your project
``` C#
    <PackageReference Include="CA.Blocks.DataAccess.Generators" Version="3.9.+" OutputItemType="Analyzer" ReferenceOutputAssembly="true" />
```
Then Mark your SampleCustomer with the GenerateDbRowTranslator
``` C#
[GenerateDbRowTranslator]
public class SampleCustomer
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public DateTime CreatedDate { get; init; }
}
```
The Compiler will generate and register the class SampleCustomerDbRowTranslator you can then use it to translate SampleCustomer
``` C#
     var result = await ExecuteAsync(cmd).ToListOf<SampleCustomer>();
```



