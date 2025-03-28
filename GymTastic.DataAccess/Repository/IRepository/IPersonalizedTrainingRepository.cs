using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IPersonalizedTrainingRepository : IRepository<PersonalizedTraining>
    {
        void Update(PersonalizedTraining personalizedTraining);
    }
}
