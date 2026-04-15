using BussnissLogicLayer;
using DataAccessLayer;
using ProductService.Api;
using ProductService.Api.MiddleWare;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayerRegistration(builder.Configuration);
builder.Services.AddBusinessLogicLayerRegistration();
builder.Services.AddApiRegistration();


builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseRouting();



app.UseCustomExeptionHandllingMiddleware();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
