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
    public class ClassFeedback
    {
        [Key]
        public int Id { get; set; }

        public int AtleteId { get; set; }
        [ValidateNever]
        public Atlete Atlete { get; set; }

        public int ClassId { get; set; }
        [ValidateNever]
        public Classes Class { get; set; }

        public string Comment { get; set; }
        [Required(ErrorMessage = "O Rating da Aula tem de ser preenchido")]
        [Range(1, 10, ErrorMessage = "O Rating deve estar entre 1 e 10")]
        [DisplayName("Rating(1-10)")]
        public int Rating { get; set; }


        public DateTime FeedbackDate { get; set; }
    }
}
