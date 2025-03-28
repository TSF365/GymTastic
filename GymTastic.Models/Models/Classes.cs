using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class Classes
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O Nome da Aula é obrigatorio.")]
        [StringLength(50, ErrorMessage = "O Nome da Aula não pode ter mais de 50 caracteres.")]
        public string ClassName { get; set; }
        [Required(ErrorMessage = "O Horário da Aula é obrigatorio.")]
        public DateTime ClassTime { get; set; }
        [Required(ErrorMessage = "O Treinador da Aula é obrigatorio.")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [Required(ErrorMessage = "O número de atletas permitidos é obrigatorio.")]
        public int MaxAtletes { get; set; }
    }
}
