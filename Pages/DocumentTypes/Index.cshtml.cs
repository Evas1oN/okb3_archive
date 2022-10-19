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
        private readonly IConfiguration _configuration;
        public bool IsDocumentTypeMaintainer = false;

        public IndexModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IList<DocumentType> DocumentType { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (User.IsInRole(_configuration.GetSection("DocumentTypeMaintainerRole").Get<string>()))
                IsDocumentTypeMaintainer = true;

            DocumentType = await _context.DocumentTypes.ToListAsync();
        }
    }
}
