using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using WebApplication.Application;
using WebApplication.ApplicationData;
using WebApplication.SqlDataAccess;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
IHostEnvironment env = builder.Environment;
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
var configuration = (IConfiguration)builder.Configuration;


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbServices(configuration);
builder.Services.AddApplicationData();
builder.Services.AddApplication();



builder.Services.AddAuthentication(
    opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5000",
            ValidAudience = "http://localhost:5000",
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerDocument(
    config =>
    {
        config.PostProcess = document =>
        {
            document.Info.Version = "v1";
            document.Info.Title = "Web Application API";
            document.Info.Description = "ASP.NET Web API for Web Application.";

            document.Info.Contact = new OpenApiContact
            {
                Name = "Leonard Surac",
                Email = "leonardsurac@web.de"
            };
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
    }); ;
}

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();