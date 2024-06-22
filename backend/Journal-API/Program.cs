var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/journal", (WeatherForecast forecast) =>
{
    return forecast;
});

app.MapPut("/journal", (WeatherForecast forecast) =>
{
    return forecast;
});


