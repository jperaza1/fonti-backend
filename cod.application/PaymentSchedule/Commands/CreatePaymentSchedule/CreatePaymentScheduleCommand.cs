using cod.application.Common.Clases;
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

namespace cod.application.PaymentSchedule.Commands.CreatePaymentSchedule
{
    public class CreatePaymentScheduleCommand : BaseCommand, ICreatePaymentScheduleCommand
    {
        private readonly AuthService _authService;

        public CreatePaymentScheduleCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task<int> Execute(PaymentScheduleModel model)
        {
            ValidateModel(model);

            var timezone = _authService.ClientTimezone;
            model.Date = DateService.GetCLientLocalTime(model.Date, timezone);

            var carried = await GetUnpaidFromLastSchedule();
            var details = BuildDetails(model, carried);

            if (!details.Any())
                throw new ApplicationException("Debe agregar al menos un proveedor");

            var schedule = new domain.Finance.PaymentSchedule
            {
                Date = model.Date,
                Descripcion = model.Descripcion.Trim(),
                Details = details
            };

            _repository.InsertAsync(schedule);
            await _repository.CommitAsync();

            return schedule.Id;
        }

        private void ValidateModel(PaymentScheduleModel model)
        {
            if (model.Date == DateTime.MinValue)
                this._exceptionFactory.Add("date", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrWhiteSpace(model.Descripcion))
                this._exceptionFactory.Add("descripcion", this._exceptionFactory.GetMessage("Descripcion", ExceptionEnums.ExceptionCode.Required));

            var incoming = model.Details ?? new List<PaymentScheduleDetailModel>();
            var duplicated = incoming
                .Where(f => !string.IsNullOrWhiteSpace(f.Vendor))
                .GroupBy(f => f.Vendor.Trim(), StringComparer.OrdinalIgnoreCase)
                .Any(f => f.Count() > 1);

            if (duplicated)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Hay proveedores repetidos", ExceptionEnums.ExceptionCode.Default));

            if (incoming.Any(f => string.IsNullOrWhiteSpace(f.Vendor)))
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (incoming.Any(f => f.ProjectedAmount < 0))
                this._exceptionFactory.Add("projectedAmount", this._exceptionFactory.GetMessage("El proyectado no puede ser negativo", ExceptionEnums.ExceptionCode.Default));

            if (!this._exceptionFactory.IsValid())
            {
                var message = this._exceptionFactory.Exceptions.SelectMany(f => f.Value).FirstOrDefault() ?? "Revise los datos de la programación";
                throw new ApplicationException(message, "PROGRAMACION DE PAGO", this._exceptionFactory.Exceptions);
            }
        }

        private List<domain.Finance.PaymentScheduleDetail> BuildDetails(
            PaymentScheduleModel model,
            Dictionary<string, CarriedLine> carried)
        {
            var lines = new Dictionary<string, domain.Finance.PaymentScheduleDetail>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in carried.Values.Where(f => f.PendingAmount > 0))
            {
                var detail = CreateDetail(item.Vendor, item.PendingAmount, item.PendingAmount, item.Descripcion);
                lines[item.Vendor] = detail;
            }

            foreach (var item in model.Details ?? new List<PaymentScheduleDetailModel>())
            {
                var vendor = item.Vendor.Trim();
                if (lines.TryGetValue(vendor, out var existing))
                {
                    existing.ProjectedAmount = Math.Round(item.ProjectedAmount, 2);
                    existing.Descripcion = string.IsNullOrWhiteSpace(item.Descripcion)
                        ? existing.Descripcion
                        : item.Descripcion.Trim();
                    existing.Status = PaymentScheduleCalculator.Status(existing.AccumulatedAmount, existing.ProjectedAmount, 0);
                    continue;
                }

                if (item.ProjectedAmount <= 0)
                    continue;

                lines[vendor] = CreateDetail(vendor, 0, item.ProjectedAmount, item.Descripcion);
            }

            return lines.Values
                .Where(f => PaymentScheduleCalculator.Due(f.AccumulatedAmount, f.ProjectedAmount) > 0)
                .OrderBy(f => f.Vendor)
                .ToList();
        }

        private domain.Finance.PaymentScheduleDetail CreateDetail(string vendor, double accumulatedAmount, double projectedAmount, string descripcion)
        {
            var detail = new domain.Finance.PaymentScheduleDetail
            {
                Vendor = vendor,
                Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                AccumulatedAmount = Math.Round(accumulatedAmount, 2),
                ProjectedAmount = Math.Round(projectedAmount, 2),
                RealAmount = 0,
                Status = PaymentScheduleStatus.Pending
            };

            _repository.SetAuthValuesToCreate(detail);
            return detail;
        }

        private async Task<Dictionary<string, CarriedLine>> GetUnpaidFromLastSchedule()
        {
            var last = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .OrderByDescending(f => f.Date)
                .ThenByDescending(f => f.Id)
                .Select(f => new
                {
                    Details = f.Details
                        .Where(d => d.IsActive)
                        .Select(d => new
                        {
                            d.Vendor,
                            d.Descripcion,
                            d.AccumulatedAmount,
                            d.ProjectedAmount,
                            d.RealAmount
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            var result = new Dictionary<string, CarriedLine>(StringComparer.OrdinalIgnoreCase);
            if (last == null)
                return result;

            foreach (var detail in last.Details)
            {
                var pending = PaymentScheduleCalculator.Pending(detail.AccumulatedAmount, detail.ProjectedAmount, detail.RealAmount);
                if (pending <= 0 || string.IsNullOrWhiteSpace(detail.Vendor))
                    continue;

                var vendor = detail.Vendor.Trim();
                if (result.TryGetValue(vendor, out var current))
                {
                    current.PendingAmount = Math.Round(current.PendingAmount + pending, 2);
                    if (string.IsNullOrWhiteSpace(current.Descripcion) && !string.IsNullOrWhiteSpace(detail.Descripcion))
                        current.Descripcion = detail.Descripcion.Trim();
                }
                else
                    result[vendor] = new CarriedLine
                    {
                        Vendor = vendor,
                        Descripcion = string.IsNullOrWhiteSpace(detail.Descripcion) ? null : detail.Descripcion.Trim(),
                        PendingAmount = pending
                    };
            }

            return result;
        }

        private class CarriedLine
        {
            public string Vendor { get; set; }
            public string Descripcion { get; set; }
            public double PendingAmount { get; set; }
        }
    }
}
