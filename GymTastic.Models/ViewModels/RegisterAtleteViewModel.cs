using GymTastic.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymTasticWeb.ViewModels
{
    public class RegisterAtleteViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
        public string ConfirmPassword { get; set; }

        public Atlete Atlete { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> Genders { get; set; }
        
        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> PreferencesList { get; set; }
        [Display(Name = "Preferências")]
        public List<int> SelectedPreferences { get; set; }

    }
}
