namespace GymTasticWeb.Services
{
    public class FileService : IFileService
    {
        ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
            _logger.LogDebug("FileService() - File Services Instanaciated.");
        }

        public bool SaveFile(string folderPath, string filePath, IFormFile file)
        {
            _logger.LogDebug("SaveFile() - Started.");
            bool isSaved = false;
            if (file.Length > 0)
            {
                _logger.LogDebug($"      SaveFile() - Checking if Directory Exists: {folderPath}");
                if (!Directory.Exists(folderPath))
                {
                    _logger.LogDebug($"      SaveFile() - Directory {folderPath} does not exists");
                    Directory.CreateDirectory(folderPath);
                    _logger.LogDebug($"      SaveFile() - Directory {folderPath} Created");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    _logger.LogDebug($"      SaveFile() - File {filePath} Created");
                }
                isSaved = true;
            }
            else
            {
                _logger.LogDebug($"      SaveFile() - File {filePath} is 0 length");
                _logger.LogDebug($"      SaveFile() - File Not Saved");
            }
            _logger.LogDebug($"SaveFile() - Completed Sucessfuly");
            return isSaved;

        }

        public async Task<bool> SaveFileAsync(string folderPath, string filePath, IFormFile file)
        {
            _logger.LogDebug("SaveFileAsync() - Started.");
            bool isSaved = false;
            if (file.Length > 0)
            {
                _logger.LogDebug($"      SaveFileAsync() - Checking if Directory Exists: {folderPath}");
                if (!Directory.Exists(folderPath))
                {
                    _logger.LogDebug($"      SaveFileAsync() - Directory {folderPath} does not exists");
                    Directory.CreateDirectory(folderPath);
                    _logger.LogDebug($"      SaveFileAsync() - Directory {folderPath} Created");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    _logger.LogDebug($"      SaveFileAsync() - File {filePath} Created");
                }
                isSaved = true;
            }
            else
            {
                _logger.LogDebug($"      SaveFileAsync() - File {filePath} is 0 length");
                _logger.LogDebug($"      SaveFileAsync() - File Not Saved");
            }
            _logger.LogDebug($"SaveFile() - Completed Sucessfuly");
            return isSaved;

        }

        public bool ValidateFileName(IFormFile file)
        {
            _logger.LogDebug($"ValidateFileName() - Satarted.");
            bool isValid = false;
            if (!(file.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
            {
                _logger.LogDebug($"     ValidateFileName() - File {file.FileName} is valid.");

                isValid = true;
            }
            else
            {
                _logger.LogDebug($"     ValidateFileName() - File {file.FileName} is invalid.");

            }
            _logger.LogDebug($"ValidateFileName() - Completed Sucessfuly");

            return isValid;
        }
    }
}
