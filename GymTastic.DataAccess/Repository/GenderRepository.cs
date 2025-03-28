using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private readonly ApplicationDbContext _db;
        public GenderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Gender gender)
        {
            _db.Genders.Update(gender);
        }
    }
}
