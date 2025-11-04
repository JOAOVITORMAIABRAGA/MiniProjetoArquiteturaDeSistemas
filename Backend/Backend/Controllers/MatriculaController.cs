using backend.Models;
using Backend.Models;
using Backend.Models.DisciplinaAlunoServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ControllerBase
    {
        private readonly MatriculaService _matriculaService;

        public MatriculaController(MatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        // GET: api/Matricula
        [HttpGet]
        public async Task<ActionResult<List<DisciplinaAluno>>> GetAll()
        {
            var result = await _matriculaService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Matricula/discente/5
        [HttpGet("discente/{discenteId}")]
        public async Task<ActionResult<List<DisciplinaAluno>>> GetByDiscente(int discenteId)
        {
            var result = await _matriculaService.GetByDiscenteAsync(discenteId);
            return Ok(result);
        }

        // POST: api/Matricula/matricular
        [HttpPost("matricular")]
        public async Task<ActionResult> Matricular([FromBody] MatriculaRequest request)
        {
            var success = await _matriculaService.MatricularAlunoAsync(request.DiscenteId, request.DisciplinaId);
            if (!success)
                return BadRequest("Não foi possível matricular o aluno. Verifique duplicidade ou vagas.");

            return Ok("Aluno matriculado com sucesso.");
        }

        // DELETE: api/Matricula/cancelar
        [HttpDelete("cancelar")]
        public async Task<ActionResult> Cancelar([FromBody] MatriculaRequest request)
        {
            var success = await _matriculaService.CancelarMatriculaAsync(request.DiscenteId, request.DisciplinaId);
            if (!success)
                return NotFound("Matrícula não encontrada.");

            return Ok("Matrícula cancelada com sucesso.");
        }
    }

    // DTO para requisições de matrícula
    public class MatriculaRequest
    {
        public int DiscenteId { get; set; }
        public int DisciplinaId { get; set; }
    }
}
