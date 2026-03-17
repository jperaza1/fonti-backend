using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetResumeInventory
{
    public class ResumeInventoryModel
    {
        public int InventoryId { get; set; }
        public string Vendor { get; set; }
        public double Total { get; set; }
    }
}
