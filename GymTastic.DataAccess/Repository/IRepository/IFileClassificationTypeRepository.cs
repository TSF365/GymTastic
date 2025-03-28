using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IFileClassificationTypeRepository : IRepository<FileClassificationType>
    {
        void Update(FileClassificationType fileClassificationType);
    }
}
