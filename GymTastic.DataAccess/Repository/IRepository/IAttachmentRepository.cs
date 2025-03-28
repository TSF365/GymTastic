using GymTastic.Models.Models;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        void Update(Attachment attachment);
    }
}
