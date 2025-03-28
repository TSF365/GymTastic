using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IGenderRepository : IRepository<Gender>
    {
        void Update(Gender gender);
    }
}
