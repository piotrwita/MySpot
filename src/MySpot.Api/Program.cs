using Microsoft.Extensions.Options;
using MySpot.Api;
using MySpot.Application;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.UseSerilog();

var app = builder.Build();

app.UseInfrastucture();

//MinimalAPI
app.UseUsersApi();

app.Run();





