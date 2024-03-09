using GameStore.Api;
using GameStore.Api.Data;
using GameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("GameStore") ?? "";
builder.Services.AddSqlite<GameStoreContext> (connString);
var app = builder.Build();
app.MapGamesEndPoints();
app.MapGenresEndpoints();
await app.MigrateDb();
app.Run();
