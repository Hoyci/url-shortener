using CreateURL.Repositories.Database;
using CreateURL.Services;
using IDGenerator.Repositories.Redis;
using IDGenerator.Services.IDGenerator;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddSingleton<IRepositoryRedis, Redis>();
builder.Services.AddSingleton<IDPermutator>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var key = configuration.GetValue<uint>(
        "IDGenerator:PermutationKey");

    return new IDPermutator(key);
});
builder.Services.AddScoped<IIDGeneratorService, IDGeneratorService>();

builder.Services.AddScoped<IDatabaseRepository>(_ =>
    new DatabaseRepository(builder.Configuration.GetConnectionString("Postgres")!));
builder.Services.AddScoped<IDatabaseInitializer>(_ =>
    new DatabaseInitializer(builder.Configuration.GetConnectionString("Postgres")!));
builder.Services.AddScoped<ICreateURLService, CreateURLService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
}

app.MapControllers();

app.Run();