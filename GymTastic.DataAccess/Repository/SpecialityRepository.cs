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
    public class SpecialityRepository : Repository<Speciality>, ISpecialityRepository
    {
        private readonly ApplicationDbContext _db;
        public SpecialityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Speciality speciality)
        {
            _db.Specialities.Update(speciality);
        }
    }
}
