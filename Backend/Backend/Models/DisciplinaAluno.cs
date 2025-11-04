using Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class DisciplinaAluno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DiscenteId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        // Relacionamento EF Core (opcional)
        public Discente? Discente { get; set; }
        public Disciplina? Disciplina { get; set; }
    }
}
