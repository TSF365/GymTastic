using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class TrainerSpeciality
    {
        [Key]
        public int ID { get; set; }
        public int Id_Speciality { get; set; }
        public int Id_Trainer {  get; set; }
    }
}
