using DEPLOY.CQRSPattern.Api;
using DEPLOY.CQRSPattern.Application.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var commandHandlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
                .ToList();

foreach (var handlerType in commandHandlerTypes)
{
    var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
    builder.Services.AddTransient(interfaceType, handlerType);
}

var queryHandlerTypes = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
    .ToList();

foreach (var handlerType in queryHandlerTypes)
{
    var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
    builder.Services.AddTransient(interfaceType, handlerType);
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/command", async ([FromBody] SampleCommand command, [FromServices] ICommandHandler<SampleCommand> commandHandler) =>
{
    await commandHandler.Handle(command);
    return Results.Ok();
});

app.MapGet("/query", async ([FromQuery] SampleQuery query, [FromServices] IQueryHandler<SampleQuery, SampleQueryResult> queryHandler) =>
{
    var result = await queryHandler.Handle(query);
    return Results.Ok(result);
});

app.Run();