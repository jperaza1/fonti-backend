using cod.application.Inventory.Common;
using cod.application.Invoice.Queries.GetInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetInventories
{
    public interface IGetInventoriesQuery
    {
        Task<List<InventoryModel>> Execute(FilterInventory filter);
    }
}
