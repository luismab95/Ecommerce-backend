using Ecommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Capa de Infraestructura
builder.Services.AddInfrastructure(builder.Configuration, builder.Host, builder.Environment);


// Servicios necesarios para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Genera el JSON OpenAPI
    app.UseSwaggerUI(); // Genera la UI Swagger
}

app.UseHttpsRedirection();


app.MapGet("/hello", () => "Hola!");

app.Run();

