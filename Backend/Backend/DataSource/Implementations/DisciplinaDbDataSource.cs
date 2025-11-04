
using backend.Data;
using backend.Models;
using Backend.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.DataSources
{
    public class DisciplinaDbDataSource : IDataSource<Disciplina>
    {
        private readonly AppDbContext _context;

        public DisciplinaDbDataSource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Disciplina>> GetAllAsync()
        {
            return await _context.Disciplinas.ToListAsync();
        }

        public async Task<Disciplina?> GetByIdAsync(int id)
        {
            return await _context.Disciplinas.FindAsync(id);
        }

        // Função extra específica do DB para salvar múltiplos registros (paralelo ao Discente)
        public async Task SaveRangeAsync(List<Disciplina> disciplinas)
        {
            _context.Disciplinas.AddRange(disciplinas);
            await _context.SaveChangesAsync();
        }
    }
}