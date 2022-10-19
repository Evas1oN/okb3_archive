using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace okb3_archive.Pages.Download
{
    public class IndexModel : PageModel
    {
        private readonly okb3_archive.Data.ApplicationContext _context;

        public IndexModel(okb3_archive.Data.ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(Guid id, bool zipped = true, bool base64 = false)
        {
            var downloadFile = await _context.ArchivedFiles.Include(x => x.ZippedFile).AsNoTracking().FirstAsync(x => x.Id == id);

            if (base64)
                return Content(Convert.ToBase64String(downloadFile.ZippedFile!.File));
            
            if (zipped) 
                return File(downloadFile.ZippedFile!.File, "applications/gzip", downloadFile.FileName);
            
            downloadFile.ZippedFile!.File = await DecompressGzip(downloadFile.ZippedFile.File);
            downloadFile.FileName = downloadFile.FileName!.Replace(".gz", "");

            return File(downloadFile.ZippedFile.File, "applications/gzip", downloadFile.FileName);
        }
        
        public async Task<byte[]> DecompressGzip(byte[] compressedFile)
        {
            await using (var decompress = new GZipStream(new MemoryStream(compressedFile), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = decompress.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    } while (count > 0);

                    return memory.ToArray();
                }

            }
        }
    }
}
