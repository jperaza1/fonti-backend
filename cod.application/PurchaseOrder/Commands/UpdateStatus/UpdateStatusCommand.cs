using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Main.Session;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.UpdateStatus
{
    public class UpdateStatusCommand : BaseCommand, IUpdateStatusCommand
    {
        private readonly AuthService _authService;
        private readonly SessionService _sessionService;
        public UpdateStatusCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService,
            SessionService sessionService) : base(repository, exceptionFactory)
        {
            _sessionService = sessionService;
            _authService = authService;
        }

        public async Task Execute(int id)
        {
            var purchaseOrder = await _repository.QueryActive<domain.Operation.PurchaseOrder>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (purchaseOrder == null) throw new ApplicationException("Cafeteria no existe", "ORDEN COMPRA");

            await ValidateModel(id, purchaseOrder);

            switch (purchaseOrder.OrderStatus) 
            { 
                case domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Created:
                    purchaseOrder.OrderStatus = domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Approved;
                    purchaseOrder.ApprovedAt = DateTime.Now;
                    purchaseOrder.ApprovedUser = _authService.User;
                    break;
                case domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Approved:
                    purchaseOrder.OrderStatus = domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Processing;
                    purchaseOrder.ProcessingAt = DateTime.Now;
                    purchaseOrder.ProcessingUser = _authService.User;
                    break;
                case domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Processing:

                    purchaseOrder.OrderStatus = domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Review;
                    purchaseOrder.ReviewedAt = DateTime.Now;
                    purchaseOrder.ReviewedUser = _authService.User;
                    break;
                case domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Review:
                    purchaseOrder.OrderStatus = domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Completed;
                    purchaseOrder.CompletedAt = DateTime.Now;
                    purchaseOrder.CompletedUser = _authService.User;
                    break;
            }

            _repository.UpdateSync(purchaseOrder);
            await _repository.CommitAsync();
        }

        public async Task ValidateModel(int id, domain.Operation.PurchaseOrder purchaseOrder)
        {
            if(purchaseOrder.OrderStatus == domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Created)
            {
                var vendor = await _repository.Query<domain.Data.Vendor>().Where(f => f.Id == purchaseOrder.VendorId).FirstOrDefaultAsync();
                var profile = await _sessionService.GetUserProfile();

                if (vendor != null && vendor.RequiresAdminApproval && profile.UserType != domain.Features.Enums.DomainEnums.UserType.Administrativo)
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Esta orden de compra solo puede ser aprobada por un administrador.", ExceptionEnums.ExceptionCode.Default));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "ORDEN COMPRA", this._exceptionFactory.Exceptions);

            }
            if (purchaseOrder.OrderStatus == domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Processing)
            {
                var isCompleted = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                    .Where(f => f.PurchaseOrderId == id)
                    .AnyAsync(g => g.DetailStatus != domain.Features.Enums.DomainEnums.DetailStatus.Partially_Received && g.DetailStatus != domain.Features.Enums.DomainEnums.DetailStatus.Pending);

                var Invoices = await _repository.QueryActive<domain.Operation.Invoice>()
                    .Where(f => f.PurchaseOrderId == purchaseOrder.Id)
                    .Select(x => new
                    {
                        x.HasFile
                    }).ToListAsync();

                if (!Invoices.Any())
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Por favor debe de ingresar por lo menos una factura", ExceptionEnums.ExceptionCode.Default));

                var hasFile = Invoices.All(g => g.HasFile);

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "ORDEN COMPRA", this._exceptionFactory.Exceptions);

                if (!hasFile)
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Debe de adjuntar un archivo a la factura para poder continuar", ExceptionEnums.ExceptionCode.Default));

                if (!isCompleted)
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Por favor completar la recepcion de la orden de compra no se pueden dejar productos marcados como parcialmente recibidos", ExceptionEnums.ExceptionCode.Default));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "ORDEN COMPRA", this._exceptionFactory.Exceptions);

                var totalRecived = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                    .Where(f => f.PurchaseOrderId == id)
                    .SumAsync(g => g.TotalRecived);

                totalRecived = Math.Round(totalRecived, 2);

                var invoiceTotal = await _repository.QueryActive<domain.Operation.Invoice>()
                    .Where(f => f.PurchaseOrderId == id)
                    .SumAsync(g => g.Total);

                if(totalRecived != invoiceTotal)
                    throw new ApplicationException("Existe una discrepancia entre el monto recibido y las facturas registradas", "ORDEN COMPRA");
            }

            if (purchaseOrder.OrderStatus == domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Review)
            {
                var isCompleted = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                    .Where(f => f.PurchaseOrderId == id)
                    .AnyAsync(g => g.DetailStatus != domain.Features.Enums.DomainEnums.DetailStatus.Partially_Received && g.DetailStatus != domain.Features.Enums.DomainEnums.DetailStatus.Pending);

                var Invoices = await _repository.QueryActive<domain.Operation.Invoice>()
                    .Where(f => f.PurchaseOrderId == purchaseOrder.Id)
                    .Select(x => new
                    {
                        x.HasFile
                    }).ToListAsync();


                if (!Invoices.Any())
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Por favor debe de ingresar por lo menos una factura", ExceptionEnums.ExceptionCode.Default));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "ORDEN COMPRA", this._exceptionFactory.Exceptions);

                var hasFile = Invoices.All(g => g.HasFile);

                if (!hasFile)
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Debe de adjuntar un archivo a la factura para poder continuar", ExceptionEnums.ExceptionCode.Default));

                if (!isCompleted)
                    this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Por favor completar la recepcion de la orden de compra no se pueden dejar productos marcados como parcialmente recibidos", ExceptionEnums.ExceptionCode.Default));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "ORDEN COMPRA", this._exceptionFactory.Exceptions);

                var totalRecived = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                    .Where(f => f.PurchaseOrderId == id)
                    .SumAsync(g => g.TotalRecived);

                totalRecived = Math.Round(totalRecived, 2);

                var invoiceTotal = await _repository.QueryActive<domain.Operation.Invoice>()
                    .Where(f => f.PurchaseOrderId == id)
                    .SumAsync(g => g.Total);

                if (totalRecived != invoiceTotal)
                    throw new ApplicationException("Existe una discrepancia entre el monto recibido y las facturas registradas", "ORDEN COMPRA");
            }
        }

    }
}
