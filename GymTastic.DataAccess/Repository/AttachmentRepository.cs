using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        private readonly ApplicationDbContext _db;
        public AttachmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Attachment attachment)
        {
            _db.Attachments.Update(attachment);
        }
    }
}
