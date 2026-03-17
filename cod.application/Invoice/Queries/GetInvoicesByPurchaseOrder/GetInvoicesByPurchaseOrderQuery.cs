using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.application.PurchaseOrder.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoicesByPurchaseOrder
{
    public class GetInvoicesByPurchaseOrderQuery : BaseQuery, IGetInvoicesByPurchaseOrderQuery
    {
        public GetInvoicesByPurchaseOrderQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<InvoiceModel>> Execute(int purchaseOrderId)
        {
            var baseQuery = _repository.Query<domain.Operation.Invoice>()
                .Where(f => f.PurchaseOrderId == purchaseOrderId);

            var result = await baseQuery.Select(f => new InvoiceModel
            {
                PurchaseOrderId = purchaseOrderId,
                Paid = f.Paid,
                Description = f.Description,
                TaxableAmount = f.TaxableAmount,
                TaxAmount = f.TaxAmount,
                Total = f.Total,
                DueDate = f.DueDate,
                PaidAt = f.PaidAt,
                PaidUser = f.PaidUser,
                ExemptAmount = f.ExemptAmount,
                Discount = f.Discount,
                InvoiceDate = f.InvoiceDate,
                InvoiceNumber = f.InvoiceNumber,
                PaymentType = f.PaymentType,
                PaymentTypeName = f.PaymentType.GetEnumDescription(),
                SubTotal = f.SubTotal,
                Id = f.Id,
                PurchaseOrder = f.PurchaseOrder.Reference,
                VendorName = f.PurchaseOrder.Vendor.Name,
            }).ToListAsync();

            return result;
        }
    }
}
