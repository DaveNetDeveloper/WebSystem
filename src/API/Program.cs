using API.Middlewares;
using Domain.Entities;
using Application.Common; 
using Application.DependencyInjection;
using Infrastructure.DependencyInjection; 

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic; 
using System.Text;
using System.Threading.RateLimiting;
using FluentMigrator.Runner;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("UsuariosLimiter", opt =>
    {
        opt.PermitLimit = 5; // máximo x requests
        opt.Window = TimeSpan.FromSeconds(30); // cada x segundos
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddFile(builder.Configuration.GetSection("Paths")["LogFilePath"]);
 
builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); 

builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("AppConfiguration"));

builder.Configuration.AddJsonFile("Resources/messages.json", optional: false, reloadOnChange: true);

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT así: Bearer {tu token}"
    });

    // Requisito de seguridad global
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
}); 
 
// Register Services & Filters 
builder.Services.AddApplication();
 
// Register Repositories & Filters 
builder.Services.AddInfrastructure(builder.Environment.EnvironmentName);

//var key = Encoding.ASCII.GetBytes("mi__secreto_secreto");
var key = builder.Configuration["Jwt:Key"]; // guarda en appsettings.json
var issuer = builder.Configuration["Jwt:Issuer"];

//if (System.Diagnostics.Debugger.IsAttached == false)  System.Diagnostics.Debugger.Launch(); 

if (!builder.Environment.IsEnvironment(Application.Common.Environments.Test))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero // para que expire justo al llegar el tiempo
        };
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents {
            OnMessageReceived = ctx => {
                Console.Out.WriteLine($"[Auth Debug] Token recibido: {ctx.Token}");
                return Task.CompletedTask;
            }
        };
    });
    //builder.Services.AddAuthorization();
} 

if (builder.Environment.EnvironmentName == Application.Common.Environments.Test) {

    builder.Services.RemoveAll<IAuthenticationSchemeProvider>();
     
    builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = "Test";
        options.DefaultChallengeScheme = "Test";
    })
    .AddScheme<AuthenticationSchemeOptions, FakeJwtBearerHandler>("Test", _ => { });
}

//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole(Roles.Admin));
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .Build();

        });
});

//var sp = builder.Services.BuildServiceProvider();
//var authOptions = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions>>().Value;
//Console.Out.WriteLine("DefaultAuthenticateScheme: " + authOptions.DefaultAuthenticateScheme);
//Console.Out.WriteLine("DefaultChallengeScheme: " + authOptions.DefaultChallengeScheme);
 
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (!builder.Environment.IsEnvironment(Application.Common.Environments.Test) && 
    !builder.Environment.IsEnvironment(Application.Common.Environments.Production)) 
{
    using var scope = app.Services.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

// Health endpoint en JSON
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions  {
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            duration = report.TotalDuration
        });
        await context.Response.WriteAsync(result);
    }
}); 

app.UseCors(builder => {
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});

if (app.Environment.IsEnvironment(Application.Common.Environments.Development))
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRateLimiter(); 
app.MapControllers();
app.MapControllers().RequireRateLimiting("UsuariosLimiter");
app.Run();

public partial class Program { }