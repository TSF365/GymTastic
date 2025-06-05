using GymTastic.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class RegisterTrainerViewModel
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

    public Trainer Trainer { get; set; }

    // New: Selected speciality IDs
    [Display(Name = "Especialidades")]
    public List<int> SelectedSpecialityIds { get; set; } = new();

    // New: Dropdown options
    [ValidateNever]
    public IEnumerable<SelectListItem> SpecialityList { get; set; }
}
