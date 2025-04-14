using GymTastic.DataAccess.Repository;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.ViewModels;
using GymTasticWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        public FileController(IUnitOfWork unitOfWork, IFileService fileService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            GymTastic.Models.Models.Atlete? atleteResult = _unitOfWork.Atlete.Get(i => i.Id == Id);
            if (atleteResult == null)
            {
                return NotFound();
            }

            AtleteFileViewModel atleteFileViewModel = new AtleteFileViewModel();
            atleteFileViewModel.FileClassificationTypeList = _unitOfWork.FileClassificationType.GetAll().Select(u => new SelectListItem
            {
                Text = u.Description,
                Value = u.Id.ToString()
            });

            atleteFileViewModel.Atlete = atleteResult;
            atleteFileViewModel.Attachment = new GymTastic.Models.Models.Attachment();
            atleteFileViewModel.Attachment.AtleteId = atleteResult.Id;

            return View(atleteFileViewModel);

        }

        //[Authorize]
        [HttpPost]
        public IActionResult Create(AtleteFileViewModel atleteFileViewModel, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Anexo é Obrigatório");
            }

            if (!_fileService.ValidateFileName(file))
            {
                return BadRequest("Ficheiro com nomenclatura inválida");
            }

            // Complementa a informação sobre o ficheiro
            var attachment = atleteFileViewModel.Attachment;
            attachment.CreatedDate = DateTime.Now;
            attachment.FileName = file.FileName;
            attachment.FileSize = file.Length;
            attachment.FileGuid = Guid.NewGuid();
                
            var folderPath = Path.Combine(_configuration.GetSection("Settings").GetValue<string>("DefaultWebRootFolder"), _configuration.GetSection("Settings").GetValue<string>("AttachmentsFolder"));
            var filePath = Path.Combine(folderPath, attachment.FileGuid.ToString() + "_" + attachment.FileName);
            attachment.FullFileName = filePath;

            attachment.FileExtension = Path.GetExtension(filePath);
            attachment.MimeType = MimeTypes.GetMimeType(filePath);

            if (_fileService.SaveFile(folderPath, filePath, file))
            {
                _unitOfWork.Attachment.Add(attachment);
                _unitOfWork.Save();
                TempData["success"] = "Anexo adicionado com sucesso.";
                return RedirectToAction("Index", "Atlete", attachment.AtleteId);
            }
            else
            {
                TempData["success"] = "Anexo adicionado com sucesso.";
                return RedirectToAction("Index", "Atlete");
            }

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var attachment = _unitOfWork.Attachment.Get(u => u.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }
            return View(attachment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var attachment = _unitOfWork.Attachment.Get(u => u.Id == id);
            _unitOfWork.Attachment.Remove(attachment);
            _unitOfWork.Save();
            return RedirectToAction("index", "Atlete");
        }

        public IActionResult Download(int id)
        {
            var attachment = _unitOfWork.Attachment.Get(u => u.Id == id);
            if (attachment == null)
            {
                return NotFound("File Not Found");
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = attachment.FileName,
                Inline = true
            };
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachment.FullFileName);
            string fileName = attachment.FileName;
            string mimeType = attachment.MimeType;

            return File(fileBytes, mimeType, fileName);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            string[] includes =
            [
                nameof(GymTastic.Models.Models.Attachment.Atlete),
                nameof(GymTastic.Models.Models.Attachment.FileClassificationType)
            ];

            var attachment = _unitOfWork.Attachment.Get(i => i.Id == Id, includes);
            if (attachment == null)
            {
                return NotFound();
            }

            AtleteFileViewModel atleteFileViewModel = new AtleteFileViewModel
            {
                Atlete = attachment.Atlete,
                Attachment = attachment,
                FileClassificationTypeList = _unitOfWork.FileClassificationType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Description,
                    Value = u.Id.ToString(),
                    Selected = (u.Id == attachment.FileClassificationTypeId)
                }).ToList()
            };

            return View(atleteFileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AtleteFileViewModel atleteFileViewModel)
        {

            string[] includes =
            [
                nameof(GymTastic.Models.Models.Attachment.Atlete),
                nameof(GymTastic.Models.Models.Attachment.FileClassificationType)
            ];
            var attachmentDb = _unitOfWork.Attachment.Get(i => i.Id == atleteFileViewModel.Attachment.Id, includes);
            if (attachmentDb == null)
            {
                TempData["error"] = "O anexo não foi encontrado.";
                return RedirectToAction("Index", "Atlete");
            }

            try
            {
                // Atualizar apenas os campos que foram editados
                attachmentDb.Title = atleteFileViewModel.Attachment.Title;
                attachmentDb.Description = atleteFileViewModel.Attachment.Description;
                attachmentDb.CreatedDate = atleteFileViewModel.Attachment.CreatedDate;

                _unitOfWork.Attachment.Update(attachmentDb);
                _unitOfWork.Save();
                TempData["success"] = "Anexo atualizado com sucesso.";
            }
            catch (Exception)
            {
                TempData["error"] = "Não foi possível atualizar o anexo.";
                return View(atleteFileViewModel);
            }

            return RedirectToAction("Index", "Atlete");
        }
    }
}
