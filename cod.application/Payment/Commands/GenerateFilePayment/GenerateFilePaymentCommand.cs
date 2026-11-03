using cod.application.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Main;
using cod.domain.Features;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Payment.Commands.GenerateFilePayment
{
    public class GenerateFilePaymentCommand : BaseCommand, IGenerateFilePaymentCommand
    {
        private readonly IBankFileService _bankFileService;
        private readonly IConfigurationService _configurationService;
        public GenerateFilePaymentCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            IBankFileService bankFileService,
            IConfigurationService configurationService) : base(repository, exceptionFactory)
        {
            _bankFileService = bankFileService;
            _configurationService = configurationService;
        }

        public async Task<GenerateFilePaymentModel> Execute(int paymentId)
        {
            var payment = await _repository.AllAsync<domain.Finance.Payment>()
                .Where(f => f.Id == paymentId).FirstOrDefaultAsync();

            if (payment != null)
            {
                payment.PaymentStatus = domain.Features.Enums.PaymentStatus.Processing;

                if (payment.BankAccount.Bank.Name == "BANCO ATLANTIDA")
                {
                    string templatePath = Path.Combine(
                        "C:\\Templates\\FORMATO_PLANILAS_SAP.xlsm"
                    );

                    using var package = new ExcelPackage(new FileInfo(templatePath));

                    var worksheet = package.Workbook.Worksheets["Hoja1"];
                    const int filaInicial = 2;
                    int filaActual = filaInicial;

                    var details = payment.Details.GroupBy(f => f.Invoice.Expense.VendorId).ToList();
                    var detailsFiles = new List<ProviderPaymentDetailAtlantida>();
                    foreach (var item in details)
                    {
                        var vendorGrouped = item.First().Invoice.Expense.Vendors;

                        detailsFiles.Add(new ProviderPaymentDetailAtlantida
                        {
                            BankAccountHolder = vendorGrouped.BankAccountHolder,
                            IdentificationHolder = vendorGrouped.IdentificationHolder,
                            DestinationBranch = vendorGrouped.C1,
                            DestinationOffice = vendorGrouped.C2,
                            Description = string.Join(", ", item.Select(g => g.Description).ToArray()),
                            Amount = (decimal)item.Sum(g => g.Amount),
                            Email = vendorGrouped.Email,
                            DestinationAccount = vendorGrouped.BankAccount,
                            AccountTypeText = vendorGrouped.TypeAccount.Name,
                            BankAlias = vendorGrouped.Bank.Alias,
                        });
                    }

                    foreach (var item in detailsFiles)
                    {
                        worksheet.Cells[filaActual, 1].Value = (filaActual - 1);
                        worksheet.Cells[filaActual, 2].Value = item.BankAccountHolder;
                        worksheet.Cells[filaActual, 3].Value = item.IdentificationHolder;
                        worksheet.Cells[filaActual, 4].Value = item.Amount;
                        worksheet.Cells[filaActual, 5].Value = item.BankAlias == "BANCO FICOHSA" ? $"{item.DestinationBranch}{item.DestinationOffice}{item.DestinationAccount}" : item.DestinationAccount;
                        worksheet.Cells[filaActual, 6].Value = item.BankAlias;
                        worksheet.Cells[filaActual, 7].Value = item.AccountTypeText == "CC" ? "Cheques" : "Ahorros";
                        worksheet.Cells[filaActual, 8].Value = "CORTES";
                        worksheet.Cells[filaActual, 9].Value = "SAN PEDRO SULA";
                        worksheet.Cells[filaActual, 10].Value = "BARRIO RIO DE PIEDRA";
                        worksheet.Cells[filaActual, 12].Value = item.Email;
                        worksheet.Cells[filaActual, 13].Value = item.Description;

                        
                        worksheet.Cells[filaActual, 4].Style.Numberformat.Format = "#,##0.00";

                        filaActual++;
                    }

                    _repository.UpdateSync(payment);

                    await _repository.CommitAsync();

                    using var outputStream = new MemoryStream();
                    package.SaveAs(outputStream);

                    return new GenerateFilePaymentModel
                    {
                        File = outputStream.ToArray(),
                        FileName = $"Proveedores {payment.Id}.xlsm"
                    };
                }

                if (payment.BankAccount.Bank.Name == "BANCO FICOHSA")
                {
                    var batch_correlative = await _configurationService.Get<int>("BATCH_CORRELATIVE");
                    batch_correlative += 1;

                    var details = payment.Details.GroupBy(f => f.Invoice.Expense.VendorId).ToList();

                    var detailsFiles = new List<ProviderPaymentDetail>();

                    foreach (var item in details)
                    {
                        var vendorGrouped = item.First().Invoice.Expense.Vendors;

                        detailsFiles.Add(new ProviderPaymentDetail
                        {
                            DestinationBankCode = vendorGrouped.Bank.Code,
                            DestinationAccountTypeCode = "01",
                            DestinationBranch = vendorGrouped.C1,
                            DestinationOffice = vendorGrouped.C2,
                            DestinationAccount = vendorGrouped.BankAccount,
                            AccountTypeText = vendorGrouped.TypeAccount.Name,
                            BeneficiaryName = vendorGrouped.BankAccountHolder,
                            Description = string.Join(", ", item.Select(g => g.Description).ToArray()),
                            Amount = (decimal)item.Sum(g => g.Amount),
                            Email = vendorGrouped.Email,
                        });
                    }

                    var fileModel = new ProviderPaymentFileRequest
                    {
                        FileNameWithoutExtension = $"Proveedores {batch_correlative}.txt",
                        ProfileCode = "02998076",
                        GeneratedAt = Clock.Now,
                        BatchNumber = $"{batch_correlative}",
                        SourceBankCode = "028",
                        SourceAccountType = "01",
                        SourceBranch = "000",
                        SourceOffice = "000",
                        SourceAccount = "200010320177",
                        Details = detailsFiles
                    };

                    var file = _bankFileService.GenerateTxt(fileModel);

                    await _repository.UpdateAsync<domain.General.Configuration>(
                        g => g.Key == "BATCH_CORRELATIVE",
                        g => new domain.General.Configuration
                        {
                            Value = $"{batch_correlative}"
                        });

                    _repository.UpdateSync(payment);

                    await _repository.CommitAsync();

                    return new GenerateFilePaymentModel
                    {
                        File = file,
                        FileName = fileModel.FileNameWithoutExtension
                    };
                }
            }

            throw new ApplicationException("CANCELED", "PAGO NO ENCONTRADO");
        }
    }
}
