using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Commands.UpdateStatus
{
    public interface IUpdateStatusOfficeRequisitionCommand
    {
        Task Execute(int id);
    }
}
