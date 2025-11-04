using backend.Data;
using backend.Models;
using Backend.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.DataSources
{
    public class LivroDbDataSource : IDataSource<Livro>
    {
        private readonly AppDbContext _context;

        public LivroDbDataSource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Livro>> GetAllAsync()
        {
            return await _context.Livros.ToListAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            return await _context.Livros.FindAsync(id);
        }

        // Função extra específica do DB para salvar múltiplos registros (paralelo ao Discente)
        public async Task SaveRangeAsync(List<Livro> livros)
        {
            _context.Livros.AddRange(livros);
            await _context.SaveChangesAsync();
        }
    }
}