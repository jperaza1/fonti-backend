using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.UpdateDocumentId
{
    public interface IUpdateDocumentIdCommand
    {
        Task Execute(UpdateDocumentIdModel model);
    }
}
