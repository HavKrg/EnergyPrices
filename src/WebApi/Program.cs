using Application.DTOs.Area;
using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<EnergyPricesDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAreaService, AreaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("EnergyPrices/v1/Areas", async (IAreaService areaService, AreaCreateDto areaCreateDto) =>
{
// Convert AreaCreateDTO to Area domain object
    var response = await areaService.AddAsync(areaCreateDto);

// Save the area to your data store or perform any other required actions
    // ...

    return Results.Created($"/EnergyPrices/v1/Areas/{response.Data.Id}", response.Data);
});

app.MapGet("EnergyPrices/v1/Areas", async (IAreaService areaService) =>
{
    var result = await areaService.GetAllAsync();

    return Results.Ok(result.Data);
});

app.MapGet("EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id) =>
{
    var result = await areaService.GetByIdAsync(id);

    if (!result.IsSuccessful)
    {
        return Results.NotFound(result.ErrorMessage);
    }

    return Results.Ok(result.Data);
});

app.MapGet("EnergyPrices/v1/Areas/name/{name}", async (IAreaService areaService, string name) =>
{
    var result = await areaService.GetByNameAsync(name);

    if (!result.IsSuccessful)
    {
        return Results.NotFound(result.ErrorMessage);
    }

    return Results.Ok(result.Data);
});

app.MapPut("/EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id, AreaUpdateDto areaUpdateDto) =>
{
    var result = await areaService.UpdateAsync(id, areaUpdateDto);
    if(!result.IsSuccessful)
        return Results.NotFound(result.ErrorMessage);
    return Results.Ok(result.Data);
});

app.MapDelete("EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id) =>
    {
        var result = await areaService.DeleteAsync(id);
        if(!result.IsSuccessful == true)
            return Results.NotFound(result.ErrorMessage);
        return Results.Ok();
    });

app.MapControllers();

app.Run();
