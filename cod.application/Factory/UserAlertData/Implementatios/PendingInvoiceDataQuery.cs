using cod.application.Features.Models;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using cod.domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class PendingInvoiceDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public PendingInvoiceDataQuery(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> GetCount()
        {
            var result = await _repository.QueryActive<domain.Operation.Invoice>().Where(f =>
                !f.Paid &&
                f.InvoiceType == DomainEnums.InvoiceType.Expense &&
                f.PaymentType == DomainEnums.PaymentType.Credit &&
                f.PaymentStatus != DomainEnums.PaymentStatus.Paid
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var baseQuery = _repository.QueryActive<domain.Operation.Invoice>().Where(f =>
                !f.Paid &&
                f.InvoiceType == DomainEnums.InvoiceType.Expense &&
                f.PaymentType == DomainEnums.PaymentType.Credit &&
                f.PaymentStatus != DomainEnums.PaymentStatus.Paid
            ).Select(f => new
            {
                f.Id,
                f.InvoiceDate,
                f.InvoiceNumber,
                f.PaymentStatus,
                f.PurchaseOrderId,
                PurchaseOrder = f.PurchaseOrderId.HasValue ? f.PurchaseOrder.Reference : string.Empty,
                Vendor = f.VendorId.HasValue ? f.Vendor.Name : string.Empty,
                f.VendorId,
                f.Description,
                Cafeteria = f.Cafeteria.Name,
                f.CafeteriaId,
                f.TaxableAmount,
                f.ExemptAmount,
                f.Discount,
                f.SubTotal,
                f.Total,
                f.TaxAmount,
                f.DueDate,
                f.Balance,
                RemainingDays = f.DueDate.HasValue ? (f.DueDate.Value.Date - Clock.Now.Date).TotalDays : 0,
            }).OrderBy(f => f.InvoiceDate);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "invoiceNumber", Caption = "Número de Factura",  CellTemplate = "invoiceNumberTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "purchaseOrder", Caption = "Orden de Compra",  CellTemplate = "purchaseOrderTemplate" },
                new DxColumnModel{DataField = "vendor", Caption = "Proveedor" },
                new DxColumnModel{DataField = "invoiceDate", Caption = "Fecha de Factura",  CellTemplate = "dateTemplate" },
                new DxColumnModel{DataField = "dueDate", Caption = "Fecha de Vencimiento",  CellTemplate = "dateTemplate" },
                new DxColumnModel{DataField = "remainingDays", Caption = "Días restantes" },
                new DxColumnModel{DataField = "total", Caption = "Total", CellTemplate = "amountTemplate" },
                new DxColumnModel{DataField = "balance", Caption = "Saldo", CellTemplate = "amountTemplate" },
            };

            var result = await baseQuery.ToListAsync();

            return new AlertPreviewModel
            {
                Columns = _dxColums1,
                DataList = result.Cast<object>().ToList()
            };
        }
    }
}
