using cod.application.OfficeRequisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Commands.UpdateOfficeRequisition
{
    public interface IUpdateOfficeRequisitionCommand
    {
        Task Execute(OfficeRequisitionModel model, int id);
    }
}
