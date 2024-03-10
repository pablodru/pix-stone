using Microsoft.EntityFrameworkCore;
using Pix.Services;
using Pix.Repositories;
using Pix.Data;
using Pix.Middlewares;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Database

builder.Services.AddDbContext <AppDBContext> (options => {

    string host = builder.Configuration["Database:Host"] ?? string.Empty;
    string port = builder.Configuration["Database:Port"] ?? string.Empty;
    string username = builder.Configuration["Database:Username"] ?? string.Empty;
    string database = builder.Configuration["Database:Name"] ?? string.Empty;
    string password = builder.Configuration["Database:Password"] ?? string.Empty;

    string connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";

    options.UseNpgsql(connectionString);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services and Repositories
builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<KeysRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<BankRepository>();
builder.Services.AddScoped<KeyService>();
builder.Services.AddScoped<TokenValidationMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics(options => options.AddCustomLabel("host", context => context.Request.Host.Host));

app.UseHttpsRedirection();

app.MapControllers();

app.MapMetrics();

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
