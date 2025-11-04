using backend.Models;
using Backend.Models;
using Backend.Models.BibliotecaServices;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecaController : ControllerBase
    {
        private readonly BibliotecaService _service;

        public BibliotecaController(BibliotecaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros()
        {
            var livros = await _service.GetLivros();
            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivroById(int id)
        {
            var livro = await _service.GetLivroById(id);
            if (livro == null)
                return NotFound();
            return Ok(livro);
        }
    }
}