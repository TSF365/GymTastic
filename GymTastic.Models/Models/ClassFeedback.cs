using System;
using System.Collections.Generic;
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
        public Atlete Atlete { get; set; }

        public int ClassId { get; set; }
        public Classes Class { get; set; }

        public string Comment { get; set; }
        public int Rating { get; set; }

        public DateTime FeedbackDate { get; set; }
    }
}
