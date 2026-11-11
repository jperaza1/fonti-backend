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
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PriceApproval> PriceApprovals { get; set; }
        public DbSet<PriceApprovalDetail> PriceApprovalDetails { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseInvoice> ExpenseInvoices { get; set; }
        public DbSet<DispatchControl> DispatchControl { get; set; }
        public DbSet<DispatchControlDetail> DispatchControlDetail { get; set; }
        public DbSet<DispatchTransfer> DispatchTransfers { get; set; }
        public DbSet<ProductMovement> ProductMovements { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<InventoryKardex> InventoryKardex { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionShift> ProductionShifts { get; set; }
    }
}
