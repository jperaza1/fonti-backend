using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace cod.application.Interfaces
{
    public interface IPdfCreatorService
    {
        byte[] MergePdf(List<byte[]> pdfs);

        byte[] GetPdf(string htmlContent, string name, Orientation orientation);

        string GetUrlPdf(string htmlContent, string name, Orientation orientation);
    }
}
