using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            return await _repository.Query<domain.Operation.ExpenseInvoice>()
                .Where(f => f.PurchaseOrderId == purchaseOrderId)
                .Select(f => new InvoiceModel
                {
                    Id = f.Id,
                    PurchaseOrderId = f.PurchaseOrderId,
                    PurchaseOrder = f.PurchaseOrder.Reference,
                    VendorId = f.PurchaseOrder.VendorId,
                    VendorName = f.PurchaseOrder.Vendor.Name,
                    InvoiceNumber = f.InvoiceNumber,
                    Description = f.Description,
                    InvoiceDate = f.InvoiceDate,
                    TaxableAmount = f.TaxableAmount,
                    ExemptAmount = f.ExemptAmount,
                    Discount = f.Discount,
                    SubTotal = f.SubTotal,
                    TaxAmount = f.TaxAmount,
                    Total = f.Total,
                    Balance = f.Balance,
                    DueDate = f.DueDate,
                    Paid = f.Paid,
                    PaidAt = f.PaidAt,
                    PaidUser = f.PaidUser,
                    PaymentType = f.PaymentType,
                    PaymentTypeName = f.PaymentType.GetEnumDescription(),
                    PaymentStatus = f.PaymentStatus,
                    PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                    DocumentId = f.DocumentId,
                    HasFile = f.DocumentId.HasValue && f.DocumentId > 0,
                    FileUploadedAt = f.FileUploadedAt,
                    FileUploadedUser = f.FileUploadedUser
                }).ToListAsync();
        }
    }
}
