using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IAtleteRepository Atlete { get; private set; }
        public IClassesRepository Classes { get; private set; }
        public IClassFeedbackRepository ClassFeedback { get; private set; }
        public IGenderRepository Gender { get; private set; }
        public IPersonalizedTrainingRepository PersonalizedTraining { get; private set; }
        public ITrainerRepository Trainer { get; private set; }
        public IAttachmentRepository Attachment { get; private set; }
        public IFileClassificationTypeRepository FileClassificationType { get; private set; }
        public ISpecialityRepository Speciality { get; private set; }
        public ITrainerSpecialityRepository TrainerSpeciality { get; private set; }
        public IClassRegistrationRepository ClassRegistration { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Atlete = new AtleteRepository(_db);
            Classes = new ClassesRepository(_db);
            ClassFeedback = new ClassFeedbackRepository(_db);
            Gender = new GenderRepository(_db);
            PersonalizedTraining = new PersonalizedTrainingRepository(_db);
            Trainer = new TrainerRepository(_db);
            Attachment = new AttachmentRepository(_db);
            FileClassificationType = new FileClassificationTypeRepository(_db);
            Speciality = new SpecialityRepository(_db);
            TrainerSpeciality = new TrainerSpecialityRepository(_db);
            ClassRegistration = new ClassRegistrationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
