using Backend.Models;
using Backend.Models.LivroReservaService;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly LivroReservaService _reservaService;

        public ReservaController(LivroReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/LivroReserva
        [HttpGet]
        public async Task<ActionResult<List<LivroReserva>>> GetAll()
        {
            var result = await _reservaService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/LivroReserva/discente/5
        [HttpGet("discente/{discenteId}")]
        public async Task<ActionResult<List<LivroReserva>>> GetByDiscente(int discenteId)
        {
            var result = await _reservaService.GetByDiscenteAsync(discenteId);
            return Ok(result);
        }

        // POST: api/LivroReserva/reservar
        [HttpPost("reservar")]
        public async Task<ActionResult> Reservar([FromBody] ReservaRequest request)
        {
            var success = await _reservaService.ReservarLivroAsync(request.DiscenteId, request.LivroId);
            if (!success)
                return BadRequest("Não foi possível reservar o livro. Verifique disponibilidade ou duplicidade.");

            return Ok("Livro reservado com sucesso.");
        }

        // DELETE: api/Reserva/cancelar/1/5
        [HttpDelete("cancelar/{discenteId:int}/{livroId:int}")]
        public async Task<ActionResult> Cancelar(int discenteId, int livroId)
        {
            var success = await _reservaService.CancelarReservaAsync(discenteId, livroId);

            if (!success)
                return NotFound("Reserva não encontrada.");

            return Ok("Reserva cancelada com sucesso.");
        }

    }

    // DTO para requisições de reserva
    public class ReservaRequest
    {
        public int DiscenteId { get; set; }
        public int LivroId { get; set; }
    }
}
