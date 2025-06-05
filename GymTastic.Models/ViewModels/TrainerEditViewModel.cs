using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.ViewModels
{
    public class TrainerEditViewModel
    {
        public Trainer Trainer { get; set; }
        public List<Speciality> Specialities { get; set; } = new();
        public List<int> SelectedSpecialityIds { get; set; } = new();
    }

}
