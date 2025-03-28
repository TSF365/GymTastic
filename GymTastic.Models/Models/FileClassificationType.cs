using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class FileClassificationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(128)]
        [DisplayName("File Classification Type")]
        public string Description { get; set; }
    }
}
