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

namespace okb3_archive.Pages.DocumentTypes
{
    public class EditModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public EditModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public DocumentType DocumentType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.DocumentTypes == null)
            {
                return NotFound();
            }

            var documenttype =  await _context.DocumentTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (documenttype == null)
            {
                return NotFound();
            }
            DocumentType = documenttype;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.IsInRole(_configuration.GetSection("DocumentTypeMaintainerRole").Get<string>()))
            {
                TempData["Error"] = "” вас нет прав дл€ совершени€ данного действи€";
                return RedirectToPage("./Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DocumentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentTypeExists(DocumentType.Id))
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

        private bool DocumentTypeExists(Guid id)
        {
          return (_context.DocumentTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
