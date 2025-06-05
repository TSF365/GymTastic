using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class Speciality
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome da especialidade é obrigatório")]
        [DisplayName("Nome Especialidade")] 
        public string Name { get; set; }
    }
}
