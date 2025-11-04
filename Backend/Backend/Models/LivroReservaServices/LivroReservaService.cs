using backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.LivroReservaService
{
    public class LivroReservaService
    {
        private readonly AppDbContext _context;

        public LivroReservaService(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as reservas
        public async Task<List<LivroReserva>> GetAllAsync()
        {
            return await _context.LivroReserva
                .Include(lr => lr.Discente)
                .Include(lr => lr.Livro)
                .ToListAsync();
        }

        // Listar reservas de um discente
        public async Task<List<LivroReserva>> GetByDiscenteAsync(int discenteId)
        {
            return await _context.LivroReserva
                .Include(lr => lr.Livro)
                .Where(lr => lr.DiscenteId == discenteId)
                .ToListAsync();
        }

        // Reservar um livro
        public async Task<bool> ReservarLivroAsync(int discenteId, int livroId)
        {
            // ✅ Verifica se aluno existe
            var discente = await _context.Discentes.FirstOrDefaultAsync(d => d.Id == discenteId);
            if (discente == null)
                return false;

            // ✅ Verifica status do discente (somente "Ativo" pode reservar)
            if (!string.Equals(discente.Status, "Ativo", StringComparison.OrdinalIgnoreCase))
                return false;

            // Verifica se já existe reserva
            var exists = await _context.LivroReserva
                .AnyAsync(lr => lr.DiscenteId == discenteId && lr.LivroId == livroId);

            if (exists)
                return false;

            // Verifica disponibilidade
            var livro = await _context.Livros.FindAsync(livroId);
            if (livro == null || livro.Status != "Disponível")
                return false;

            var reserva = new LivroReserva
            {
                DiscenteId = discenteId,
                LivroId = livroId
            };

            _context.LivroReserva.Add(reserva);

            // Marca livro como indisponível
            livro.Status = "Indisponível";

            await _context.SaveChangesAsync();
            return true;
        }

        // Cancelar reserva
        public async Task<bool> CancelarReservaAsync(int discenteId, int livroId)
        {
            var reserva = await _context.LivroReserva
                .FirstOrDefaultAsync(lr => lr.DiscenteId == discenteId && lr.LivroId == livroId);

            if (reserva == null)
                return false;

            _context.LivroReserva.Remove(reserva);

            // Volta o livro para disponível
            var livro = await _context.Livros.FindAsync(livroId);
            if (livro != null)
                livro.Status = "Disponível";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
