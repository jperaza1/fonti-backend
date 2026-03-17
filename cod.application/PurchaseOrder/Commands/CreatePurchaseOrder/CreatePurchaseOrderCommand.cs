using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommand : BaseCommand, ICreatePurchaseOrderCommand
    {
        private readonly AuthService _authService;
        public CreatePurchaseOrderCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task<int> Execute(PurchaseOrderModel model)
        {
            this.ValidateModel(model);

            var cafeteria = await _repository.QueryActive<domain.Data.Cafeteria>().Where(f => f.Id == model.CafeteriaId).FirstOrDefaultAsync();

            if (cafeteria == null) throw new ApplicationException("Cafeteria no existe", "ORDEN COMPRA");

            var reference = $"{cafeteria.Code}-CO-{cafeteria.PurchaseOrderCorelative.ToString("D5")}";
            cafeteria.PurchaseOrderCorelative++;

            var purchaseOrder = new domain.Operation.PurchaseOrder
            {
                VendorId = model.VendorId,
                CafeteriaId = model.CafeteriaId,
                Date = model.Date,
                Reference = reference,
                Total = model.Details.Sum(f => f.Total),
                OrderStatus = DomainEnums.PurchaseOrderStatus.Created,
                TypeInoice = model.TypeInoice,
                PurchaseOrderDetails = GenerateDetails(model)
            };

            _repository.InsertAsync(purchaseOrder);
            _repository.UpdateSync(cafeteria);
            await _repository.CommitAsync();

            return purchaseOrder.Id;
        }

        public List<domain.Operation.PurchaseOrderDetail> GenerateDetails(PurchaseOrderModel model) 
        {
            var lst = new List<domain.Operation.PurchaseOrderDetail>();
            foreach(var item in model.Details)
            {
                var detail = new domain.Operation.PurchaseOrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SubTotal = Math.Round(item.SubTotal, 2),
                    Tax = Math.Round(item.Tax, 2),
                    TaxPercentage = item.TaxPercentage,
                    Discount = Math.Round(item.Discount, 2),
                    DiscountPercentage = item.DiscountPercentage,
                    Cost = item.Cost,
                    Total = Math.Round(item.Total, 2),
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                    DetailStatus = DomainEnums.DetailStatus.Received,
                    QuantityRecived = item.Quantity,
                    CostRecived = item.Cost,
                    TotalRecived = Math.Round(item.Total, 2),
                    SubTotalRecived = Math.Round(item.SubTotal, 2),
                    TaxRecived = Math.Round(item.Tax, 2),
                    DiscountRecived = Math.Round(item.Discount, 2),
                };
                lst.Add(detail);
            }
            return lst;
        }


        public void ValidateModel(PurchaseOrderModel model)
        {
            if (model.VendorId <= 0)
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if(string.IsNullOrEmpty(model.TypeInoice))
                this._exceptionFactory.Add("typeInoice", this._exceptionFactory.GetMessage("Tipo de Factura", ExceptionEnums.ExceptionCode.Required));

            if (model.Date == DateTime.MinValue)
                this._exceptionFactory.Add("date", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if(model.Details.Count == 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Producto Requerido", ExceptionEnums.ExceptionCode.Required));

            if(model.OrderStatus.IsValid())

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
