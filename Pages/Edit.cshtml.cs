using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.Archive
{
    public class EditModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;
        private readonly IConfiguration _configuration;
        public bool IsDocumentTypeMaintainer = false;
        public EditModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public FileEntry FileEntry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.ArchivedFiles == null)
                return NotFound();

            var archivedfile =  await _context.ArchivedFiles.FirstOrDefaultAsync(m => m.Id == id);
            if (archivedfile == null)
            {
                return NotFound();
            }
            FileEntry = archivedfile;
           ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FileEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArchivedFileExists(FileEntry.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArchivedFileExists(Guid id)
        {
          return (_context.ArchivedFiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
