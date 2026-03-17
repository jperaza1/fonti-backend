using cod.application.Common.Extensions;
using cod.application.Employee.Commands.RequestVacation;
using cod.application.Employee.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Operation;
using cod.domain.Rrhh;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Employee.Queries.GetEmployee
{
    public class GetEmployeeQuery : BaseQuery, IGetEmployeeQuery
    {
        public GetEmployeeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<EmployeeModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Rrhh.Employee>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.FirstName,
                    f.LastName,
                    f.Identification,
                    f.Gender,
                    f.BirthDay,
                    f.BirthPlace,
                    f.Profession,
                    f.PhoneNumber,
                    f.Address,
                    f.EmergencyContactAddress,
                    f.EmergencyContactName,
                    f.EmergencyContactPhone,
                    f.EmergencyContactRelationship,
                    f.EmploymentStatus,
                    f.EmploymentType,
                    f.PositionId,
                    f.UniformSizeId,
                    f.CafeteriaId,
                    f.Shift,
                    f.Salary,
                    f.PayFrequency,
                    f.StartDate,
                    f.EndDate,
                    f.PermanentStartDate,
                    f.TerminationDate,
                    f.TotalVacation,
                    VacationPeriod = f.VacationPeriods.Select(g => new
                    {
                        g.Id,
                        g.PeriodEnd,
                        g.PeriodStart,
                        g.ServiceYear,
                        g.EntitledDays
                    }).ToList(),
                    VacationRequested = f.VacationRequested.Select(g => new
                    {
                        g.Id,
                        g.EmployeeId,
                        g.Start,
                        g.End,
                        g.TotalDays,
                        g.RequestType
                    }).ToList()
                });

            var employee = await baseQuery.FirstOrDefaultAsync();

            if (employee == null)
                throw new ApplicationException("Empleado no existe", "EMPLEADO");

            return new EmployeeModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Identification = employee.Identification,
                Gender = employee.Gender,
                BirthDay = employee.BirthDay,
                BirthPlace = employee.BirthPlace,
                Profession = employee.Profession,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                EmergencyContactAddress = employee.EmergencyContactAddress,
                EmergencyContactName = employee.EmergencyContactName,
                EmergencyContactPhone = employee.EmergencyContactPhone,
                EmergencyContactRelationship = employee.EmergencyContactRelationship,
                EmploymentStatus = employee.EmploymentStatus,
                EmploymentType = employee.EmploymentType,
                PositionId = employee.PositionId,
                UniformSizeId = employee.UniformSizeId,
                CafeteriaId = employee.CafeteriaId,
                Shift = employee.Shift,
                Salary = employee.Salary,
                PayFrequency = employee.PayFrequency,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                PermanentStartDate = employee.PermanentStartDate,
                TerminationDate = employee.TerminationDate,
                VacationPeriod = employee.VacationPeriod.Select(g => new VacationPeriodModel
                {
                    EntitledDays = g.EntitledDays,
                    PeriodEnd = g.PeriodEnd,
                    PeriodStart = g.PeriodStart,
                    ServiceYear = g.ServiceYear
                }).ToList(),
                VacationRequested = employee.VacationRequested.Select(g => new RequestVacationModel
                {
                    Id = g.Id,
                    EmployeeId = g.EmployeeId,
                    StartDate = g.Start,
                    EndDate = g.End,
                    RequestType = g.RequestType,
                    RequestTypeName = g.RequestType.GetEnumDescription(),
                    TotalDays = g.TotalDays
                }).ToList(),
                TotalVacation = employee.TotalVacation
            };
        }
    }
}
