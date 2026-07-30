using System.Data;
using CA.Blocks.DataAccess.DataTableHelpers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresSQLDataAccessTests.Base;

namespace CA.Blocks.PostgresSQLDataAccessTests.Samples.QuickStartExamples;

// We wrapp the QuickStartExamples to all the containter to start
[Collection("DbIntegrationTests")]
public class QuickStartExamplesContainter
{
    
      // Ths is your custom class
      // Here we note that the Naming is standard Csharp Naming convention
      public class MyCustomObject
      {
          public required string TableName {get; init;}
          public required string Owner {get; init;}
          public required bool HasIndexes {get; init;}
      }
      
  
    // Data Access Class
    [Collection("DbIntegrationTests")]
    public class QuickStartDataAccess : UnitTestDataAccess
    {

        public async Task<DataTable> GetPostgresTables()
        {
            var sqlCmd = CreateTextCommand(@"
SELECT * FROM pg_catalog.pg_tables");
            return await ExecuteAsync(sqlCmd).ToDataTable();
        }

        
        public async Task<IList<MyCustomObject>> GetMyCustomObjects()
        {
            // Step 1 Build the query
            var sqlCmd = CreateTextCommand(@"
SELECT tablename as TableName, tableowner as Owner, hasindexes as HasIndexes FROM pg_catalog.pg_tables")
                ;
            // Step 2 and 3 are to execute and translate
            return await ExecuteAsync(sqlCmd).ToListOf<MyCustomObject>();
            // Step 2 ^^^^^^^^^^^^^^^^^^^^^^^
            // here we are execute the cmdAsync
            // Step 3                         ^^^^^^^^^^^^^^^^^^^^^^^^^^
            // here we reading the tabular data stream and converting the results to MyCustomObject object adding to list as we go.
        }
        
    
      
    }

  // Usages
  [Collection("DbIntegrationTests")]
  public class QuickStartExampleCalls(ITestOutputHelper output)
  {

      [Fact]
      public async Task InvokeAndDumpGetPostgresTables()
      {
          var myDataAccessClass = new QuickStartDataAccess();
          var dt = await myDataAccessClass.GetPostgresTables();
          output.WriteLine(DataTableToTextHelper.OutPutAsAlignedText(dt));
      }
      
      
      [Fact]
      public async Task InvokeAndDumpGetMyCustomObjects()
      {
          //1 Create a instance this can come from dependency injection or create pending your architecture 
          var myDataAccessClass = new QuickStartDataAccess();
          var theSystemObjects = await myDataAccessClass.GetMyCustomObjects();
          output.WriteLine($"There are {theSystemObjects.Count} objects in the result top 10 are");
          foreach (var myCustomObject in theSystemObjects.Take(10))
          {
                output.WriteLine($"\t Table name '{myCustomObject.TableName}' is owned by {myCustomObject.Owner} and HasIndexes={myCustomObject.HasIndexes}");
          }
      }
  }
}
