using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.DocumentTypes
{
    public class CreateModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;

        public CreateModel(okb3_archive.Data.ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DocumentType DocumentType { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.DocumentTypes == null || DocumentType == null)
            {
                return Page();
            }

            _context.DocumentTypes.Add(DocumentType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
