using GymTastic.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.ViewModels
{
    public class ClassesTrainerViewModel
    {
        public Classes Classes { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TrainerList { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> SpecialityList { get; set; }
    }
}
