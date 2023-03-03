using Microsoft.EntityFrameworkCore;
using QueenFisher.Api.Extensions;
using QueenFisher.Data.Context;
using QueenFisher.Data.Seeding;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

// Add services to the container.
builder.Services.AddHttpClient();

//For Entity Framework

builder.Services.AddDbContext<QueenFisherDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("ConnStr")));

//builder.Services.AddControllers();
// Configure Mailing Service
builder.Services.ConfigureMailService(config);

builder.Services.AddSingleton(Log.Logger);


// Configure Identity
builder.Services.ConfigureIdentity();

builder.Services.AddAuthentication();

// Add Jwt Authentication and Authorization
services.ConfigureAuthentication(config);


//Add cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:44351", "http://localhost:4200", "http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// Configure AutoMapper
services.ConfigureAutoMappers();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

// Register Dependency Injection Service Extension
builder.Services.AddDependencyInjection(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Seeder.SeedData(app).Wait();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
