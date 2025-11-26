using Ecommerce.Api.Configurations;
using Ecommerce.Api.Filters;
using Ecommerce.Application.UseCases.Auth;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Data.Configurations;
using Ecommerce.Infrastructure.Extensions;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Servicios
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IEmailService, GmailSmtpEmailSender>();


// Use Cases
builder.Services.AddScoped<SignInUseCase>();
builder.Services.AddScoped<SignUpUseCase>();
builder.Services.AddScoped<SignOutUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<PostAuthorizeFilter>();
builder.Services.AddScoped<ResetPasswordUseCase>();
builder.Services.AddScoped<ForgotPasswordUseCase>();

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

// Middleware
//app.UseMiddleware<ValidationErrorHandlerMiddleware>();

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
