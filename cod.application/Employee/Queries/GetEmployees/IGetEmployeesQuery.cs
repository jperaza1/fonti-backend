using cod.application.Employee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetEmployees
{
    public interface IGetEmployeesQuery
    {
        Task<List<EmployeeModel>> Execute();
    }
}
