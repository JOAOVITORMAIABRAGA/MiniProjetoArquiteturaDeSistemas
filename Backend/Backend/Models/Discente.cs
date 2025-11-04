using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Discente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // ❌ Sem IDENTITY
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Modalidade { get; set; } = string.Empty;
        public int Semestre { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
