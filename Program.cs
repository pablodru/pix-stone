using Microsoft.EntityFrameworkCore;
using Pix.Services;
using Pix.Repositories;
using Pix.Data;
using Pix.Middlewares;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();

app.Run();
