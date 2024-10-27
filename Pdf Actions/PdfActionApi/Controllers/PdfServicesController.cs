using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfActionApi.Services;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using System.IO.Compression;

namespace PdfActionApi.Controllers
{
    [Route("api/pdfactions")]
    [ApiController]
    public class PdfServicesController : ControllerBase
    {
        [HttpPost("split")]
        public IActionResult SpllitPdf(IFormFile file)
        {
            if(file == null || file.Length ==0)
            {
                return BadRequest("please insert valid file");
            }
            try
            {
                using var zipStream = PdfActionsServices.GetSplitFiles(file);

                return File(zipStream.ToArray(), "application/zip", "pdf_pages.zip");
            }
            catch (Exception ex) 
            { 
                     return BadRequest(ex.Message);
            }
        }

        [HttpPost("merge")]
        public IActionResult MergePdf(List<IFormFile> files)
        {
            using var pdfstream = PdfActionsServices.GetMargeFiles(files);

            return File(pdfstream.ToArray(), "application/pdf", "myfile.pdf");
        }

        [HttpPost("splitrange")]
         public IActionResult SplitRange(IFormFile file, int start,int end)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("please insert valid file");
            }

            var stremsplit = PdfActionsServices.GetSplityRange(file, start, end);

            return File(stremsplit.ToArray(), "application/pdf", "newpdf.pdf");
        }

    }
    
}
