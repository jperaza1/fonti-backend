using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Employee.Commands.RequestVacation
{
    public class RequestVacationModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public VacationRequestType RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public int TotalDays { get; set; }
    }
}
