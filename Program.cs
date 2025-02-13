using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;
using System.Security.Claims;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Services.Service;
using Unilever.CDExcellent.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services

// Register DbContext with the connection string from configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services for authentication and authorization
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register services for Area and Distributor management
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IDistributorService, DistributorService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Register VisitPlan, Notification, and UserTask services
builder.Services.AddScoped<IVisitPlanService, VisitPlanService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();

// ? Register CMS Services (Article, Category, Comment)
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


// Register IHttpContextAccessor for accessing the current HTTP context
builder.Services.AddHttpContextAccessor();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero  // Reduces token expiration delay
        };

        // Event hooks for better error handling
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("JWT Challenge: " + context.ErrorDescription);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT Token validated successfully.");
                return Task.CompletedTask;
            }
        };
    });

// Configure Authorization policies
builder.Services.AddAuthorization(options =>
{
    // Ensure that only users with 'Admin' role can access Admin resources
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

// Add services for controllers and Swagger UI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Unilever CDExcellent API",
        Version = "v1",
        Description = "API for managing users, distributors, areas, and CMS",
    });
});

// Add CORS policy if required
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Build the application
var app = builder.Build();

// Configure middleware pipeline

// Enable Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unilever CDExcellent API v1");
    });
}

// Enable CORS
app.UseCors("AllowAllOrigins");

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
