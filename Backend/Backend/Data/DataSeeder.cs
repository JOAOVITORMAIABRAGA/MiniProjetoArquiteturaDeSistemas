using backend.Models;
using Backend.Models.BibliotecaServices;
using Backend.Models.DiscenteServices;
using Backend.Models.DisciplinaServices;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            DiscenteService discenteService,
            DisciplinaService disciplinaService,
            BibliotecaService bibliotecaService)
        {

            // =======================
            // DISCENTES (AWS → DB)
            // =======================
            if (!context.Discentes.Any())
            {
                Console.WriteLine("🔄 Importando Discentes da AWS...");
                await discenteService.ImportarDiscentesDaAws();
                Console.WriteLine("✅ Discentes importados!");
            }

            // =======================
            // DISCIPLINAS (AWS → DB)
            // =======================
            if (!context.Disciplinas.Any())
            {
                Console.WriteLine("🔄 Importando Disciplinas da AWS...");
                await disciplinaService.ImportarDisciplinasDaAws();
                Console.WriteLine("✅ Disciplinas importadas!");
            }

            // =======================
            // LIVROS (AWS → DB)
            // =======================
            if (!context.Livros.Any())
            {
                Console.WriteLine("🔄 Importando Livros da AWS...");
                await bibliotecaService.ImportarLivrosDaAws();
                Console.WriteLine("✅ Livros importados!");
            }
        }
    }
}
