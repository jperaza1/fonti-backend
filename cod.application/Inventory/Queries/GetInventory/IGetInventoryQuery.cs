using cod.application.Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetInventory
{
    public interface IGetInventoryQuery
    {
        Task<InventoryModel> Execute(int id);
    }
}
