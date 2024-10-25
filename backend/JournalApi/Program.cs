using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Services.AddTransient<IDbConnection>(x => new NpgsqlConnection(
    builder.Configuration.GetConnectionString("DatabaseConnection")
));

var jwtConfig = builder.Configuration.GetSection("JwtConfig");

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig["SecretKey"] ?? string.Empty)
            ),
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    response = new { isAuthorized = false },
                    messageText = "You are not authenticated to access " + "this resource.",
                };

                var result = JsonSerializer.Serialize(response);

                return context.Response.WriteAsync(result);
            },

            OnChallenge = context =>
            {
                context.HandleResponse(); // Skip the default response
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

// Add controllers to the services

builder.Services.AddControllers();

// Add CORS services and allow all origins, methods, and headers

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseRouting();

// Use CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
