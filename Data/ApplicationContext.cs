using Microsoft.EntityFrameworkCore;
using okb3_archive.Data.Models;

namespace okb3_archive.Data;

public class ApplicationContext : DbContext
{
    public DbSet<FileEntry> ArchivedFiles { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
}