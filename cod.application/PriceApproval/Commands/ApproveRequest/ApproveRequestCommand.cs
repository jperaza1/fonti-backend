using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PriceApproval.Common;
using cod.application.PurchaseOrder.Commands.UpdateStatus;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PriceApproval.Commands.ApproveRequest
{
    public class ApproveRequestCommand : BaseCommand, IApproveRequestCommand
    {
        private readonly AuthService _authService;

        public ApproveRequestCommand(IRepository repository, IExceptionFactory exceptionFactory, AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(ApproveRequestModel model)
        {
            var request = await _repository.Query<domain.Operation.PriceApproval>()
                .FirstOrDefaultAsync(item => item.Id == model.Id);

            if (request == null)
                throw new ApplicationException("Solicitud de cambio de precios no existe", "PRECIOS");

            if (request.Status != PriceApprovalStatus.Pending)
                throw new ApplicationException("La solicitud ya fue resuelta", "PRECIOS");

            request.Status = model.Status;

            if (model.Status == PriceApprovalStatus.Approved)
            {
                request.ApprovedAt = Clock.Now;
                request.ApprovedUser = _authService.User;

                var details = await _repository.Query<domain.Operation.PriceApprovalDetail>()
                    .Where(item => item.PriceApprovalId == model.Id)
                    .Select(item => new
                    {
                        item.ProductVendorId,
                        item.NewCost,
                        item.ProductVendor.ProductId,
                        item.ProductVendor.Unit
                    })
                    .ToListAsync();

                foreach (var item in details)
                    await VendorPrice.Apply(_repository, item.ProductVendorId, item.ProductId, item.Unit, item.NewCost, "Solicitud " + request.Token);
            }

            if (model.Status == PriceApprovalStatus.Rejected)
            {
                request.Notes = model.Note;
                request.RejectedAt = Clock.Now;
                request.RejectedUser = _authService.User;
            }

            _repository.UpdateSync(request);
            await _repository.CommitAsync();
        }
    }
}
