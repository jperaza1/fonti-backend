using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetReportExpense
{
    public interface IGetReportExpenseQuery
    {
        Task<List<ReportExpenseModel>> Execute(ReportExpenseFilterModel model);
    }
}
