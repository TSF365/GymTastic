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
    public class AtleteViewModel
    {
        public Atlete Atlete { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FileClassificationTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<Attachment> AttachmentList { get; set; }
    }
}
