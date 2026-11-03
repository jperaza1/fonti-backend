using cod.application.Correlative.Commands.GenerateCorrelative;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.InvoiceSale.Common;
using cod.domain.Common;
using cod.domain.Features;
using cod.domain.Finance;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.InvoiceSale.Commands.GenerateInvoiceSale
{
    public class GenerateInvoiceSaleCommand : BaseCommand, IGenerateInvoiceSaleCommand
    {
        private readonly IGenerateCorrelativeCommand _generateCorrelativeCommand;
        private readonly AuthService _authService;
        public GenerateInvoiceSaleCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            IGenerateCorrelativeCommand generateCorrelativeCommand,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _generateCorrelativeCommand = generateCorrelativeCommand;
            _authService = authService;
        }

        public async Task Execute(GeneratePendingBillingModel model)
        {
            switch(model.SaleType)
            {
                case domain.Features.Enums.DomainEnums.SaleType.Cash:
                    await GenerateCashInvoice(model, _generateCorrelativeCommand);
                    break;
                case domain.Features.Enums.DomainEnums.SaleType.Credit:
                    await GenerateCreditInvoice(model, _generateCorrelativeCommand);
                    break;
            }
        }

        public async Task GenerateCashInvoice(GeneratePendingBillingModel model, IGenerateCorrelativeCommand _generateCorrelativeCommand)
        {
            var customer = await _repository.Query<domain.Data.Customer>()
                .Where(f => f.Name == "CONSUMIDOR FINAL").FirstOrDefaultAsync();

            if(customer == null)
                throw new ApplicationException("CANCELED", "SE REQUIERE CONFIGURAR CONSUMIDOR FINAL PARA FACTURAS AL CONTADO", this._exceptionFactory.Exceptions);

            

            var salesDetails = await _repository.Query<domain.Operation.SaleDetail>()
                .Where(f => model.SaleIds.Any(g => g == f.SaleId))
                .Select(f => new
                {
                    f.SaleId,
                    Product = f.Product.Name,
                    f.Product.Tax,
                    f.Total,
                }).ToListAsync();

            var subTotalTax = salesDetails.Where(f => f.Tax).Sum(f =>  f.Total);
            var subTotalExempt = salesDetails.Where(f => !f.Tax).Sum(f => f.Total);
            var subTotal = subTotalTax + subTotalExempt;

            var taxAmount = Math.Round(subTotalTax * 0.15, 2);

            var total = subTotal + taxAmount;

            var invoice = new SaleInvoice
            {
                InvoiceDate = model.InvoiceDate.Value,
                PaymentType = domain.Features.Enums.DomainEnums.PaymentType.Cash,
                PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Paid,
                Balance = 0,
                Paid = true,
                PaidAt = Clock.Now,
                PaidUser = _authService.User,
                TaxableAmount18 = 0,
                Discount = 0,
                Tax18Amount = 0,
                DueDate = null,
                CustomerId = customer.Id,
                TaxableAmount = subTotalTax,
                ExemptAmount = subTotalExempt,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                Total = total,
            };

            

            var salesGrouped = salesDetails.GroupBy(f => f.SaleId).ToList();
            var salesIds = salesGrouped.Select(f => f.Key).ToList();
            invoice.Details = GenerateDetails(salesIds, _authService);

            var correlative = await _generateCorrelativeCommand.Execute();

            invoice.CorrelativeId = correlative.Id;
            invoice.InvoiceNumber = correlative.Reference;

            _repository.InsertAsync(invoice);

            await _repository.CommitAsync();

            foreach (var item in salesIds)
            {
                await _repository.UpdateAsync<domain.Operation.Sale>(
                type => type.Id == item,
                type => new domain.Operation.Sale
                {
                    SaleInvoiceId = invoice.Id,
                });
            }

            await _repository.CommitAsync();
        }

        public async Task GenerateCreditInvoice(GeneratePendingBillingModel model, IGenerateCorrelativeCommand _generateCorrelativeCommand)
        {

            foreach(var item in model.Customers)
            {
                var customer = new domain.Data.Customer();

                if(item.CustomerId > 0)
                {
                    customer = await _repository.Query<domain.Data.Customer>()
                    .Where(f => f.Id == item.CustomerId).FirstOrDefaultAsync();
                } else
                {
                    customer = await _repository.Query<domain.Data.Customer>()
                    .Where(f => f.Name == "CONSUMIDOR FINAL").FirstOrDefaultAsync();
                }

                if (customer == null)
                    throw new ApplicationException("CANCELED", "SE REQUIERE CONFIGURAR CONSUMIDOR FINAL PARA FACTURAS AL CONTADO", this._exceptionFactory.Exceptions);

                var salesDetails = await _repository.Query<domain.Operation.SaleDetail>()
                    .Where(f => item.SaleIds.Any(g => g == f.SaleId))
                    .Select(f => new
                    {
                        f.SaleId,
                        Product = f.Product.Name,
                        f.Product.Tax,
                        f.Total,
                    }).ToListAsync();

                var subTotalTax = salesDetails.Where(f => f.Tax).Sum(f => f.Total);
                var subTotalExempt = salesDetails.Where(f => !f.Tax).Sum(f => f.Total);
                var subTotal = subTotalTax + subTotalExempt;

                var taxAmount = Math.Round(subTotalTax * 0.15, 2);

                var total = subTotal + taxAmount;

                var invoice = new SaleInvoice
                {
                    InvoiceDate = item.InvoiceDate,
                    PaymentType = domain.Features.Enums.DomainEnums.PaymentType.Credit,
                    PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Pending,
                    Balance = total,
                    Paid = false,
                    PaidAt = Clock.Now,
                    PaidUser = _authService.User,
                    TaxableAmount18 = 0,
                    Discount = 0,
                    Tax18Amount = 0,
                    DueDate = Clock.Now.AddDays(30),
                    CustomerId = customer.Id,
                    TaxableAmount = subTotalTax,
                    ExemptAmount = subTotalExempt,
                    SubTotal = subTotal,
                    TaxAmount = taxAmount,
                    Total = total,
                };

                var salesGrouped = salesDetails.GroupBy(f => f.SaleId).ToList();
                var salesIds = salesGrouped.Select(f => f.Key).ToList();
                invoice.Details = GenerateDetails(salesIds, _authService);

                var correlative = await _generateCorrelativeCommand.Execute();

                invoice.CorrelativeId = correlative.Id;
                invoice.InvoiceNumber = correlative.Reference;

                await _repository.UpdateAsync<domain.Data.Correlative>(
                    type => type.Id == correlative.Id,
                    type => new domain.Data.Correlative
                    {
                        Current = correlative.Current + 1,
                    });

                _repository.InsertAsync(invoice);

                await _repository.CommitAsync();

                foreach (var sale in salesIds)
                {
                    await _repository.UpdateAsync<domain.Operation.Sale>(
                    type => type.Id == sale,
                    type => new domain.Operation.Sale
                    {
                        SaleInvoiceId = invoice.Id,
                    });
                }

                await _repository.CommitAsync();

            }
            
        }

        public static List<SaleInvoiceDetail> GenerateDetails(List<int> saleIds, AuthService authService)
        {
            return saleIds.Select(g => new SaleInvoiceDetail
            {
                SaleId = g,
                IsActive = true,
                CreationTime = Clock.Now,
                CreatorUser = authService.User,
            }).ToList();
        }
    }
}
