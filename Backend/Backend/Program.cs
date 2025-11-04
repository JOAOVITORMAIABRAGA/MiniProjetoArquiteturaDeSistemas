using backend.Data;
using backend.DataSources;
using Backend.Models.BibliotecaServices;
using Backend.Models.DiscenteServices;
using Backend.Models.DisciplinaAlunoServices;
using Backend.Models.DisciplinaServices;
using Backend.Models.LivroReservaService;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();


builder.Services.AddHostedService<Backend.Data.Services.DatabaseCleanerHostedService>();

var app = builder.Build();


// ================================
// APLICAR MIGRATIONS (antes do Seed)
// ================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Configurações para controlar comportamento em diferentes ambientes
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

    // ================================
    // SEED DOS DADOS (executa 1 vez no início)
    // ================================
    var discenteService = services.GetRequiredService<DiscenteService>();
    var disciplinaService = services.GetRequiredService<DisciplinaService>();
    var bibliotecaService = services.GetRequiredService<BibliotecaService>();

    await DataSeeder.SeedAsync(context, discenteService, disciplinaService, bibliotecaService);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();