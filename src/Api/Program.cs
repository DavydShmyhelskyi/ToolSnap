using Api.Modules;
using Api.Services.GemeniAiService;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.SetupServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<GeminiService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// DB init
await app.InitialiseDatabaseAsync();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
