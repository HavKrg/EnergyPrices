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

    return Results.Created($"/EnergyPrices/v1/Areas/{response.Id}", response);
});

app.MapGet("EnergyPrices/v1/Areas", async (IAreaService areaService) =>
{
    var response = await areaService.GetAllAsync();

    return Results.Ok(response);
});

app.MapGet("EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id) =>
{
    var response = await areaService.GetByIdAsync(id);

    if (response == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(response);
});

app.MapGet("EnergyPrices/v1/Areas/name/{name}", async (IAreaService areaService, string name) =>
{
    var area = await areaService.GetByNameAsync(name);

    if (area == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(area);
});

app.MapPut("/EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id, AreaUpdateDto areaUpdateDto) =>
{
    //bool isUpdated = await areaService.UpdateAsync(id, areaUpdateDto);

    //if (!isUpdated)
    //{
    //  return Results.NotFound();
    //}
    await areaService.UpdateAsync(id, areaUpdateDto);
    return Results.NoContent();
});

app.MapDelete("EnergyPrices/v1/Areas/{id:guid}", async (IAreaService areaService, Guid id) =>
    {
        var result = await areaService.DeleteAsync(id);
        if(result == true)
            return Results.Ok();
        else
            return Results.NotFound();
    });

app.MapControllers();

app.Run();
