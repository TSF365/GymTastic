using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository
{
    public class FileClassificationTypeRepository : Repository<FileClassificationType>, IFileClassificationTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public FileClassificationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FileClassificationType fileClassificationType)
        {
            _db.FileClassificationTypes.Update(fileClassificationType);
        }
    }
}
