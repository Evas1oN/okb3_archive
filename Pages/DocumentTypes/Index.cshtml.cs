using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.DocumentTypes
{
    public class IndexModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;

        public IndexModel(okb3_archive.Data.ApplicationContext context)
        {
            _context = context;
        }

        public IList<DocumentType> DocumentType { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DocumentType = await _context.DocumentTypes.ToListAsync();
        }
    }
}
