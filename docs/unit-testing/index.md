# Unit Testing & Integration Testing Guide

CA.Blocks.DataAccess was built from the ground up to be fully testable Unlike traditional ORMs that require complex in-memory setups or mock DbSets, our minimal abstraction layer uses clean interfaces that allow for effortless mocking of your data access code
Additionally, for true end-to-end validation, we fully support modern, containerized integration testing.

## Unit Testing Business Logic (Mocking)
   
When unit testing the business logic or services that depend on your data access layer, the best practice is to extract an interface for your custom Data Access class. Because CA.Blocks.DataAccess handles the underlying connection and command execution pipelines, you only need to mock the inputs and outputs of your specific methods.

### Example Setup:
First, define an interface for your specific data access class:
````csharp
public interface IMyDataAccess
{
    Task<IList<MyModel>> GetAllAsync();
}
````
You then work with e instance class
````csharp
public class MyDataAccess : {youdbClassINstanc} , IMyDataAccess
{
       // The Code
       public async Task<IList<MyModel>> GetAllAsync()
       {
           var cmd = CreateCommand("SELECT * FROM MyTable");
           return await ExecuteAsync(cmd).ToListOf<MyModel>();
       }
}

````

# Writing the Test (using xUnit and Moq):

In your unit tests, you can easily mock IMyDataAccess to bypass the database entirely and ensure your business logic behaves correctly:

````csharp
[Fact]
public async Task UserService_Should_Return_Mapped_Models()
{
    // Arrange
   var mockDataAccess = new Mock<IMyDataAccess>();
   mockDataAccess.Setup(db => db.GetAllAsync())
   .ReturnsAsync(new List<MyModel> { new MyModel { Id = 1, Name = "Test" } });

    var service = new MyBusinessService(mockDataAccess.Object);

    // Act
    var result = await service.ProcessDataAsync();

    // Assert
    Assert.NotNull(result);
    mockDataAccess.Verify(db => db.GetAllAsync(), Times.Once);
}

````
## Integration Testing with xUnit & .NET Aspire

While unit tests are great for business logic, verifying your complex SQL queries and custom row translators requires integration testing against a real database. As part of the 2026 CA.Blocks.DataAccess roadmap, we advocate for migrating integration testing matrices to xUnit + .NET Aspire for modern, containerized cloud-native orchestration

By utilizing .AppHost projects, you can dynamically spin up containerized instances of SQL Server, PostgreSQL, MySQL, or SQLite to run your queries exactly as they would execute in production
 
View our working examples: The CA.Blocks.DataAccess source code contains comprehensive examples of this architecture in the [CA.Blocks.SQLServer.AppHost](https://github.com/CodeAssociate/CA.Blocks.DataAccess/tree/master/CA.Blocks.SQLServer.AppHost), [CA.Blocks.PostgresAppHost](https://github.com/CodeAssociate/CA.Blocks.DataAccess/tree/master/CA.Blocks.PostgresAppHost), and [CA.Blocks.MySqlAppHost](https://github.com/CodeAssociate/CA.Blocks.DataAccess/tree/master/CA.Blocks.MySqlAppHost) projects

Zero Boilerplate: By resolving connection strings securely via Aspire components, you can inject test databases directly into your DataAccessConfig without managing local database scripts
   