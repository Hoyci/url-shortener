using System.Reflection;
using BuildingBlocks.Extensions;
using BuildingBlocks.Migrations;
using CreateURL.Migrations;
using CreateURL.Repositories.Database;
using CreateURL.Services;
using IDGenerator.Repositories.Redis;
using IDGenerator.Services.IDGenerator;
using Microsoft.OpenApi;
using StackExchange.Redis;
using RedirectDatabaseRepository = RedirectURL.Repositories.Database.DatabaseRepository;
using RedirectIDatabaseRepository = RedirectURL.Repositories.Database.IDatabaseRepository;
using RedirectIRedisRepository = RedirectURL.Repositories.Redis.IRedisRepository;
using RedirectRedisRepository = RedirectURL.Repositories.Redis.RedisRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "API para criação e redirecionamento de URLs encurtadas."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpClient();

builder.Services.AddPostgres(builder.Configuration);

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddSingleton<IRepositoryRedis, Redis>();
builder.Services.AddSingleton<IDPermutator>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var key = configuration.GetValue<uint>("IDGenerator:PermutationKey");

    return new IDPermutator(key);
});
builder.Services.AddScoped<IIDGeneratorService, IDGeneratorService>();

builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddScoped<IDatabaseMigration, UrlsTableMigration>();
builder.Services.AddScoped<ICreateURLService, CreateURLService>();

builder.Services.AddScoped<RedirectIRedisRepository, RedirectRedisRepository>();
builder.Services.AddScoped<RedirectIDatabaseRepository, RedirectDatabaseRepository>();
builder.Services.AddScoped<RedirectURL.Services.IRedirectURLService, RedirectURL.Services.RedirectURLService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
    await migrator.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1");
    options.DocumentTitle = "URL Shortener API";
});

app.MapControllers();

app.Run();
