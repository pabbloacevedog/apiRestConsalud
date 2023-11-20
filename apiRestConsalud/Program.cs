using apiConsalud.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Cargar la configuración desde el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Cargar la configuración desde el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Obtener la clave de API desde la configuración
var apiKey = builder.Configuration["ApiKey"];

builder.Services.AddAuthentication("ApiKeyScheme")
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", options =>
    {
        options.ApiKey = apiKey; // Pasa la clave de API al handler
    });
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<FacturaService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

