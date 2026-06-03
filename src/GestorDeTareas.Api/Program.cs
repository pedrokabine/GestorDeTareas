using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Application.Services;
using GestorDeTareas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddSingleton<ITareaRepository, TareaRepositoryEnMemoria>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
