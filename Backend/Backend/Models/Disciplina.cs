using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Disciplina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } // ID da disciplina

        public string Nome { get; set; } = string.Empty; // Nome da disciplina

        public string Curso { get; set; } = string.Empty; // Nome do curso

        public int Vagas { get; set; } // Número de vagas disponíveis

        public string Status { get; set; } = "aberta"; // Status da disciplina
    }
}
