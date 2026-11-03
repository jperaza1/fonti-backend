using cod.application.Common;
using cod.application.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.infraestructure.Services
{
    public class BankFileService : IBankFileService
    {
        public BankFileService()
        {

        }
        public byte[] GenerateTxt(ProviderPaymentFileRequest request)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var ansiEncoding = Encoding.GetEncoding(1252); // Windows-1252 / ANSI

            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms, ansiEncoding, bufferSize: 1024, leaveOpen: true);

            writer.WriteLine("TRANSBNKEL");
            writer.WriteLine((request.ProfileCode ?? "").PadLeft(8, '0'));
            writer.WriteLine(request.GeneratedAt.ToString("yyyy/MM/dd HH:mm:ss"));
            writer.WriteLine();
            writer.WriteLine($"{request.Details.Count}:{request.Details.Sum(x => x.Amount).ToString("0.00", CultureInfo.InvariantCulture)}");
            writer.WriteLine(request.BatchNumber ?? "");
            writer.WriteLine("<Inicio>");

            int sequence = 1;

            foreach (var item in request.Details)
            {
                var line = BuildDetailLine(sequence, request, item);
                writer.WriteLine(line);
                sequence++;
            }

            writer.WriteLine("<Fin>");

            writer.Flush();

            return ms.ToArray();
        }

        private static string BuildDetailLine(int sequence, ProviderPaymentFileRequest header, ProviderPaymentDetail item)
        {
            var emailBlock = BuildEmailBlock(item.Email, item.Description);

            return string.Concat(
                PadLeftZeros(sequence.ToString(), 6),              // A
                "001",                                             // B
                "1",                                               // C
                PadRightSpaces("", 10),                            // D
                PadLeftZeros(header.SourceBankCode, 3),            // E
                PadLeftZeros(header.SourceAccountType, 2),         // F
                PadLeftZeros(header.SourceBranch, 3),              // G
                PadLeftZeros(header.SourceOffice, 3),              // H
                PadLeftZeros(header.SourceAccount, 12),            // I
                PadLeftZeros(item.DestinationBankCode, 3),         // J
                PadLeftZeros(item.DestinationAccountTypeCode, 2),  // K
                item.DestinationBankCode == "028" ? PadLeftZeros(item.DestinationBranch, 3) : PadLeftSpace(item.DestinationBranch, 3),           // L
                item.DestinationBankCode == "028" ? PadLeftZeros(item.DestinationOffice, 3) : PadLeftSpace(item.DestinationOffice, 3),           // M
                item.DestinationBankCode == "028" ? PadLeftZeros(item.DestinationAccount, 12) : PadLeftSpace(item.DestinationAccount, 12),         // N
                PadRightSpaces(item.AccountTypeText, 2),           // O
                PadRightSpaces("", 15),                            // P
                PadRightSpaces((item.BeneficiaryName ?? "").ToUpperInvariant(), 40), // Q
                PadRightSpaces((item.Description ?? "").ToUpperInvariant(), 90),     // R
                PadRightZeros("0", 3),                             // S
                PadLeftZeros(FormatAmount(item.Amount), 14),       // T
                PadRightZeros("0", 3),                             // U
                PadRightSpaces(emailBlock, 400)                    // V
            );
        }

        private static string BuildEmailBlock(string? email, string? description)
        {
            email = FixEmail(email ?? "");
            description = (description ?? "").ToUpperInvariant();

            return $"#E#{email}#e#T#{description}#t#";
        }

        private static string FixEmail(string email)
        {
            return email
                .Replace(",co", ".co", StringComparison.OrdinalIgnoreCase)
                .Replace(",es", ".es", StringComparison.OrdinalIgnoreCase)
                .Replace(",org", ".org", StringComparison.OrdinalIgnoreCase)
                .Replace(",hn", ".hn", StringComparison.OrdinalIgnoreCase)
                .Replace(",com", ".com", StringComparison.OrdinalIgnoreCase)
                .Replace(",net", ".net", StringComparison.OrdinalIgnoreCase)
                .Trim();
        }

        private static string FormatAmount(decimal amount)
        {
            // 48391.00 => 00000000048391.00 cuando se complete a 14
            return amount.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string PadLeftZeros(string? value, int length)
        {
            value ??= "";
            if (value.Length > length)
                value = value[..length];

            return value.PadLeft(length, '0');
        }

        private static string PadLeftSpace(string? value, int length)
        {
            value ??= "";
            if (value.Length > length)
                value = value[..length];

            return value.PadLeft(length, ' ');
        }

        private static string PadRightZeros(string? value, int length)
        {
            value ??= "";
            if (value.Length > length)
                value = value[..length];

            return value.PadRight(length, '0');
        }

        private static string PadRightSpaces(string? value, int length)
        {
            value ??= "";
            if (value.Length > length)
                value = value[..length];

            return value.PadRight(length, ' ');
        }

        private static string SanitizeFileName(string value)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                value = value.Replace(c, '_');

            return value.Trim();
        }
    }
}
