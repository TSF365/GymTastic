using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAtleteRepository Atlete { get; }
        IClassesRepository Classes { get; }
        IClassFeedbackRepository ClassFeedback { get; }
        IAttachmentRepository Attachment { get; }
        IGenderRepository Gender { get; }
        IFileClassificationTypeRepository FileClassificationType { get; }
        IPersonalizedTrainingRepository PersonalizedTraining { get; }
        ITrainerRepository Trainer { get; }
        ISpecialityRepository Speciality { get; }
        ITrainerSpecialityRepository TrainerSpeciality { get; }
        IClassRegistrationRepository ClassRegistration { get; }

        void Save();
    }
}
