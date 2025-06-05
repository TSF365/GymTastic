using GymTastic.Utility;
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
    public class Atlete
    {

        // Dados Base do Atleta
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O preenchimento do Primeiro Nome é obrigatório")]
        [StringLength(32, ErrorMessage = "O Primeiro nome não pode ter mais de 32 caracteres")]
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "O preenchimento do Apelido é obrigatório")]
        [StringLength(96, ErrorMessage = "O Apelido não pode ter mais de 96 caracteres")]
        [Display(Name = "Apelido")]
        public string LastName { get; set; }

        [DisplayName("Nome Completo")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public IdentityUser User { get; set; }

        //public int SpecialtyId { get; set; }

        //[ForeignKey("SpecialtyId")]
        //[ValidateNever]
        //public Speciality Speciality { get; set; }

        // Dados Pessoais

        [Required(ErrorMessage = "O preenchimento da Data de Nascimento é obrigatório")]
        [Display(Name = "Data de Nascimento")]
        [BirthDateValidation]
        public DateTime BirthDate { get; set; } 

        [DisplayName("Idade")]
        public int Age
        {
            get
            {
                return DateTime.Now.Year - BirthDate.Year;
            }
        }

        [DisplayName("Ano de Nascimento")]
        public int Year
        {
            get
            {
                return BirthDate.Year;
            }
        }

        [Required(ErrorMessage = "É obrigatório o preenchimento do campo Sexo.")]
        [DisplayName("Sexo")]
        [Range(1, int.MaxValue)]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        [ValidateNever]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "O preenchimento do Número de Identificação Fiscal é obrigatório")]
        [DisplayName("NIF")]
        [StringLength(9, ErrorMessage = "O NIF deve ter 9 dígitos.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF deve conter apenas 9 dígitos.")]
        public string FIN { get; set; }


        [Required(ErrorMessage = "O preenchimento do Cartão de Cidadão é obrigatório")]
        [DisplayName("Cartão de Cidadão")]
        [StringLength(8, ErrorMessage = "O Cartão de Cidadão deve ter 8 dígitos.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "O Cartão de Cidadão deve conter apenas 8 dígitos.")]
        public string? CC { get; set; }

        public DateTime InscriptionDate { get; set; } = DateTime.UtcNow;


        // Dados de Contacto

        [Required(ErrorMessage = "O preenchimento do Email é obrigatório")]
        [EmailAddress(ErrorMessage = "O Email introduzido não é válido")]
        [DisplayName("E-mail")]
        [StringLength(320)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O preenchimento do Número de Telemóvel é obrigatório")]
        [DisplayName("Telemóvel")]
        [StringLength(9, ErrorMessage = "O Telemóvel deve ter 9 dígitos.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O Telemóvel deve conter apenas 9 dígitos.")]
        public string PhoneNumber { get; set; }

        // Dados de Morada

        [Required(ErrorMessage = "O preenchimento da Morada é obrigatório")]
        [DisplayName("Morada")]
        [StringLength(256)]
        public string Address { get; set; }

        [Required(ErrorMessage = "O preenchimento do Código Postal é obrigatório")]
        [DisplayName("Código Postal")]
        [StringLength(8, ErrorMessage = "O Código Postal deve ter 8 dígitos no formato 0000-000.")]
        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O Código Postal deve estar no formato 0000-000.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "O preenchimento da Localidade é obrigatório")]
        [DisplayName("Localidade")]
        [StringLength(128)]
        public string City { get; set; }

        // Contactos de Emergência

        [Required(ErrorMessage = "O preenchimento do Nome do Contacto de Emergência é obrigatório")]
        [DisplayName("Nome do Contacto de Emergência")]
        [StringLength(128)]
        public string EmergencyContact { get; set; }

        [Required(ErrorMessage = "O preenchimento do Número de Telemóvel do Contacto de Emergência é obrigatório")]
        [DisplayName("Telemóvel do Contacto de Emergência")]
        [StringLength(9, ErrorMessage = "O Telemóvel do Contacto de Emergência deve ter 9 dígitos.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O Telemóvel do Contacto de Emergência deve conter apenas 9 dígitos.")]
        public string EmergencyPhone { get; set; }

        [DisplayName("E-mail de Emergência")]
        [EmailAddress(ErrorMessage = "O Email introduzido não é válido")]   
        [StringLength(320)]
        public string? EmergencyEmail { get; set; }

        // Dados de Saúde

        [DisplayName("Altura (M) ex: 1.77")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Range(1, 3, ErrorMessage = "A altura deve estar entre 1 e 3 metros.")]
        public float Height { get; set; }

        [DisplayName("Peso (Kg) ex: 77.2")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Range(1, 500, ErrorMessage = "O peso deve estar entre 1 e 500 kg.")]
        public float Weight { get; set; }
    }
}
