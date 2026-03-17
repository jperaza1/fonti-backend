using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetReportExpense
{
    public class ReportExpenseFilterModel
    {
        public int CafeteriaId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public FilterType FilterType { get; set; }
    }

    public class ReportExpenseModel
    {
        public string Vendor { get; set; }
        public double Total { get; set; }
        public Dictionary<string, double> DateMoves { get; set; }
    }

    public enum FilterType
    {
        Weekly = 1,
        Monthly = 2
    }
}
