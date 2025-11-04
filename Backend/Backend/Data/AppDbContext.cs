using Microsoft.EntityFrameworkCore;
using backend.Models;
using Backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Discente> Discentes { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<DisciplinaAluno> DisciplinaAluno { get; set; }
        public DbSet<LivroReserva> LivroReserva { get; set; }

    }
}
