using Ecommerce.Api.Configurations;
using Ecommerce.Api.Filters;
using Ecommerce.Application.UseCases.Auth;
using Ecommerce.Application.UseCases.Categories;
using Ecommerce.Application.UseCases.Users;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Data.Configurations;
using Ecommerce.Infrastructure.Extensions;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT"
    });
    c.OperationFilter<SwaggerAuthorizeOperationFilter>();
});


builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configurar JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Servicios
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IEmailService, GmailSmtpEmailSender>();


// Use Cases
builder.Services.AddScoped<SignInUseCase>();
builder.Services.AddScoped<SignUpUseCase>();
builder.Services.AddScoped<SignOutUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<ResetPasswordUseCase>();
builder.Services.AddScoped<ForgotPasswordUseCase>();
builder.Services.AddScoped<UserUseCases>();
builder.Services.AddScoped<CategoryUseCases>();


// Filters
builder.Services.AddScoped<PostAuthorizeFilter>();
builder.Services.AddScoped<PostAuthorizeRoleFilter>();

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddCustomInvalidModelStateResponse();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NewPolicy");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
