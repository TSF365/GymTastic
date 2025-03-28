using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface ITrainerRepository : IRepository<Trainer>
    {
        void Update(Trainer trainer);
    }
}
