using cod.application.Employee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Commands.UpdateEmployee
{
    public interface IUpdateEmployeeCommand
    {
        Task Execute(EmployeeModel model, int id);
    }
}
