using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetVacationRequestPdf
{
    public class GetVacationRequestModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string Cafeteria { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalDays { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
