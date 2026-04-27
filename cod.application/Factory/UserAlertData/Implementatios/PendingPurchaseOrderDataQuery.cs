using cod.application.Features.Base;
using cod.application.Features.Models;
using cod.application.Interfaces;
using cod.application.Main.Session;
using cod.domain.Features.Enums;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class PendingPurchaseOrderDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;
        private readonly ISessionService _sessionService;
        public PendingPurchaseOrderDataQuery(IRepository repository, ISessionService sessionService)
        {
            _repository = repository;
            _sessionService = sessionService;
        }

        public async Task<int> GetCount()
        {
            var userProfile = await _sessionService.GetUserProfile();

            var result = await _repository.QueryActive<domain.Operation.PurchaseOrder>().Where(f =>
                f.OrderStatus == DomainEnums.PurchaseOrderStatus.Created &&
                f.CafeteriaId == userProfile.CafeteriaId.Value
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var userProfile = await _sessionService.GetUserProfile();

            var baseQuery = _repository.QueryActive<domain.Operation.PurchaseOrder>().Where(f =>
                f.OrderStatus == DomainEnums.PurchaseOrderStatus.Created &&
                f.CafeteriaId == userProfile.CafeteriaId.Value
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
            }).OrderBy(f => f.Date);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "reference", Caption = "Correlativo",  CellTemplate = "purchaseOrderTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "vendor", Caption = "Proveedor" },
                new DxColumnModel{DataField = "date", Caption = "Fecha Creacion",  CellTemplate = "dateTemplate" },
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
