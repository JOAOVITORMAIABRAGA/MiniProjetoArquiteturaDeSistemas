using backend.Data;
using backend.Models;
using Backend.DataSource.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.DataSources
{
    public class DiscenteDbDataSource : IDataSource<Discente>
    {
        private readonly AppDbContext _context;

        public DiscenteDbDataSource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Discente>> GetAllAsync()
        {
            return await _context.Discentes.ToListAsync();
        }

        public async Task<Discente?> GetByIdAsync(int id)
        {
            return await _context.Discentes.FindAsync(id);
        }

        // Função extra específica do DB para salvar múltiplos registros
        public async Task SaveRangeAsync(List<Discente> discentes)
        {
            _context.Discentes.AddRange(discentes);
            await _context.SaveChangesAsync();
        }
    }
}
