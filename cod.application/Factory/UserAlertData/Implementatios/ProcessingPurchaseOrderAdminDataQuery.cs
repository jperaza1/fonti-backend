using cod.application.Features.Models;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class ProcessingPurchaseOrderAdminDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public ProcessingPurchaseOrderAdminDataQuery(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCount()
        {
            var result = await _repository.QueryActive<domain.Operation.PurchaseOrder>().Where(f =>
                f.OrderStatus == DomainEnums.PurchaseOrderStatus.Processing
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var baseQuery = _repository.QueryActive<domain.Operation.PurchaseOrder>().Where(f =>
                f.OrderStatus == DomainEnums.PurchaseOrderStatus.Processing
            ).Select(f => new
            {
                PurchaseOrderId = f.Id,
                f.VendorId,
                Vendor = f.Vendor.Name,
                f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                f.Reference,
                f.Date,
                f.Total,
                f.CreatorUser,
                HasInvoice = f.Invoices.Any()
            }).OrderBy(f => f.Date);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "reference", Caption = "Correlativo",  CellTemplate = "purchaseOrderTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "vendor", Caption = "Proveedor" },
                new DxColumnModel{DataField = "date", Caption = "Fecha Creacion",  CellTemplate = "dateTemplate" },
                new DxColumnModel{DataField = "hasInvoice", Caption = "Tiene Factura", CellTemplate = "hasInvoiceTemplate" },
                new DxColumnModel{DataField = "total", Caption = "Total", CellTemplate = "amountTemplate" },
                new DxColumnModel{DataField = "creatorUser", Caption = "Usuario Creacion" },
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
