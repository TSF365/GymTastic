using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GymTasticWeb.Services
{
    public interface IFileService
    {
        Task<bool> SaveFileAsync(string folderPath, string filePath, IFormFile file);
        bool SaveFile(string folderPath, string filePath, IFormFile file);
        bool ValidateFileName(IFormFile file);
    }
}