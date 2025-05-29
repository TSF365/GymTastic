using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O preenchimento do Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "O Nome não pode ter mais de 120 caracteres")]
        [Display(Name = "Nome Completo")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O preenchimento da Especialidade é obrigatório")]
        [StringLength(60, ErrorMessage = "A Especialidade não pode ter mais de 60 caracteres")]
        [Display(Name = "Especialidade")]
        public string Specialty { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public IdentityUser User { get; set; }

        // Titulo Profissional de Treinador Desportivo

        [Required(ErrorMessage = "O preenchimento do TPTD é obrigatório")]
        [DisplayName("TPTD")]
        public int TPTD { get; set; }

        [Required(ErrorMessage = "O preenchimento da Data do TPTD é obrigatório")]
        [Display(Name = "Data do TPTD")]
        public DateTime TptdDate { get; set; }

        // Dados de Contacto

        [Required(ErrorMessage = "O preenchimento do Email é obrigatório")]
        [StringLength(120, ErrorMessage = "O Email não pode ter mais de 120 caracteres")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O preenchimento do Telemóvel é obrigatório")]
        [StringLength(13, ErrorMessage = "O Telemóvel não pode ter mais de 13 caracteres")]
        [Display(Name = "Telemóvel")]
        public string PhoneNumber { get; set; }
    }
}
