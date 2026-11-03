using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.InvoiceSale.Common;
using DocuSign.eSign.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetInvoiceSaleById
{
    public class GetInvoiceSaleByIdQuery : BaseQuery, IGetInvoiceSaleByIdQuery
    {
        public GetInvoiceSaleByIdQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<SaleInvoiceModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Finance.SaleInvoice>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    Customer = f.Customer.Name,
                    f.CustomerId,
                    f.PaymentStatus,
                    f.PaymentType,
                    f.SubTotal,
                    f.Total,
                    f.TaxAmount,
                    f.DueDate,
                    f.Paid,
                    f.PaidAt,
                    f.PaidUser,
                    f.Balance,
                    CustomerName = f.Customer.Name,
                    CustomerRtn = f.Customer.Rtn,
                    f.CreatorUser,
                    f.Tax18Amount,
                    f.TaxableAmount18,
                    f.TaxableAmount,
                    f.ExemptAmount,
                    f.Discount,
                    Details = f.Details.Select(g => new
                    {
                        g.Id,
                        g.SaleInvoiceId,
                        g.SaleId,
                        g.Sale.SaleDate,
                        g.Sale.SaleType,
                        g.Sale.RouteId,
                        g.Sale.Route.Name,
                        g.Sale.Amount,
                    }).ToList()
                });

            var response = await baseQuery.FirstOrDefaultAsync();

            return new SaleInvoiceModel
            {
                Id = response.Id,
                InvoiceDate = response.InvoiceDate,
                InvoiceNumber = response.InvoiceNumber,
                Customer = response.Customer,
                CustomerId = response.CustomerId,
                PaymentStatus = response.PaymentStatus,
                PaymentStatusName = response.PaymentStatus.GetEnumDescription(),
                PaymentType = response.PaymentType,
                PaymentTypeName = response.PaymentType.GetEnumDescription(),
                SubTotal = response.SubTotal,
                Total = response.Total,
                TaxAmount = response.TaxAmount,
                DueDate = response.DueDate,
                Paid = response.Paid,
                PaidAt = response.PaidAt,
                PaidUser = response.PaidUser,
                Balance = response.Balance,
                CreatorUser = response.CreatorUser,
                Tax18Amount = response.Tax18Amount,
                TaxableAmount18 = response.TaxableAmount18,
                TaxableAmount = response.TaxableAmount,
                ExemptAmount = response.ExemptAmount,
                Discount = response.Discount,
                Details = response.Details.Select(f => new SaleInvoiceDetailModel
                {
                    Id = f.Id,
                    Amount = f.Amount,
                    SaleInvoiceId = f.SaleInvoiceId,
                    SaleId = f.SaleId,
                    SaleDate = f.SaleDate,
                    RouteId = f.RouteId,
                    RouteName = f.Name,
                    SaleTypeName = f.SaleType.GetEnumDescription()
                }).ToList(),    
            };
        }
    }
}
