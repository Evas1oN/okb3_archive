using System.ComponentModel.DataAnnotations;

namespace okb3_archive.Data.Models;

public class DocumentType
{
    public Guid Id { get; set; }
    
    [Display(Name = "Наименование")]
    public string Name { get; set; }
    
    public List<FileEntry>? ArchivedFiles { get; set; }
}