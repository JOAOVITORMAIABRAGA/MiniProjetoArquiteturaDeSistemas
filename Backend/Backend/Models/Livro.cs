using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Livro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } // ID do livro
        public string Titulo { get; set; } = string.Empty; // Título do livro
        public string Autor { get; set; } = string.Empty; // Autor do livro
        public int Ano { get; set; } // Ano de publicação
        public string Status { get; set; } = "disponível"; // Status do livro
    }
}
