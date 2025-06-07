using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Nome da Aula")]
        public string ClassName { get; set; }
        [Required(ErrorMessage = "O Horário da Aula é obrigatorio.")]
        [DisplayName("Tempo da Aula")]
        public DateTime ClassTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O Treinador da Aula é obrigatorio.")]
        [DisplayName("Treinador")]
        public int? TrainerId { get; set; }
        [ValidateNever]
        public Trainer Trainer { get; set; }

        [Required(ErrorMessage = "O tipo de aula é obrigatorio.")]
        [DisplayName("Tipo de Aula")]
        public int? SpecialityId { get; set; }
        [ValidateNever]

        public Speciality Speciality { get; set; }

        [DisplayName("Atletas Inscritos")]
        //[DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int RegAtletes { get; set; }  //Atletas Inscritos
        [Required(ErrorMessage = "O número de atletas permitidos é obrigatorio.")]
        [DisplayName("Máximo de Atletas")]
        public int MaxAtletes { get; set; }

    }
}
