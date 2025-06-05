using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Utility
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            if (value is DateTime data)
            {
                var dataMinima = new DateTime(1900, 1, 1);
                var dataMaxima = DateTime.Today;

                return data >= dataMinima && data <= dataMaxima;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"A {name} deve estar entre 01/01/1900 e {DateTime.Today:dd/MM/yyyy}.";
        }
    }
}
