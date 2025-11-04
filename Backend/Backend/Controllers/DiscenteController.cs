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
                return NotFound();
            return Ok(discente);
        }
    }
}
