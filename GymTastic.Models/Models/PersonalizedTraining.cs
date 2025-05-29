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
    public class PersonalizedTraining
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome do Treino é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Nome do Treino não pode ter mais de 50 caracteres.")]
        [DisplayName("Nome do Treino")]
        public string TrainingName { get; set; }

        [Required(ErrorMessage = "O Treinador do Treino é obrigatório.")]
        [DisplayName("Treinador")]
        public int TrainerId { get; set; }
        [ValidateNever]
        public Trainer Trainer { get; set; }

        [Required(ErrorMessage = "O Atleta do Treino é obrigatório.")]
        [DisplayName("Atleta")]
        public int AtleteId { get; set; }
        [ValidateNever]
        public Atlete Atlete { get; set; }

        [Required(ErrorMessage = "O Objetivo do Treino é obrigatório.")]
        [StringLength(120, ErrorMessage = "O Objetivo do Treino não pode ter mais de 120 caracteres.")]
        [DisplayName("Objetivo do Treino")]
        public string TrainingObjective { get; set; }
    }
}
