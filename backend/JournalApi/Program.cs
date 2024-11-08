using System.Data;
using System.Text;
using System.Text.Json;
using JournalApi.Models;
using JournalApi.Repositories;
using JournalApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add Database
builder.Services.AddTransient<IDbConnection>(
    x => new NpgsqlConnection(
        builder.Configuration.GetConnectionString("DatabaseConnection")));

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IPasswordStrengthValidatorService,
                           PasswordStrengthValidatorService>();

// Setup JWT Authentication Configuration
var jwtConfig = builder.Configuration.GetSection("JwtConfig");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(jwtConfig["SecretKey"] ?? string.Empty)),
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed =
              context =>
              {
                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  context.Response.ContentType = "application/json";

                  var response = new
                  {
                      response = new { isAuthorized = false },
                      messageText =
                      "You are not authenticated to access " + "this resource.",
                  };

                  var result = JsonSerializer.Serialize(response);

                  return context.Response.WriteAsync(result);
              },

            OnChallenge =
              context =>
              {
                  context.HandleResponse();
                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  context.Response.ContentType = "application/json";

                  var response = new
                  {
                      response = new { isAuthorized = false },
                      messageText = "You are not authorized to access this resource.",
                  };

                  var result = JsonSerializer.Serialize(response);
                  return context.Response.WriteAsync(result);
              },
        };
    });

// Add services to the container.
builder.Services.AddControllers();

// Add CORS services and allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure pipeline
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
