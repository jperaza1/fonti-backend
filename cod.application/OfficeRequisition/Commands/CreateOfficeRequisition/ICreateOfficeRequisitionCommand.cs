using cod.application.OfficeRequisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Commands.CreateOfficeRequisition
{
    public interface ICreateOfficeRequisitionCommand
    {
        Task Execute(OfficeRequisitionModel model);
    }
}
