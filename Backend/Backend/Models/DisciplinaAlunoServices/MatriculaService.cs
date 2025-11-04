using backend.Data;
using backend.Models;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.DisciplinaAlunoServices
{
    public class MatriculaService
    {
        private readonly AppDbContext _context;

        public MatriculaService(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as matrículas
        public async Task<List<DisciplinaAluno>> GetAllAsync()
        {
            return await _context.DisciplinaAluno
                .Include(da => da.Discente)
                .Include(da => da.Disciplina)
                .ToListAsync();
        }

        // Listar matrículas de um discente
        public async Task<List<DisciplinaAluno>> GetByDiscenteAsync(int discenteId)
        {
            return await _context.DisciplinaAluno
                .Include(da => da.Disciplina)
                .Where(da => da.DiscenteId == discenteId)
                .ToListAsync();
        }

        // Matricular um aluno em uma disciplina
        public async Task<bool> MatricularAlunoAsync(int discenteId, int disciplinaId)
        {
            // Verifica se já está matriculado
            var exists = await _context.DisciplinaAluno
                .AnyAsync(da => da.DiscenteId == discenteId && da.DisciplinaId == disciplinaId);

            if (exists)
                return false; // Já matriculado

            // Verifica se disciplina existe e se ainda há vagas
            var disciplina = await _context.Disciplinas.FindAsync(disciplinaId);
            if (disciplina == null || disciplina.Vagas <= 0)
                return false;

            // Adiciona matrícula
            var matricula = new DisciplinaAluno
            {
                DiscenteId = discenteId,
                DisciplinaId = disciplinaId
            };

            _context.DisciplinaAluno.Add(matricula);

            // Atualiza vagas
            disciplina.Vagas -= 1;

            await _context.SaveChangesAsync();
            return true;
        }

        // Cancelar matrícula
        public async Task<bool> CancelarMatriculaAsync(int discenteId, int disciplinaId)
        {
            var matricula = await _context.DisciplinaAluno
                .FirstOrDefaultAsync(da => da.DiscenteId == discenteId && da.DisciplinaId == disciplinaId);

            if (matricula == null)
                return false;

            _context.DisciplinaAluno.Remove(matricula);

            // Devolver vaga
            var disciplina = await _context.Disciplinas.FindAsync(disciplinaId);
            if (disciplina != null)
                disciplina.Vagas += 1;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
