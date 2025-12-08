using TomadaStore.SaleConsumerAPI.Data;
using TomadaStore.SaleConsumerAPI.Repositories;
using TomadaStore.SaleConsumerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddScoped<ConnectionDB>();

builder.Services.AddScoped<SaleRepository>();
builder.Services.AddScoped<SaleConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
