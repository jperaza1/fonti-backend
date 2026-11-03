using cod.application.Invoice.Commands.UpdateDocumentId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Expense.Commands.UpdateExpenseDocumentId
{
    public interface IUpdateExpenseDocumentIdCommand
    {
        Task Execute(UpdateDocumentIdModel model);
    }
}
