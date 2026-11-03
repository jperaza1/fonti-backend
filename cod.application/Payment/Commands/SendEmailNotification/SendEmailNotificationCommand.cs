using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.application.Payment.Queries.GetPaymentById;
using cod.domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.SendEmailNotification
{
    public class SendEmailNotificationCommand : BaseCommand, ISendEmailNotificationCommand
    {
        private readonly IGetPaymentByIdQuery _getPaymentByIdQuery;
        private readonly IEmailService _emailService;
        public SendEmailNotificationCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IGetPaymentByIdQuery getPaymentByIdQuery,
            IEmailService emailService) : base(repository, exceptionFactory)
        {
            _getPaymentByIdQuery = getPaymentByIdQuery;
            _emailService = emailService;
        }

        public async Task Execute(int paymentId)
        {
            var payment = await _getPaymentByIdQuery.Execute(paymentId);

            if (payment == null)
                return;

            var emails = ""
                        .Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(e => e.Trim())
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .ToList();

            if (!emails.Any())
                return;

            var body = GeneratePaymentNotificationEmail(payment);

            await _emailService.SendAsync($"Notificacion de Pago a proveedores Pago # {payment.Reference}", body, emails);
        }

        public static string GeneratePaymentNotificationEmail(PaymentModel model)
        {
            var culture = CultureInfo.GetCultureInfo("es-HN");
            var details = model.Details ?? new System.Collections.Generic.List<PaymentDetailModel>();

            var vendors = details
                .GroupBy(d => new { d.VendorId, VendorName = d.VendorName ?? "Sin proveedor" })
                .Select(g => new
                {
                    g.Key.VendorId,
                    g.Key.VendorName,
                    InvoiceCount = g.Count(),
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(v => v.VendorName)
                .ToList();

            var vendorCount = vendors.Count;
            var paymentTotal = vendors.Any() ? vendors.Sum(v => v.Total) : model.Amount;
            var invoiceCount = details.Count;

            var rows = new StringBuilder();

            foreach (var vendor in vendors)
            {
                rows.AppendFormat(
                    @"<tr>
                        <td style=""padding:12px; border:1px solid #ddd;"">
                            {0}
                        </td>
                        <td style=""padding:12px; border:1px solid #ddd; text-align:center;"">
                            {1}
                        </td>
                        <td style=""padding:12px; border:1px solid #ddd; text-align:right; font-weight:bold;"">
                            {2}
                        </td>
                    </tr>",
                    WebUtility.HtmlEncode(vendor.VendorName),
                    vendor.InvoiceCount,
                    vendor.Total.ToString("C", culture)
                );
            }

            var body = new StringBuilder();

            body.AppendFormat(
                @"<!DOCTYPE html>
                <html lang=""es"">
                <body style=""margin:0; padding:24px; font-family:Arial,sans-serif; color:#333;"">
                    <div style=""max-width:700px; margin:auto;"">
                        <h2 style=""color:#166534;"">
                            Notificación de pago a proveedores
                        </h2>

                        <p>Hola:</p>

                        <p>
                            Se ha registrado el pago <strong>{0}</strong> con fecha <strong>{2}</strong>.
                        </p>

                        <table style=""width:100%; border-collapse:collapse; margin:20px 0;"">
                            <tr>
                                <td style=""padding:12px; border:1px solid #ddd; background-color:#f3f4f6; width:50%;"">
                                    Proveedores pagados
                                </td>
                                <td style=""padding:12px; border:1px solid #ddd; font-weight:bold;"">
                                    {3}
                                </td>
                            </tr>
                            <tr>
                                <td style=""padding:12px; border:1px solid #ddd; background-color:#f3f4f6;"">
                                    Facturas incluidas
                                </td>
                                <td style=""padding:12px; border:1px solid #ddd; font-weight:bold;"">
                                    {4}
                                </td>
                            </tr>
                            <tr>
                                <td style=""padding:12px; border:1px solid #ddd; background-color:#f3f4f6;"">
                                    Total del pago
                                </td>
                                <td style=""padding:12px; border:1px solid #ddd; font-weight:bold; color:#166534;"">
                                    {5}
                                </td>
                            </tr>
                        </table>

                        <p>
                            Detalle de proveedores y el total pagado a cada uno:
                        </p>

                        <table style=""width:100%; border-collapse:collapse; margin:20px 0;"">
                            <thead>
                                <tr style=""background-color:#f3f4f6;"">
                                    <th style=""padding:12px; border:1px solid #ddd; text-align:left;"">
                                        Proveedor
                                    </th>
                                    <th style=""padding:12px; border:1px solid #ddd; text-align:center;"">
                                        Facturas
                                    </th>
                                    <th style=""padding:12px; border:1px solid #ddd; text-align:right;"">
                                        Total pagado
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {6}
                            </tbody>
                        </table>
                    </div>
                </body>
                </html>",
                WebUtility.HtmlEncode(model.Reference ?? string.Empty),
                model.PaymentDate.ToString("dd/MM/yyyy", culture),
                vendorCount,
                invoiceCount,
                paymentTotal.ToString("C", culture),
                rows.ToString()
            );

            return body.ToString();
        }
    }
}
