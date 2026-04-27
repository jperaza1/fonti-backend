using cod.domain.Data;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseInvoice> ExpensesInvoice { get; set; }
    }
}
