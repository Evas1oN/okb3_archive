using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;

namespace okb3_archive.Data.Models;

public class FileEntry
{
    public Guid Id { get; set; }
    
    [Display(Name = "Наименование")]
    public string Name { get; set; }
    
    [Display(Name = "Имя файла")]
    public string? FileName { get; set; }

    [Display(Name = "Дата архивации")]
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    [Display(Name = "Тип документа")]
    public DocumentType? DocumentType { get; set; }
    
    [Display(Name = "Файл")]
    public ZippedFile? ZippedFile { get; set; }
    public string? MD5Hash { get; set; }

    public Guid FileId { get; set; }
    public Guid DocumentTypeId { get; set; }
}

