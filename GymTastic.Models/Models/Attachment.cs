using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.Models.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public Guid FileGuid { get; set; }
        public int AtleteId { get; set; }
        public Atlete Atlete { get; set; }
        [Required(ErrorMessage = "O Documento tem que ter um título.")]
        [StringLength(120, ErrorMessage = "O Título do Documento não pode ter mais de 120 caracteres.")]
        [DisplayName("Título")]
        public string? Title { get; set; }
        [StringLength(2048)]
        [DisplayName("Descrição")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Anexo")]
        public string FileName { get; set; }
        public string FullFileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        [Required(ErrorMessage = "É obrigatorio classificar os documentos que são carregados.")]
        [DisplayName("Tipo de Documento")]
        public int FileClassificationTypeId { get; set; }
        public FileClassificationType FileClassificationType { get; set; }
        [DisplayName("Data do Documento")]
        public DateTime CreatedDate { get; set; }
    }
}
