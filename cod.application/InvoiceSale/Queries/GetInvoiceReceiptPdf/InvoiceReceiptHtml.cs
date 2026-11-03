using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace cod.application.InvoiceSale.Queries.GetInvoiceReceiptPdf
{
    public class InvoiceReceiptHtml
    {
        private static readonly CultureInfo Dates = CultureInfo.GetCultureInfo("es-HN");
        private static readonly CultureInfo Numbers = CultureInfo.InvariantCulture;

        public static string GetInvoiceReceiptHtml(InvoiceReceiptModel model)
        {
            model ??= new InvoiceReceiptModel();
            model.Lines ??= new List<InvoiceReceiptLineModel>();

            var lines = new StringBuilder();
            if (model.Lines.Any())
            {
                foreach (var line in model.Lines)
                {
                    var taxMark = line.Tax ? "G" : "E";
                    lines.Append($@"
                        <tr>
                            <td class=""qty"">{FormatQty(line.Quantity)}</td>
                            <td>{H(line.Description)} ({taxMark})</td>
                            <td class=""num"">{Money(line.UnitPrice)}</td>
                            <td class=""num"">{Money(line.Amount)}</td>
                        </tr>");
                }
            }
            else
            {
                lines.Append(@"
                        <tr>
                            <td colspan=""4"" class=""empty"">Sin detalle</td>
                        </tr>");
            }

            var dueDateRow = model.DueDate.HasValue
                ? $@"<tr>
                        <td class=""lbl"">Vence</td>
                        <td>{FormatDate(model.DueDate.Value)}</td>
                    </tr>"
                : string.Empty;

            var advisor = string.IsNullOrWhiteSpace(model.Advisor)
                ? string.Empty
                : $@"<div class=""meta-line"">Asesor {H(model.Advisor)}</div>";

            var fiscalNotes = $@"
                    {advisor}
                    <div class=""meta-line"">No Constancia Exonerada {H(model.ExemptCertificateNumber)}</div>
                    <div class=""meta-line"">No orden Exenta {H(model.ExemptOrderNumber)}</div>
                    <div class=""meta-line"">No Registro SAG {H(model.SagRegistryNumber)}</div>";

            var logo = string.IsNullOrWhiteSpace(model.Logo)
                ? string.Empty
                : $@"<td class=""header-logo""><img src=""{Src(model.Logo)}"" alt="""" /></td>";

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Factura {H(model.InvoiceNumber)}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
        }}

        body {{
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #111;
            line-height: 1.55;
            padding: 18px 22px 90mm 22px;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
        }}

        .page-footer {{
            position: fixed;
            left: 0;
            bottom: 0;
            width: 100%;
            padding: 0 22px 6px 22px;
            box-sizing: border-box;
        }}

        .company-name {{
            font-size: 16px;
            font-weight: 700;
            text-transform: uppercase;
            line-height: 1.4;
        }}

        .muted {{
            color: #444;
            font-size: 11px;
            line-height: 1.5;
        }}

        .header td {{
            vertical-align: middle;
        }}

        .header-logo {{
            width: 120px;
            vertical-align: middle;
            padding-right: 14px;
        }}

        .header-logo img {{
            width: 110px;
            height: auto;
            max-height: 110px;
        }}

        .header-left {{
            width: 62%;
            padding-right: 12px;
            vertical-align: middle;
        }}

        .header-right {{
            width: 38%;
            text-align: right;
            vertical-align: middle;
        }}

        .doc-title {{
            font-size: 16px;
            font-weight: 700;
            text-transform: uppercase;
            line-height: 1.4;
        }}

        .doc-type {{
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            line-height: 1.5;
        }}

        .doc-number {{
            font-size: 11px;
            font-weight: 700;
            line-height: 1.5;
        }}

        .rule {{
            border: none;
            border-top: 1px solid #111;
            margin: 10px 0;
        }}

        .rule-light {{
            border: none;
            border-top: 1px solid #bbb;
            margin: 8px 0;
        }}

        .meta td,
        .party td {{
            padding: 3px 0;
            vertical-align: top;
        }}

        .lbl {{
            width: 180px;
            color: #555;
            font-size: 11px;
            text-transform: uppercase;
            white-space: nowrap;
            padding-right: 12px;
        }}

        .items th {{
            text-align: left;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            color: #555;
            border-bottom: 1px solid #111;
            padding: 6px 8px;
        }}

        .items td {{
            padding: 7px 8px;
            border-bottom: 1px solid #e5e5e5;
            vertical-align: top;
            line-height: 1.5;
        }}

        .items .qty {{
            width: 56px;
            text-align: right;
        }}

        .items .num {{
            width: 90px;
            text-align: right;
            white-space: nowrap;
        }}

        .items th.qty,
        .items th.num {{
            text-align: right;
        }}

        .empty {{
            text-align: center;
            color: #777;
            padding: 14px 6px;
        }}

        .summary {{
            margin-top: 10px;
        }}

        .summary-left {{
            width: 58%;
            vertical-align: top;
            padding-right: 16px;
        }}

        .summary-right {{
            width: 42%;
            vertical-align: top;
        }}

        .words {{
            font-size: 11px;
            color: #222;
            line-height: 1.5;
        }}

        .words strong {{
            display: block;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            color: #555;
            margin-bottom: 4px;
        }}

        .totals td {{
            padding: 3px 0;
        }}

        .totals .t-lbl {{
            color: #555;
            font-size: 11px;
        }}

        .totals .t-val {{
            text-align: right;
            white-space: nowrap;
        }}

        .totals .total-row td {{
            border-top: 1px solid #111;
            padding-top: 6px;
            font-weight: 700;
            font-size: 12px;
        }}

        .legend {{
            margin-top: 0;
            font-size: 11px;
            color: #555;
        }}

        .motto {{
            margin-top: 14px;
            text-align: center;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            line-height: 1.5;
        }}

        .copies {{
            margin-top: 8px;
            text-align: center;
            font-size: 10px;
            color: #444;
            text-transform: uppercase;
            line-height: 1.5;
        }}

        .meta-line {{
            margin-top: 6px;
            font-size: 11px;
            color: #555;
            line-height: 1.5;
        }}
    </style>
</head>
<body>
    <div class=""page-main"">
        <table class=""header"">
            <tr>
                {logo}
                <td class=""header-left"">
                    <div class=""company-name"">{H(model.CompanyName)}</div>
                    <div class=""muted"">{H(model.Address)}</div>
                    <div class=""muted"">{JoinContact(model.PhoneNumber, model.Email)}</div>
                    <div class=""muted"">RTN {H(model.CompanyRtn)}</div>
                </td>
                <td class=""header-right"">
                    <div class=""doc-title"">FACTURA</div>
                    <div class=""doc-type"">{H(model.PaymentTypeName)}</div>
                    <div class=""doc-number"">{H(model.InvoiceNumber)}</div>
                    <div class=""muted"">Fecha Factura {FormatDate(model.InvoiceDate)}</div>
                </td>
            </tr>
        </table>

        <hr class=""rule"" />

        <table class=""meta"">
            <tr>
                <td class=""lbl"">CAI</td>
                <td>{H(model.Cai)}</td>
            </tr>
            <tr>
                <td class=""lbl"">Fecha Límite de Emisión</td>
                <td>{FormatDate(model.EmissionLimitDate)}</td>
            </tr>
            <tr>
                <td class=""lbl"">Rango autorizado</td>
                <td>{H(model.CorrelativeFrom)} &ndash; {H(model.CorrelativeTo)}</td>
            </tr>
        </table>

        <hr class=""rule-light"" />

        <table class=""party"">
            <tr>
                <td class=""lbl"">Cliente</td>
                <td>{H(model.CustomerName)}</td>
            </tr>
            <tr>
                <td class=""lbl"">RTN</td>
                <td>{H(model.CustomerRtn)}</td>
            </tr>
            {dueDateRow}
        </table>

        <hr class=""rule"" />

        <table class=""items"">
            <thead>
                <tr>
                    <th class=""qty"">Cant.</th>
                    <th>Descripción</th>
                    <th class=""num"">P. Unit.</th>
                    <th class=""num"">Importe</th>
                </tr>
            </thead>
            <tbody>
                {lines}
            </tbody>
        </table>
    </div>
    <div class=""page-footer"">
        <div class=""legend"">(G) Gravado &nbsp;&nbsp; (E) Exento</div>

        <table class=""summary"">
            <tr>
                <td class=""summary-left"">
                    <div class=""words"">
                        <strong>Son</strong>
                        {H(ToLempiras(model.Total))}
                    </div>
                    {fiscalNotes}
                </td>
                <td class=""summary-right"">
                    <table class=""totals"">
                        <tr>
                            <td class=""t-lbl"">Gravado 15%</td>
                            <td class=""t-val"">L. {Money(model.TaxableAmount)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">Gravado 18%</td>
                            <td class=""t-val"">L. {Money(model.TaxableAmount18)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">Exento</td>
                            <td class=""t-val"">L. {Money(model.ExemptAmount)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">Exonerado</td>
                            <td class=""t-val"">L. {Money(model.ExoneratedAmount)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">Sub-Total</td>
                            <td class=""t-val"">L. {Money(model.SubTotal)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">Descuento</td>
                            <td class=""t-val"">L. {Money(model.Discount)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">ISV 15%</td>
                            <td class=""t-val"">L. {Money(model.TaxAmount)}</td>
                        </tr>
                        <tr>
                            <td class=""t-lbl"">ISV 18%</td>
                            <td class=""t-val"">L. {Money(model.Tax18Amount)}</td>
                        </tr>
                        <tr class=""total-row"">
                            <td>Total a pagar</td>
                            <td class=""t-val"">L. {Money(model.Total)}</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <div class=""motto"">LA FACTURA ES BENEFICIO DE TODOS &quot;EXIJALA&quot;</div>
        <div class=""copies"">ORIGINAL CLIENTE AUTO EMISOR ORIGINAL-CLIENTE COPIA-OBLIGADO TRIBUTARIO EMISOR</div>
    </div>
</body>
</html>";
        }

        private static string JoinContact(string phone, string email)
        {
            var parts = new[] { phone, email }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(H);
            return string.Join("  ·  ", parts);
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy", Dates);
        }

        private static string FormatQty(double quantity)
        {
            return quantity.ToString("0.###", Numbers);
        }

        private static string Money(double amount)
        {
            return amount.ToString("#,0.00", Numbers);
        }

        private static string H(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        private static string Src(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("\"", string.Empty);
        }

        private static string ToLempiras(double amount)
        {
            var rounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            var integer = (long)Math.Floor(Math.Abs(rounded));
            var cents = (int)Math.Round((Math.Abs(rounded) - integer) * 100, MidpointRounding.AwayFromZero);

            if (cents == 100)
            {
                integer++;
                cents = 0;
            }

            var prefix = rounded < 0 ? "MENOS " : string.Empty;
            return $"{prefix}{ToSpanish(integer)} LEMPIRAS CON {cents:00}/100";
        }

        private static string ToSpanish(long number)
        {
            if (number == 0)
                return "CERO";

            if (number == 1)
                return "UN";

            return ConvertGroup(number).Trim();
        }

        private static string ConvertGroup(long number)
        {
            if (number >= 1000000000000)
            {
                var unit = number / 1000000000000;
                var rest = number % 1000000000000;
                var label = unit == 1 ? "UN BILLON" : $"{ConvertGroup(unit)} BILLONES";
                return JoinParts(label, rest);
            }

            if (number >= 1000000)
            {
                var unit = number / 1000000;
                var rest = number % 1000000;
                var label = unit == 1 ? "UN MILLON" : $"{ConvertGroup(unit)} MILLONES";
                return JoinParts(label, rest);
            }

            if (number >= 1000)
            {
                var unit = number / 1000;
                var rest = number % 1000;
                var label = unit == 1 ? "MIL" : $"{ConvertGroup(unit)} MIL";
                return JoinParts(label, rest);
            }

            if (number >= 100)
            {
                var unit = (int)(number / 100);
                var rest = number % 100;
                string[] hundreds = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };
                if (number == 100)
                    return "CIEN";
                return JoinParts(hundreds[unit], rest);
            }

            if (number >= 30)
            {
                var tens = (int)(number / 10);
                var rest = number % 10;
                string[] tenNames = { "", "", "", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
                return rest == 0 ? tenNames[tens] : $"{tenNames[tens]} Y {ConvertGroup(rest)}";
            }

            string[] underThirty =
            {
                "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE",
                "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISEIS", "DIECISIETE",
                "DIECIOCHO", "DIECINUEVE", "VEINTE", "VEINTIUNO", "VEINTIDOS", "VEINTITRES",
                "VEINTICUATRO", "VEINTICINCO", "VEINTISEIS", "VEINTISIETE", "VEINTIOCHO", "VEINTINUEVE"
            };

            return underThirty[number];
        }

        private static string JoinParts(string left, long rest)
        {
            return rest == 0 ? left : $"{left} {ConvertGroup(rest)}";
        }
    }
}
