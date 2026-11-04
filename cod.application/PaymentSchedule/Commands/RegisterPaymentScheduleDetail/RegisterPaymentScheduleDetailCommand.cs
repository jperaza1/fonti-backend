using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PaymentSchedule.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PaymentSchedule.Commands.RegisterPaymentScheduleDetail
{
    public class RegisterPaymentScheduleDetailCommand : BaseCommand, IRegisterPaymentScheduleDetailCommand
    {
        private readonly AuthService _authService;

        public RegisterPaymentScheduleDetailCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(RegisterPaymentScheduleDetailModel model)
        {
            var detail = await _repository.QueryActive<domain.Finance.PaymentScheduleDetail>()
                .FirstOrDefaultAsync(f => f.Id == model.DetailId);

            if (detail == null)
                throw new ApplicationException("Linea de programacion no encontrada");

            var schedule = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .Where(f => f.Id == detail.PaymentScheduleId)
                .Select(f => new { f.Id, f.Date })
                .FirstOrDefaultAsync();

            if (schedule == null)
                throw new ApplicationException("Programacion no encontrada");

            var hasNewer = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .AnyAsync(f => f.Date > schedule.Date || (f.Date == schedule.Date && f.Id > schedule.Id));

            if (hasNewer)
                throw new ApplicationException("Solo se puede registrar el pago en la ultima programacion");

            var realAmount = Math.Round(model.RealAmount, 2);
            var due = PaymentScheduleCalculator.Due(detail.AccumulatedAmount, detail.ProjectedAmount);

            if (realAmount < 0)
                this._exceptionFactory.Add("realAmount", this._exceptionFactory.GetMessage("El monto real no puede ser negativo", ExceptionEnums.ExceptionCode.Default));

            if (realAmount > due)
                this._exceptionFactory.Add("realAmount", this._exceptionFactory.GetMessage("El monto real no puede ser mayor al monto a pagar. Puede registrar un pago parcial", ExceptionEnums.ExceptionCode.Default));

            if (!this._exceptionFactory.IsValid())
            {
                var message = this._exceptionFactory.Exceptions.SelectMany(f => f.Value).FirstOrDefault() ?? "El monto real no es válido";
                throw new ApplicationException(message, "PROGRAMACION DE PAGO", this._exceptionFactory.Exceptions);
            }

            detail.RealAmount = realAmount;
            detail.Status = PaymentScheduleCalculator.Status(detail.AccumulatedAmount, detail.ProjectedAmount, realAmount);
            detail.LastModifiedUser = _authService.User;
            detail.LastModificationTime = Clock.Now;

            _repository.UpdateSync(detail);
            await _repository.CommitAsync();
        }
    }
}
