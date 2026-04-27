using cod.application.ExpenseInvoice.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.application.Expense.Common
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public bool IsActive { get; set; }
        public ExpenseInvoiceModel ExpenseInvoice { get; set; }
    }
}
