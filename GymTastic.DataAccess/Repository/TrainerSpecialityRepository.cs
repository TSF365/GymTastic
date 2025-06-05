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
    public class TrainerSpecialityRepository : Repository<TrainerSpeciality>, ITrainerSpecialityRepository
    {
        private readonly ApplicationDbContext _db;
        public TrainerSpecialityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TrainerSpeciality trainerSpeciality)
        {
            _db.TrainerSpecialities.Update(trainerSpeciality);
        }
    }
}
