using CA.Blocks.MySqlAppHost;

var builder = DistributedApplication.CreateBuilder(args);

// name can only have Ascii hyphens and numbers  and db need to quote hyphens so go simple
var databaseName = ConfigSettings.DB_NAME;
var creationScript = $"CREATE DATABASE {databaseName};";

var server = builder.AddMySql(ConfigSettings.SERVER_SERVICE_NAME);
// Can use WithPhpMyAdmin when debugging
 // .WithPhpMyAdmin(); 

var db = server.AddDatabase(databaseName);
    
db.WithCreationScript(creationScript);

builder.Build().Run();