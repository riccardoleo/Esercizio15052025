using Esercizio15052025.Models;
using Esercizio15052025.profile;
using Esercizio15052025.Repository.PlantComponent_Repo;
using Esercizio15052025.Repository.PlantComponent_Repo.Interfaces;
using Esercizio15052025.Repository.Tool_Repo;
using Esercizio15052025.Repository.Tool_Repo.Interfaces;
using Esercizio15052025.Repository.ToolCategory_Repo;
using Esercizio15052025.Repository.ToolCategory_Repo.Interfaces;
using Esercizio15052025.Service.PlantComponent_Service;
using Esercizio15052025.Service.PlantComponent_Service.Interfaces;
using Esercizio15052025.Service.Tool_Service;
using Esercizio15052025.Service.Tool_Service.Interfeces;
using Esercizio15052025.Service.ToolsCategory_Service;
using Esercizio15052025.Service.ToolsCategory_Service.Interfaces;
using Esercizio20052025.Controllers;
using Esercizio20052025.Repository.LPermission_Repo;
using Esercizio20052025.Repository.LPermission_Repo.Interfaces;
using Esercizio20052025.Repository.LVisibility_Repo;
using Esercizio20052025.Repository.LVisibility_Repo.Interfaces;
using Esercizio20052025.Repository.User_Repo;
using Esercizio20052025.Repository.User_Repo.Interfaces;
using Esercizio20052025.Service.LPermission_Service;
using Esercizio20052025.Service.LPermission_Service.Interfaces;
using Esercizio20052025.Service.LVisibility_Service;
using Esercizio20052025.Service.LVisibility_Service.Interfaces;
using Esercizio20052025.Service.User_Service;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("Avvio dell'applicazione");

    // JWT e autenticazione
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7121",
            ValidAudience = "https://localhost:7121",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pippo1234pippo1234pippo1234pippo1234")),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

    // CORS per Angular
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()  // ⚠️ PERICOLOSO - accetta da qualsiasi dominio
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // Nota: non puoi usare AllowCredentials() con AllowAnyOrigin()
        });
    });

    // Non dimenticare di usare la policy nel middleware


    // DbContext
    builder.Services.AddDbContext<EQUIPPINGContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Repository e servizi
    builder.Services.AddScoped<IPlantComponent_Repo, PlantComponent_Repo>();
    builder.Services.AddScoped<ITool_Repo, Tool_Repo>();
    builder.Services.AddScoped<IToolCategory_Repo, ToolCategory_Repo>();
    builder.Services.AddScoped<IUser_Repo, User_Repo>();

    builder.Services.AddScoped<IPlantComponentService, PlantComponentService>();
    builder.Services.AddScoped<ITool_Service, Tool_Service>();
    builder.Services.AddScoped<IToolCategory_Service, ToolCategory_Service>();
    builder.Services.AddScoped<IUser_Service, User_Service>();
    builder.Services.AddScoped<ILPermission_Repo, LPermission_Repo>();
    // Repository
    builder.Services.AddScoped<ILVisibility_Repo, LVisibility_Repo>();

    // Service
    builder.Services.AddScoped<ILVisibility_Service, LVisibility_Service>();
    builder.Services.AddScoped<ILPermission_Service, LPermission_Service>();
    builder.Services.AddScoped<ILPermission_Repo, LPermission_Repo>();


    // Controller, Swagger e AutoMapper
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthentication();  // <— Prima l’autenticazione
    app.UseAuthorization();   // <— Poi l’autorizzazione

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Errore fatale all'avvio dell'app");
    throw;
}
finally
{
    LogManager.Shutdown();
}
