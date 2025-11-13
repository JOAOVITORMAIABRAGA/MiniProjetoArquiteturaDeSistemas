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
        // Matricular um aluno em uma disciplina
        // Matricular um aluno em uma disciplina
        public async Task<bool> MatricularAlunoAsync(int discenteId, int disciplinaId)
        {
            // ✅ Verifica se aluno existe
            var discente = await _context.Discentes.FirstOrDefaultAsync(d => d.Id == discenteId);
            if (discente == null)
                return false;

            // ✅ Verifica status ("Ativo" pode, outros NÃO podem)
            if (!string.Equals(discente.Status, "Ativo", StringComparison.OrdinalIgnoreCase))
                return false;

            // ✅ Verifica se já está matriculado nessa disciplina
            var exists = await _context.DisciplinaAluno
                .AnyAsync(da => da.DiscenteId == discenteId && da.DisciplinaId == disciplinaId);
            if (exists)
                return false; // Já matriculado

            // ✅ Verifica quantidade de disciplinas em que o aluno já está matriculado
            var totalMatriculas = await _context.DisciplinaAluno
                .CountAsync(da => da.DiscenteId == discenteId);
            if (totalMatriculas >= 5)
                return false; // ❌ Aluno já atingiu o limite de 5 disciplinas

            // ✅ Verifica se disciplina existe e se ainda há vagas
            var disciplina = await _context.Disciplinas.FindAsync(disciplinaId);
            if (disciplina == null || disciplina.Vagas <= 0)
                return false;

            // ✅ Verifica se o curso do aluno é o mesmo da disciplina
            if (discente.Curso != disciplina.Curso)
                return false; // ❌ Aluno tentando se matricular em disciplina de outro curso

            // ✅ Adiciona matrícula
            var matricula = new DisciplinaAluno
            {
                DiscenteId = discenteId,
                DisciplinaId = disciplinaId
            };

            _context.DisciplinaAluno.Add(matricula);

            // ✅ Atualiza vagas
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
