using cod.application.Common.Extensions;
using cod.application.Employee.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using cod.domain.Rrhh;
using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : BaseCommand, ICreateEmployeeCommand
    {
        private readonly IConfigurationService _configurationService;
        private readonly AuthService authService;
        public CreateEmployeeCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            IConfigurationService configurationService,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _configurationService = configurationService;
            this.authService = authService;
        }

        public async Task Execute(EmployeeModel model)
        {
            await this.ValidateModel(model);

            var daysToContract = await _configurationService.Get<int>("DAYS_EMPLOYEE_TEMPORAL");

            var newEmployee = new domain.Rrhh.Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Identification = model.Identification,
                Gender = model.Gender,
                BirthDay = model.BirthDay,
                BirthPlace = model.BirthPlace,
                Profession = model.Profession,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmergencyContactAddress = model.EmergencyContactAddress,
                EmergencyContactName = model.EmergencyContactName,
                EmergencyContactPhone = model.EmergencyContactPhone,
                EmergencyContactRelationship = model.EmergencyContactRelationship,
                EmploymentStatus = model.EmploymentStatus,
                EmploymentType = model.EmploymentType,
                PositionId = model.PositionId,
                UniformSizeId = model.UniformSizeId,
                CafeteriaId = model.CafeteriaId,
                Shift = model.Shift,
                Salary = model.Salary,
                PayFrequency = model.PayFrequency,
                StartDate = model.StartDate,
                EndDate = model.EmploymentType == EmploymentType.Temporary ? model.StartDate.Value.AddDays(daysToContract) : null,
                TrialPeriodDate = model.StartDate.Value.AddDays(60),
                PermanentStartDate = model.EmploymentType == EmploymentType.FullTime ? model.StartDate : null,
                TerminationDate = model.TerminationDate,
            };

            if (model.EmploymentType == EmploymentType.FullTime)
            {
                newEmployee.VacationPeriods = await CalculateVacations(model);
                newEmployee.TotalVacation = newEmployee.VacationPeriods.Sum(f => f.EntitledDays);
            }

            _repository.InsertAsync(newEmployee);

            await _repository.CommitAsync();
        }

        public async Task<List<VacationPeriod>> CalculateVacations(EmployeeModel model)
        {
            var totalYear = ((Clock.Now.Date - model.StartDate.Value.Date).TotalDays / 365);
            var fullYears = (int)Math.Floor(totalYear);
            
            List<VacationPeriod> lst = new();

            if (fullYears > 0)
            {
                var policiesVacations = await _repository.QueryActive<domain.Data.VacationPolicy>()
                .ToListAsync();

                int i = 1;
                int totalVacation = 0;
                DateTime startDate = model.StartDate.Value;
                while (i <= fullYears)
                {
                    var finalDate = startDate.AddYears(1);
                    var policie = policiesVacations.FirstOrDefault(x => x.ToYear == i || x.ToYear == 0);
                    totalVacation += policie == null ? 0 : policie.DayPerYear;
                    lst.Add(new VacationPeriod
                    {
                        PeriodStart = startDate,
                        PeriodEnd = finalDate,
                        ServiceYear = i,
                        EntitledDays = policie.DayPerYear,
                        CreatorUser = authService.User,
                        CreationTime = startDate
                    });
                    startDate = finalDate;
                    i++;
                }
                return lst;
            }

            return lst;
        }

        public async Task ValidateModel(EmployeeModel model)
        {
            if(string.IsNullOrEmpty(model.FirstName))
                this._exceptionFactory.Add("firstName", this._exceptionFactory.GetMessage("Nombres", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.LastName))
                this._exceptionFactory.Add("lastName", this._exceptionFactory.GetMessage("Apellido(s)", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.Identification))
                this._exceptionFactory.Add("identification", this._exceptionFactory.GetMessage("Identificación", ExceptionEnums.ExceptionCode.Required));

            if (!model.Gender.IsValid())
                this._exceptionFactory.Add("gender", this._exceptionFactory.GetMessage("Genero", ExceptionEnums.ExceptionCode.Required));

            if (model.BirthDay == DateTime.MinValue)
                this._exceptionFactory.Add("birthDay", this._exceptionFactory.GetMessage("Fecha de nacimiento", ExceptionEnums.ExceptionCode.Required));

            if(string.IsNullOrEmpty(model.BirthPlace))
                this._exceptionFactory.Add("birthPlace", this._exceptionFactory.GetMessage("Lugar de nacimiento", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.Profession))
                this._exceptionFactory.Add("profession", this._exceptionFactory.GetMessage("Profesión", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.PhoneNumber))
                this._exceptionFactory.Add("phoneNumber", this._exceptionFactory.GetMessage("Teléfono", ExceptionEnums.ExceptionCode.Required));
            
            if(string.IsNullOrEmpty(model.Address))
                this._exceptionFactory.Add("address", this._exceptionFactory.GetMessage("Dirección", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.EmergencyContactName))
                this._exceptionFactory.Add("emergencyContactName", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.EmergencyContactPhone))
                this._exceptionFactory.Add("emergencyContactPhone", this._exceptionFactory.GetMessage("Teléfono", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.EmergencyContactAddress))
                this._exceptionFactory.Add("emergencyContactAddress", this._exceptionFactory.GetMessage("Dirección", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.EmergencyContactRelationship))
                this._exceptionFactory.Add("emergencyContactRelationship", this._exceptionFactory.GetMessage("Parentesco", ExceptionEnums.ExceptionCode.Required));

            if(!model.EmploymentStatus.IsValid())
                this._exceptionFactory.Add("employmentStatus", this._exceptionFactory.GetMessage("Estado", ExceptionEnums.ExceptionCode.Required));

            if (!model.EmploymentType.IsValid())
                this._exceptionFactory.Add("employmentType", this._exceptionFactory.GetMessage("Tipo de Contrato", ExceptionEnums.ExceptionCode.Required));

            if (model.PositionId <= 0)
                this._exceptionFactory.Add("positionId", this._exceptionFactory.GetMessage("Posición", ExceptionEnums.ExceptionCode.Required));

            if (model.UniformSizeId <= 0)
                this._exceptionFactory.Add("uniformSizeId", this._exceptionFactory.GetMessage("Talla de uniforme", ExceptionEnums.ExceptionCode.Required));

            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if(!model.Shift.IsValid())
                this._exceptionFactory.Add("shift", this._exceptionFactory.GetMessage("Turno", ExceptionEnums.ExceptionCode.Required));

            if(model.Salary <= 0)
                this._exceptionFactory.Add("salary", this._exceptionFactory.GetMessage("Salario", ExceptionEnums.ExceptionCode.Required));
            
            if(!model.PayFrequency.IsValid())
                this._exceptionFactory.Add("payFrequency", this._exceptionFactory.GetMessage("Tipo de pago", ExceptionEnums.ExceptionCode.Required));

            if (!model.StartDate.HasValue)
                this._exceptionFactory.Add("startDate", this._exceptionFactory.GetMessage("Fecha de inicio", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);

            if (model.EmploymentStatus == EmploymentStatusType.Terminated || model.EmploymentStatus == EmploymentStatusType.Resigned && !model.TerminationDate.HasValue)
                this._exceptionFactory.Add("terminationDate", this._exceptionFactory.GetMessage("Fecha de renuncia / despido", ExceptionEnums.ExceptionCode.Required));

            if(!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);

            var existIdentification = await _repository.AnyAsync<domain.Rrhh.Employee>(f => f.Identification == model.Identification);


            if(existIdentification)
                this._exceptionFactory.Add("identification", this._exceptionFactory.GetMessage("Identificación", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
