using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System.IO.Compression;
using System.IO;

namespace PdfActionApi.Services
{
    public class PdfActionsServices
    {
        public static MemoryStream GetSplitFiles(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var inputDoc = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            using var zipStream = new MemoryStream();

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                for (int i = 0; i < inputDoc.PageCount; i++)
                {
                    var outputdoc = new PdfDocument();
                    outputdoc.AddPage(inputDoc.Pages[i]);
                    using var pageStream = new MemoryStream();

                    outputdoc.Save(pageStream, false);

                    pageStream.Seek(0, SeekOrigin.Begin);

                    var zipEntry = archive.CreateEntry($"page-{i + 1}.pdf", CompressionLevel.Fastest);

                    using var entrystreem = zipEntry.Open();

                    pageStream.CopyTo(entrystreem);

                }

            }
            zipStream.Seek(0, SeekOrigin.Begin);

            return zipStream;
        }
        public static MemoryStream GetMargeFiles(List<IFormFile> files)
        {


            PdfDocument margeDoc = new PdfDocument();

            foreach (var file in files)
            {

                var stream = file.OpenReadStream();
                using var inputDoc = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                foreach(var page in inputDoc.Pages)
                {
                    margeDoc.Pages.Add(page);
                }

                
            }

            using var pageStream = new MemoryStream();

            margeDoc.Save(pageStream, false);

            return pageStream;
        }
        public static MemoryStream GetSplityRange(IFormFile file, int startIndex, int endIndex)
        {

                  var filetream = file.OpenReadStream();
           
                PdfDocument mydoc = PdfReader.Open(filetream, PdfDocumentOpenMode.Import);

                PdfPages mypages = mydoc.Pages;

                 var memorystrem = new MemoryStream();

                PdfDocument newdoc = new PdfDocument();

                for (int i = startIndex; i  <= endIndex; i++)
                {
                    newdoc.Pages.Add(mypages[i - 1]);  //כיון שמערך מתחיל מ 0

                }
                newdoc.Save(memorystrem, false);

                memorystrem.Seek(0, SeekOrigin.Begin);

                    return memorystrem;
            


        }
    }
   
}
