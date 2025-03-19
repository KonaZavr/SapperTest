using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SapperTest.Contracts;
using SapperTest.Models;
using SapperTest.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GameInfoDatabaseSettings>(builder.Configuration.GetSection("GameInfoDatabase"));

// Add services to the container.
var cors = new List<string>
{
    "https://minesweeper-test.studiotg.ru"
};

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
                      {
                          policy.WithOrigins(cors.ToArray())
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(scope =>
{
    var databaseSettings = scope.GetRequiredService<IOptions<GameInfoDatabaseSettings>>();
    return new MongoClient(databaseSettings.Value.ConnectionString);
});

builder.Services.AddSingleton(scope =>
{
    var client = scope.GetRequiredService<IMongoClient>();
    var databaseSettings = scope.GetRequiredService<IOptions<GameInfoDatabaseSettings>>();
    return client.GetDatabase(databaseSettings.Value.DatabaseName);
});

builder.Services.AddSingleton<IMinesweeperService, MinesweeperService>();
builder.Services.AddSingleton<IGameInfoService, GameInfoService>();

builder.Services.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
