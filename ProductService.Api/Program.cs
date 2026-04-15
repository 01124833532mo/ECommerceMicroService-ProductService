using BussnissLogicLayer;
using DataAccessLayer;
using eCommerce.ProductsMicroService.API.APIEndpoints;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCustomExeptionHandllingMiddleware();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();




app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndpoints();
app.Run();
