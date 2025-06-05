using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IClassRegistrationRepository : IRepository<ClassRegistration>
    {
        void Update(ClassRegistration classRegistrations);
    }
}
