using cod.application.Features.Base;
using cod.application.Interfaces;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf
{
    public class GetPurchaseOrderPdfQuery : BaseQuery, IGetPurchaseOrderPdfQuery
    {
        private readonly IPdfCreatorService _pdfCreatorService;

        public GetPurchaseOrderPdfQuery(IRepository repository, IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int id, bool hideCost)
        {
            var order = await _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(item => item.Id == id)
                .Select(item => new
                {
                    item.Reference,
                    item.Date,
                    Vendor = item.Vendor.Name,
                    item.TypeInoice,
                    Lines = item.PurchaseOrderDetails.Select(line => new
                    {
                        Product = line.ProductVendorId.HasValue ? line.ProductVendor.Product.Name : line.Product.Name,
                        line.Quantity,
                        line.Cost,
                        line.Total
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
                throw new ApplicationException("Orden no existe", "ORDEN COMPRA");

            var rows = new StringBuilder();
            foreach (var line in order.Lines)
            {
                rows.Append("<tr><td>")
                    .Append(line.Product)
                    .Append("</td><td style=\"text-align:right\">")
                    .Append(line.Quantity.ToString("0.##"));
                if (!hideCost)
                {
                    rows.Append("</td><td style=\"text-align:right\">")
                        .Append(line.Cost.ToString("0.00"))
                        .Append("</td><td style=\"text-align:right\">")
                        .Append(line.Total.ToString("0.00"));
                }
                rows.Append("</td></tr>");
            }

            var costHeaders = hideCost ? "" : "<th>Costo</th><th>Total</th>";
            var html = "<html><body style=\"font-family:Arial\"><h2>Orden de compra " + order.Reference
                + "</h2><p>Proveedor: " + order.Vendor + "<br>Fecha: " + order.Date.ToString("dd/MM/yyyy")
                + "<br>Tipo: " + order.TypeInoice + "</p><table border=\"1\" cellpadding=\"6\" cellspacing=\"0\"><tr><th>Producto</th><th>Cantidad</th>"
                + costHeaders + "</tr>" + rows + "</table></body></html>";

            var pdf = _pdfCreatorService.GetPdf(html, order.Reference, Orientation.Portrait);
            return Convert.ToBase64String(pdf);
        }
    }
}
