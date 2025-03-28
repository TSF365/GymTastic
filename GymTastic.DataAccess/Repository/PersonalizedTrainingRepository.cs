using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository
{
    public class PersonalizedTrainingRepository : Repository<PersonalizedTraining>, IPersonalizedTrainingRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonalizedTrainingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PersonalizedTraining personalizedTraining)
        {
            _db.PersonalizedTraining.Update(personalizedTraining);
        }
    }
}
