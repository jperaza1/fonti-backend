using cod.application.Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Commands.UpdateInventory
{
    public interface IUpdateInventoryCommand
    {
        Task Execute(int id, InventoryModel model);
    }
}
