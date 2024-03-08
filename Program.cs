using Microsoft.EntityFrameworkCore;
using Pix.Services;
using Pix.Repositories;
using Pix.Data;
using Pix.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Database

builder.Services.AddDbContext<AppDBContext>();

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
