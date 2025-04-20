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
    public class FeedbackClassViewModel
    {
        public ClassFeedback ClassFeedback { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ClassesList { get; set; }
    }
}
