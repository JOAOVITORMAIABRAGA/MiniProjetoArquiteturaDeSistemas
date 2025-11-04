using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Data.Services
{
    public class DatabaseCleanerHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public DatabaseCleanerHostedService(IServiceProvider provider, IConfiguration config, IHostEnvironment env)
        {
            _provider = provider;
            _config = config;
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Segurança: só executa se explicitamente configurado e (por padrão) apenas em Development
            var clearOnStop = _config.GetValue<bool>("Database:ClearOnStop", false);
            var allowInNonDev = _config.GetValue<bool>("Database:AllowClearInNonDevelopment", false);

            if (!clearOnStop)
            {
                return;
            }

            if (!_env.IsDevelopment() && !allowInNonDev)
            {
                Console.WriteLine("⚠️  Ignorado: não está em ambiente Development e não autorizado pelas configurações.");
                return;
            }

            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                Console.WriteLine("🔁 Limpando banco de dados (EnsureDeletedAsync)...");
                await context.Database.EnsureDeletedAsync(cancellationToken);
                Console.WriteLine("✅ Banco de dados limpo com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⛔ Erro ao limpar o banco: {ex.Message}");
                // opcional: registrar exceção em log estruturado se houver logger no projeto
            }
        }
    }
}