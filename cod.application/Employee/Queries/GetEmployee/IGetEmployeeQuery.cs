using cod.application.Employee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetEmployee
{
    public interface IGetEmployeeQuery
    {
        Task<EmployeeModel> Execute(int id);
    }
}
