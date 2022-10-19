using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.ArchiveEntry
{
    public class IndexModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IList<FileEntry> ArchiveFile { get;set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public Guid? DocumentType { get; set; }

        [BindProperty(SupportsGet = true)] 
        public int CurrentPage { get; set; } = 1;
        
        [BindProperty(SupportsGet = true)]
        public int TotalPages { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? From { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? To { get; set; }

        private int _pageSize;

        public async Task OnGetAsync()
        {
            var archivedFiles = _context.ArchivedFiles.Include(x => x.DocumentType).AsQueryable();

            ViewData["DocumentTypes"] = new SelectList(archivedFiles.Select(x => x.DocumentType).Distinct(), "Id", "Name");

            if (DocumentType.HasValue)
                archivedFiles = archivedFiles.Where(x => x.DocumentTypeId.Equals(DocumentType.Value));

            if (!string.IsNullOrWhiteSpace(Search))
                archivedFiles = archivedFiles.Where(x => x.Name.ToUpper().Contains(Search.ToUpper()) || x.FileName!.ToUpper().Contains(Search.ToUpper()));
            
            if (From.HasValue)
                archivedFiles = archivedFiles.Where(x => x.CreatedOn >= From.Value);
            
            if (To.HasValue)
                archivedFiles = archivedFiles.Where(x => x.CreatedOn <= To.Value);
            
            _pageSize = _configuration.GetSection("PageSize").Get<int>();
            TotalPages = (int)Math.Ceiling(await archivedFiles.CountAsync() / (double)_pageSize);
            
            ArchiveFile = await archivedFiles.Skip((CurrentPage - 1) * _pageSize).Take(_pageSize).ToListAsync();
        }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

    }
}
