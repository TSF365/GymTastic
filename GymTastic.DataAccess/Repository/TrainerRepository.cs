using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        private readonly ApplicationDbContext _db;
        public TrainerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Trainer trainer)
        {
            _db.Trainers.Update(trainer);
        }
    }
}
