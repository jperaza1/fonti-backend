using System;

namespace cod.application.Expense.Common
{
    public class ExpenseFilterModel
    {
        public int RouteId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
