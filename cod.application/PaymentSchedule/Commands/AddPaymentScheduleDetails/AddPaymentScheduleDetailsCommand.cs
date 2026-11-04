using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PaymentSchedule.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PaymentSchedule.Commands.AddPaymentScheduleDetails
{
    public class AddPaymentScheduleDetailsCommand : BaseCommand, IAddPaymentScheduleDetailsCommand
    {
        public AddPaymentScheduleDetailsCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task<List<int>> Execute(AddPaymentScheduleDetailsModel model)
        {
            var incoming = Validate(model);

            var schedule = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .Where(f => f.Id == model.PaymentScheduleId)
                .Select(f => new { f.Id, f.Date })
                .FirstOrDefaultAsync();

            if (schedule == null)
                throw new ApplicationException("Programacion no encontrada");

            var hasNewer = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .AnyAsync(f => f.Date > schedule.Date || (f.Date == schedule.Date && f.Id > schedule.Id));

            if (hasNewer)
                throw new ApplicationException("Solo se pueden agregar lineas a la ultima programacion");

            var existingVendors = await _repository.QueryActive<domain.Finance.PaymentScheduleDetail>()
                .Where(f => f.PaymentScheduleId == schedule.Id && f.Vendor != null)
                .Select(f => f.Vendor)
                .ToListAsync();

            var known = new HashSet<string>(
                existingVendors.Where(f => !string.IsNullOrWhiteSpace(f)).Select(f => f.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var created = new List<domain.Finance.PaymentScheduleDetail>();
            foreach (var item in incoming)
            {
                var vendor = item.Vendor.Trim();
                if (known.Contains(vendor))
                    this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("El proveedor ya esta en la programacion", ExceptionEnums.ExceptionCode.Default));

                known.Add(vendor);
                created.Add(new domain.Finance.PaymentScheduleDetail
                {
                    PaymentScheduleId = schedule.Id,
                    Vendor = vendor,
                    Descripcion = string.IsNullOrWhiteSpace(item.Descripcion) ? null : item.Descripcion.Trim(),
                    AccumulatedAmount = 0,
                    ProjectedAmount = Math.Round(item.ProjectedAmount, 2),
                    RealAmount = 0,
                    Status = PaymentScheduleStatus.Pending
                });
            }

            if (!this._exceptionFactory.IsValid())
            {
                var message = this._exceptionFactory.Exceptions.SelectMany(f => f.Value).FirstOrDefault() ?? "Revise las lineas de la programación";
                throw new ApplicationException(message, "PROGRAMACION DE PAGO", this._exceptionFactory.Exceptions);
            }

            foreach (var detail in created)
                _repository.InsertAsync(detail);

            await _repository.CommitAsync();

            return created.Select(f => f.Id).ToList();
        }

        private List<PaymentScheduleDetailModel> Validate(AddPaymentScheduleDetailsModel model)
        {
            var incoming = model?.Details ?? new List<PaymentScheduleDetailModel>();

            if (model == null || model.PaymentScheduleId <= 0)
                this._exceptionFactory.Add("paymentScheduleId", this._exceptionFactory.GetMessage("Programacion", ExceptionEnums.ExceptionCode.Required));

            if (!incoming.Any())
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Debe agregar al menos un proveedor", ExceptionEnums.ExceptionCode.Default));

            var duplicated = incoming
                .Where(f => !string.IsNullOrWhiteSpace(f.Vendor))
                .GroupBy(f => f.Vendor.Trim(), StringComparer.OrdinalIgnoreCase)
                .Any(f => f.Count() > 1);

            if (duplicated)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Hay proveedores repetidos", ExceptionEnums.ExceptionCode.Default));

            if (incoming.Any(f => string.IsNullOrWhiteSpace(f.Vendor)))
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (incoming.Any(f => f.ProjectedAmount <= 0))
                this._exceptionFactory.Add("projectedAmount", this._exceptionFactory.GetMessage("El proyectado debe ser mayor a cero", ExceptionEnums.ExceptionCode.Default));

            if (!this._exceptionFactory.IsValid())
            {
                var message = this._exceptionFactory.Exceptions.SelectMany(f => f.Value).FirstOrDefault() ?? "Revise las lineas de la programación";
                throw new ApplicationException(message, "PROGRAMACION DE PAGO", this._exceptionFactory.Exceptions);
            }

            return incoming;
        }
    }
}
