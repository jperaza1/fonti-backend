using ImageMagick;
using iTextSharp.text;
using iTextSharp.text.pdf;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace cod.infraestructure.Implementations
{
    public class PdfCreatorService : IPdfCreatorService
    {
        private Document PdfDocument;
        private readonly IConverter _converter;
        public PdfCreatorService(IConverter converter)
        {
            this.PdfDocument = new Document(PageSize.A4, 10, 10, 30, 1);
            this._converter = converter;
        }

        public void New()
        {
            this.PdfDocument = new Document();
        }
       
        public Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new System.Drawing.Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);

            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }

        public byte[] MergePdf(List<byte[]> pdfs)
        {

            byte[] mergedPdf = null;

            using (MemoryStream ms = new MemoryStream())
            {

                PdfDocument = new Document();

                PdfSmartCopy copy = new PdfSmartCopy(PdfDocument, ms);

                PdfDocument.Open();

                pdfs.ForEach(pdf =>
                {

                    PdfReader reader = new PdfReader(pdf);

                    int n = reader.NumberOfPages;

                    for (int page = 0; page < n;)
                    {

                        copy.AddPage(copy.GetImportedPage(reader, ++page));

                    }

                });

                copy.Close();

                mergedPdf = ms.ToArray();

            }

            return mergedPdf;

        }

        public byte[] GetPdf(string htmlContent, string name, Orientation orientation)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = orientation,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 2 },
                DocumentTitle = name
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };
            var htmlDocument = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return _converter.Convert(htmlDocument);
        }

        public string GetUrlPdf(string htmlContent, string name, Orientation orientation)
        {
            byte[] file = GetPdf(htmlContent, name, orientation);
            return Convert.ToBase64String(file);
        }
    }
}
