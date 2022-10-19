using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.DocumentTypes
{
    public class DeleteModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public DeleteModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public DocumentType DocumentType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            var documenttype = await _context.DocumentTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (documenttype == null)
            {
                return NotFound();
            }
            else 
            {
                DocumentType = documenttype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            if (!User.IsInRole(_configuration.GetSection("DocumentTypeMaintainerRole").Get<string>()))
            {
                TempData["Error"] = "У вас нет прав для совершения данного действия";
                return RedirectToPage("./Index");
            }
                

            var isContainingValues = await _context.DocumentTypes
                .Where(x => x.Id == id)
                .Include(x => x.ArchivedFiles)
                .SelectMany(x => x.ArchivedFiles!).AnyAsync();

            var documenttype = await _context.DocumentTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (documenttype == null) 
                return NotFound();

            DocumentType = documenttype;
                
            if (isContainingValues)
            {
                TempData["Error"] = "Удаление невозможно, пока в архиве существуют файлы имеющие данные тип.";
                return Page();
            }
            
            _context.DocumentTypes.Remove(DocumentType);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
    }
}
