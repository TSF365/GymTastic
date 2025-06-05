using GymTastic.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.ViewModels
{
    public class PTrainingTrainerAtleteViewModel
    {
        public PersonalizedTraining PersonalizedTraining { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TrainerList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AtleteList { get; set; }
    }
}
