using backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LivroReserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DiscenteId { get; set; }

        [Required]
        public int LivroId { get; set; }

        public Discente? Discente { get; set; }
        public Livro? Livro { get; set; }
    }
}
