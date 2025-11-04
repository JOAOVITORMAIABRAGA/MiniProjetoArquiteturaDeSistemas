using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Models.DiscenteServices;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscenteController : ControllerBase
    {
        private readonly DiscenteService _service;

        public DiscenteController(DiscenteService service)
        {
            _service = service;
        }

        // GET api/discente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discente>>> GetDiscentes()
        {
            var discentes = await _service.GetDiscentes();
            return Ok(discentes);
        }

        // GET api/discente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discente>> GetDiscenteById(int id)
        {
            var discente = await _service.GetDiscenteById(id);
            if (discente == null)
                return NotFound("Discente não encontrado.");
            return Ok(discente);
        }

        // PUT api/discente/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AlterarStatus(int id, [FromBody] string novoStatus)
        {
            var (success, message) = await _service.AlterarStatusAsync(id, novoStatus);

            if (!success)
                return NotFound(message);

            return Ok(message);
        }
    }
}
