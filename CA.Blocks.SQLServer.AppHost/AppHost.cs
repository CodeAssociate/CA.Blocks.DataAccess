using CA.Blocks.SQLServer.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var databaseName = ConfigSettings.DB_NAME;
var creationScript = $"CREATE DATABASE [{databaseName}];";

var server = builder.AddSqlServer(ConfigSettings.SERVER_SERVICE_NAME);

          
var db = server.AddDatabase(databaseName);
    
db.WithCreationScript(creationScript);


builder.Build().Run();
