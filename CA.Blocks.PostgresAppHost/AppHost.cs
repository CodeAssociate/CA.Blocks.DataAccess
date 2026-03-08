var builder = DistributedApplication.CreateBuilder(args);

var databaseName = "ca-blocks-db";
var creationScript = $"CREATE DATABASE \"{databaseName}\" WITH OWNER = postgres;";

var server = builder.AddPostgres("postgres");
// Can use PgAdmin when debugging
//  .WithPgAdmin(); 
 
          
var db = server.AddDatabase(databaseName);
    
db.WithCreationScript(creationScript);


builder.Build().Run();
