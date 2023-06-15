using System.Configuration;
using Application.DTOs.Area;
using Application.DTOs.HourlyPrices;
using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Quartz;
using WebApi.QuartzJobs.PriceGrabber;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        q.AddJob<PriceGrabberJob>(j => j.StoreDurably().WithIdentity("PriceGrabberJob", "JobGroup"));

        // Define your job trigger for 14:30 daily
        q.AddTrigger(t => t
            .ForJob("PriceGrabberJob", "JobGroup")
            .WithIdentity("TriggerPriceGrabber", "JobGroup")
            .WithCronSchedule("0 36 18 * * ?"));
    });

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create connection string
var connectionStringBuilder = new SqlConnectionStringBuilder();
connectionStringBuilder.DataSource = "localhost";
connectionStringBuilder.Encrypt = true;
connectionStringBuilder.TrustServerCertificate = true;
connectionStringBuilder.UserID = Environment.GetEnvironmentVariable("DB_LOGIN");
connectionStringBuilder.Password = Environment.GetEnvironmentVariable("DB_PASSWORD");
connectionStringBuilder.InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME");

/*connectionStringBuilder.UserID = "myeplogin";
connectionStringBuilder.Password = "myStrong(!)Password";
connectionStringBuilder.InitialCatalog = "TestDB";*/


string connectionString = connectionStringBuilder.ConnectionString;
Console.WriteLine(connectionString);

builder.Services.AddHttpClient();
builder.Services.AddDbContext<EnergyPricesDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IDailyPricesRepository, DailyPricesRepository>();
builder.Services.AddScoped<IDailyPricesService, DailyPricesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Area Endpoints
app.MapGet("EnergyPrices/v1/Areas", async (IAreaService areaService) =>
{
    var result = await areaService.GetAllAsync();

    if(result.IsSuccessful)
        return Results.Ok(result.Data);
    else
        return Results.NotFound(result.ErrorMessage);
});

app.MapGet("EnergyPrices/v1/Areas/{areaId:guid}", async (IAreaService areaService, Guid areaId) =>
{
    var result = await areaService.GetByIdAsync(areaId);

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

app.MapPost("EnergyPrices/v1/Areas", async (IAreaService areaService, AreaCreateDto areaCreateDto) =>
{
    var response = await areaService.AddAsync(areaCreateDto);

    return Results.Created($"/EnergyPrices/v1/Areas/{response.Data.Id}", response.Data);
});

app.MapPut("/EnergyPrices/v1/Areas/{areaId:guid}", async (IAreaService areaService, Guid areaId, AreaUpdateDto areaUpdateDto) =>
{
    var result = await areaService.UpdateAsync(areaId, areaUpdateDto);
    if(!result.IsSuccessful)
        return Results.NotFound(result.ErrorMessage);
    return Results.Ok(result.Data);
});

app.MapDelete("EnergyPrices/v1/Areas/{areaId:guid}", async (IAreaService areaService, Guid areaId) =>
    {
        var result = await areaService.DeleteAsync(areaId);
        if(!result.IsSuccessful == true)
            return Results.NotFound(result.ErrorMessage);
        return Results.Ok();
    });

// DailyPrices Endpoints
app.MapGet("EnergyPrices/v1/Areas/{areaId:guid}/today", async (IDailyPricesService dailyPricesService, Guid areaId) =>
{
    var result = await dailyPricesService.GetByAreaAndDate(areaId, DateTime.Today.Date);

    if(result.IsSuccessful)
        return Results.Ok(result.Data);
    else
        return  Results.NotFound(result.ErrorMessage);
});

app.MapGet("EnergyPrices/v1/Areas/{areaId:guid}/tomorrow", async (IDailyPricesService dailyPricesService, Guid areaId) =>
{
    var result = await dailyPricesService.GetByAreaAndDate(areaId, DateTime.Today.AddDays(1).Date);

    if(result.IsSuccessful)
        return Results.Ok(result.Data);
    else
        return  Results.NotFound(result.ErrorMessage);
});

app.MapPost("EnergyPrices/v1/Areas/{areaId:guid}", async (IDailyPricesService dailyPricesService, List<HourlyPriceCreateDto> prices, Guid areaId) =>
{
    if(prices.Count() != 24)
        return Results.BadRequest("invalid request");
    var result = await dailyPricesService.AddAsync(areaId, DateTime.Today.AddDays(1).Date, prices);


    if(result is { IsSuccessful: true, Data: not null })
        return Results.Created($"/EnergyPrices/v1/Areas/{result.Data.AreaId}/{result.Data.Id}", result.Data);
    return Results.Problem(result.ErrorMessage);
});


app.MapControllers();

app.Run();


