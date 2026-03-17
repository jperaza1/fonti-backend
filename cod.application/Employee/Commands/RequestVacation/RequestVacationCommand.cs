using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Employee.Commands.RequestVacation
{
    public class RequestVacationCommand : BaseCommand, IRequestVacationCommand
    {
        public RequestVacationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(RequestVacationModel model)
        {
            var employee = await _repository.QueryActive<domain.Rrhh.Employee>()
                .Where(f => f.Id == model.EmployeeId && f.EmploymentStatus == domain.Features.Enums.DomainEnums.EmploymentStatusType.Active).FirstOrDefaultAsync();

            if(employee == null)
                throw new ApplicationException("El empleado no existe", "EMPLEADO");

            var vacationRequest = new domain.Rrhh.VacationRequested
            {
                EmployeeId = employee.Id,
                Start = model.StartDate,
                End = model.EndDate,
                RequestType = model.RequestType
            };

            switch (model.RequestType)
            {
                case VacationRequestType.Requested:
                    vacationRequest.TotalDays = CalculateVacationDaysOptimized(model.StartDate.Date, model.EndDate.Date, true);
                    break;
                case VacationRequestType.Holiday:
                    vacationRequest.TotalDays = CalculateVacationDaysOptimized(model.StartDate.Date, model.EndDate.Date, true);
                    break;
            }

            employee.TotalVacation -= vacationRequest.TotalDays;
            _repository.InsertAsync(vacationRequest);
            _repository.UpdateSync(employee);

            await _repository.CommitAsync();
        }

        public static int CalculateVacationDaysOptimized(DateTime startDate, DateTime endDate, bool excludeWeekends)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date cannot be earlier than start date.");

            int totalDays = (endDate.Date - startDate.Date).Days + 1;

            if (!excludeWeekends)
                return totalDays;

            int fullWeeks = totalDays / 7;
            int remainingDays = totalDays % 7;

            int workingDays = fullWeeks * 5;

            for (int i = 0; i < remainingDays; i++)
            {
                var day = startDate.AddDays(fullWeeks * 7 + i).DayOfWeek;
                if (day != DayOfWeek.Saturday && day != DayOfWeek.Sunday)
                    workingDays++;
            }

            return workingDays;
        }
    }
}
