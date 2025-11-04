using backend.Data;
using backend.DataSources;
using Backend.Models.BibliotecaServices;
using Backend.Models.DiscenteServices;
using Backend.Models.DisciplinaAlunoServices;
using Backend.Models.DisciplinaServices;
using Backend.Models.LivroReservaService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ================================
// DATABASE
// ================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// DATASOURCES
// ================================

// AWS (Fonte externa)
builder.Services.AddHttpClient<DiscenteAwsDataSource>();
builder.Services.AddHttpClient<DisciplinaAwsDataSource>();
builder.Services.AddHttpClient<LivroAwsDataSource>();

// Banco local (repository)
builder.Services.AddScoped<DiscenteDbDataSource>();
builder.Services.AddScoped<DisciplinaDbDataSource>();
builder.Services.AddScoped<LivroDbDataSource>();

// ================================
// SERVICES (Regras de negócio)
// ================================
builder.Services.AddScoped<DiscenteService>();
builder.Services.AddScoped<DisciplinaService>();
builder.Services.AddScoped<BibliotecaService>();
builder.Services.AddScoped<MatriculaService>();
builder.Services.AddScoped<LivroReservaService>();

// ================================
// API + SWAGGER
// ================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeuProjeto API", Version = "v1", Description = "API do projeto de exemplo" });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// ================================
// CORS (OBRIGATÓRIO para acesso pelo frontend React)
// ================================
var frontendOrigin = builder.Configuration.GetValue<string>("Frontend:Url");
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (!string.IsNullOrWhiteSpace(frontendOrigin))
        {
            policy.WithOrigins(frontendOrigin)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            // Em dev, permite tudo (mantenha cuidado)
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

builder.Services.AddHostedService<Backend.Data.Services.DatabaseCleanerHostedService>();

var app = builder.Build();

// ================================
// APLICAR MIGRATIONS (antes do Seed)
// ================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    var applyMigrations = app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup", true);
    var allowInNonDev = app.Configuration.GetValue<bool>("Database:AllowMigrateInNonDevelopment", false);

    if (applyMigrations)
    {
        if (!app.Environment.IsDevelopment() && !allowInNonDev)
        {
            Console.WriteLine("⚠️ Migrations puladas: não está em Development e AllowMigrateInNonDevelopment está false.");
        }
        else
        {
            try
            {
                Console.WriteLine("🔄 Aplicando migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migrations aplicadas.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⛔ Falha ao aplicar migrations: {ex.Message}");
                throw;
            }
        }
    }

    var discenteService = services.GetRequiredService<DiscenteService>();
    var disciplinaService = services.GetRequiredService<DisciplinaService>();
    var bibliotecaService = services.GetRequiredService<BibliotecaService>();

    await DataSeeder.SeedAsync(context, discenteService, disciplinaService, bibliotecaService);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeuProjeto API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("FrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();