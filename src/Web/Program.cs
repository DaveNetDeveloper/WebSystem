using Microsoft.AspNetCore.Authentication.JwtBearer;

//
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

var app = builder.Build();

//app.MapGet("/", () => ("/index.html"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory())),
    RequestPath = ""
});

app.UseRouting();

//app.UseCors(builder =>
//{
//    builder.WithOrigins("https://localhost:7161") // Reemplaza con el dominio de tu front-end
//           .AllowAnyHeader()
//           .AllowAnyMethod();
//}); 

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/", async context =>
//    {
//        await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "index.html"));
//    });

//    // Otros endpoints y configuraciones de enrutamiento si es necesario
//});

app.Run();
