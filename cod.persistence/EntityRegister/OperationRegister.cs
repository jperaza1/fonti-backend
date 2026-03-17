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
        public DbSet<SpecialService> SpecialServices { get; set; }
        public DbSet<SpecialServiceDetail> SpecialServiceDetails { get; set; }
        public DbSet<OfficeRequisition> OfficeRequisitions { get; set; }
        public DbSet<OfficeRequisitionDetail> OfficeRequisitionDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryDetail> InventoryDetails { get; set; }
    }
}
