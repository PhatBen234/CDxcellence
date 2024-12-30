using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Services;
using Unilever.CDExcellent.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with the connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IAuthService and AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Register IUserService and UserService
builder.Services.AddScoped<IUserService, UserService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add services for controllers, Swagger, and other necessary configurations
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Authentication and Authorization
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
