using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class AtletePreferences
    {
        [Key]
        public int Id { get; set; }

        public int Id_Atlete { get; set; }

        public int Id_Preference { get; set; }
    }
}
