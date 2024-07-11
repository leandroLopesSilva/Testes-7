using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using Questao5.Application.Middlewares;
using Questao5.Application.Validators;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<MovementValidator>());

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Register SQLite connection
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Register Dapper context and IDbConnection
builder.Services.AddTransient<IDapperContext, DapperContext>();
builder.Services.AddTransient<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<DatabaseConfig>();
    return new SqliteConnection(config.Name);
});
builder.Services.AddTransient<IIdempotencyRepository, IdempotencyRepository>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank API v1");
    });
}

// Use custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initialize database
var databaseBootstrap = app.Services.GetService<IDatabaseBootstrap>();
databaseBootstrap?.Setup();

app.Run();
