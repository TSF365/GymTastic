using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class ClassRegistration
    {
        [Key]
        public int Id { get; set; }

        public int AtleteId { get; set; }

        public int ClassId { get; set; }
    }
}
