using backend.Models;
using Backend.Models;
using Backend.Models.DisciplinaServices;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinaController : ControllerBase
    {
        private readonly DisciplinaService _service;

        public DisciplinaController(DisciplinaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disciplina>>> GetDisciplinas()
        {
            var disciplinas = await _service.GetDisciplinas();
            return Ok(disciplinas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disciplina>> GetDisciplinaById(int id)
        {
            var disciplina = await _service.GetDisciplinaById(id);
            if (disciplina == null)
                return NotFound();
            return Ok(disciplina);
        }
    }
}