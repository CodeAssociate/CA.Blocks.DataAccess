[![NuGet Downloads](https://img.shields.io/nuget/dt/CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure?color=blue&label=NuGet%20Downloads)](https://www.nuget.org/packages/CA.Blocks.DataAccess/)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple)[![NuGet version (CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure)](https://img.shields.io/nuget/v/CA.Blocks.DataAccess.svg?style=flat-square)](https://www.nuget.org/packages/CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure/)
[![Build Status](https://dev.azure.com/RavinEnterprises/CA.Blocks/_apis/build/status/CA.Blocks.DataAccess?branchName=master)](https://dev.azure.com/RavinEnterprises/CA.Blocks/_build/latest?definitionId=2&branchName=master)

- [Homepage](https://www.codeassociate.com/)
- [Documentation](https://www.codeassociate.com/Blocks/DataAccess/)
- [NuGet Package Sqlite](https://www.nuget.org/packages/CA.Blocks.DataAccess.Extensions.TokenResolvers.Azure/)
- [Source Code](https://github.com/CodeAssociate/CA.Blocks.DataAccess)

This Package is an extension to the DataAccess Blocks, it will allow using an Azure Identity to connect to an Azure SQL Database

You need to install the package CA.Blocks.DataAccess before installing this package.

The reccomended way to wire this package is to use a cusom class that implements the IData dependency injection.

```csharp
public class DataAccessConfig : IDataAccessConfig
{
    public DataAccessConfig(IDataAccessConfigOptions options, IDataAccessKeyToConnectionStringResolver resolver, 
        DependencyInjection.IConnectionTokenResolver connectionTokenResolver)
    {
        Options = options;
        Resolver = resolver;
        ConnectionTokenResolver = connectionTokenResolver;
    }

    public IDataAccessConfigOptions Options { get; }
    public DependencyInjection.IConnectionTokenResolver ConnectionTokenResolver { get; }
    public IDataAccessKeyToConnectionStringResolver Resolver { get; }
}
```
Then in your DI container you can register the class as follows:

```csharp
public static class ServiceCollectionExtensions
{
    public static void AddMyDataAccessLayer()
    {
        // add in the options with the scope level you need
        services.AddSingleton<IDataAccessConfigOptions, DataAccessConfigOptions>();
        services.AddSingleton<IDataAccessKeyToConnectionStringResolver, new EnvironmentVariableConnectionStringResolver("EnvironmentKey")>();
        services.AddSingleton<IConnectionTokenResolver, AzureManagedIdentityTokenResolver>();
        
        services.AddSingleton<IDataAccessConfig>, DataAccessConfig)();
    }
}
```






