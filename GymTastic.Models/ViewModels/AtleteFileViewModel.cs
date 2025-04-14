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
    public class AtleteFileViewModel
    {
        [ValidateNever]
        public Atlete Atlete { get; set; }
        public Attachment Attachment { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FileClassificationTypeList { get; set; }
    }
}
