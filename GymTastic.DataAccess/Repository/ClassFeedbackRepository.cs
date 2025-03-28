using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;

namespace GymTastic.DataAccess.Repository
{
    public class ClassFeedbackRepository : Repository<ClassFeedback>, IClassFeedbackRepository
    {
        private readonly ApplicationDbContext _db;
        public ClassFeedbackRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ClassFeedback classFeedback)
        {
            _db.ClassFeedbacks.Update(classFeedback);
        }
    }
}
