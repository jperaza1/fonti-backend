using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Commands.RequestVacation
{
    public interface IRequestVacationCommand
    {
        Task Execute(RequestVacationModel model);
    }
}
