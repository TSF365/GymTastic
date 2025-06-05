using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.DataAccess.Repository
{
    public class ClassRegistrationRepository : Repository<ClassRegistration>, IClassRegistrationRepository
    {
        private readonly ApplicationDbContext _db;
        public ClassRegistrationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ClassRegistration classRegistration)
        {
            _db.ClassRegistrations.Update(classRegistration);
        }
    }
}
