
using CA.Blocks.DataAccess.Extensions;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.Samples.QuickStartExamples;


public class QuickStartExamplesContainter
{
    
      // Ths is your custom class
      // Here we note that the Naming is standard Csharp Naming convention
      public class MyCustomObject
      {
          public required int? RootPage {get; init;}
          public required string Name {get; init;}
          public required string ObjectType {get; init;}
      }
      
  
    // Data Access Class
    [Collection("DbIntegrationTests")]
    public class QuickStartDataAccess : UnitTestDataAccess
    {
        public async Task<IList<MyCustomObject>> GetMyCustomObjects(string type)
        {
            // Step 1 Build the query
            // 1 In this example we alias the sql names with the object name that we expect in code from the table
            // id -> Id
            // name -> Name
            // crdate -> CreateDate
            
            // 2 The types are defined int the db an flow through the the C# object
            // 3 The Object MyCustomObject is defined with required and init parameters
            // 4 We pass the value type to the ToSqlParameter @Type
            var sqlCmd = CreateTextCommand(@"
SELECT rootpage as RootPage, name as Name, type as ObjectType
FROM sqlite_master WHERE type = type").WithParameter(type.ToSqlParameter("@Type"));
            
            // Step 2 and 3 are to execute and translate
            return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();
            // Step 2 ^^^^^^^^^^^^^^^^^^^^^^^
            // here we are execute the cmdAsync
            // Step 3                         ^^^^^^^^^^^^^^^^^^^^^^^^^^
            // here we reading the tabular data stream and converting the results to MyCustomObject object adding to list as we go.
            
            /*  it you use profiler you see the follow query over the wire
             exec sp_executesql N'
Select id as Id, name as Name, crdate as CreateDate
from sys.sysobjects where type = @Type',N'@Type nvarchar(1)',@Type=N'S'
             */
        }
      
    }

  // Usages
  public class QuickStartExampleCalls(ITestOutputHelper output)
  {

      [Fact]
      public async Task InvokeAndDumpGetMyCustomObjects()
      {
          //1 Create a instance this can come from dependency injection or create pending your architecture 
          var myDataAccessClass = new QuickStartDataAccess();
          var theSystemObjects = await myDataAccessClass.GetMyCustomObjects("table");
          output.WriteLine($"There are {theSystemObjects.Count} objects in the result top 10 are");
          foreach (var myCustomObject in theSystemObjects.Take(10))
          {
                output.WriteLine($"\tobject RootPage {myCustomObject.RootPage} with name '{myCustomObject.Name}' is a {myCustomObject.ObjectType}");
          }
      }
  }
}
