using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using okb3_archive.Data;
using okb3_archive.Data.Models;

namespace okb3_archive.Pages.ArchiveEntry
{
    public class CreateModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public CreateModel(okb3_archive.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public FileEntry FileEntry { get; set; } = default!;
        
        [BindProperty]
        public IFormFile Document { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["DocumentTypes"] = new SelectList(await _context.DocumentTypes.ToListAsync(), "Id", "Name");
            return Page();
        }


        
        public async Task<IActionResult> OnPostAsync()
        {
            var allowedFiles = _configuration.GetSection("AllowedFileTypes").Get<List<string>>();
            var maxFileSize = Convert.ToInt32(_configuration.GetSection("MaxFileSize").Get<int>());

            if (!allowedFiles.Contains(Document.ContentType))
                ModelState.AddModelError("FileEntry.ZippedFile", "Недопустимый размер файла");

            if (Document.Length <= 0 || Document.Length >= maxFileSize)
                ModelState.AddModelError("FileEntry.ZippedFile", "Недопустимый размер файла");
            
            if (!ModelState.IsValid)
                return await StandardPage();

            FileEntry.ZippedFile = new ZippedFile
            {
                File = await CompressFile(Document)
            };
            FileEntry.FileName = Document.FileName + ".gz";
            FileEntry.MD5Hash = CalculateMd5Hash(FileEntry.ZippedFile.File);

            if (_context.ArchivedFiles.Any(x => x.MD5Hash == FileEntry.MD5Hash))
            {
                ModelState.AddModelError("FileEntry.ZippedFile", "Такой файл уже существует");
                return await StandardPage();
            }

            _context.ArchivedFiles.Add(FileEntry);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }

        public async Task<byte[]> CompressFile(IFormFile original)
        {
            using var ms = new MemoryStream();
            await original.CopyToAsync(ms);
            return await CompressFile(ms.ToArray());
        }
        
        public async Task<byte[]> CompressFile(byte[] original)
        {
            using var compress = new MemoryStream();
            await using (var gzip = new GZipStream(compress, CompressionMode.Compress))
                gzip.Write(original,0,original.Length);
            
            return compress.ToArray();
        }

        public string CalculateMd5Hash(byte[] file)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(file);
            return BitConverter.ToString(hash).Replace("-","");
        }

        public async Task<IActionResult> StandardPage()
        {
            ViewData["DocumentTypes"] = new SelectList(await _context.DocumentTypes.ToListAsync(), "Id", "Name");
            return Page();
        }
    }
}
