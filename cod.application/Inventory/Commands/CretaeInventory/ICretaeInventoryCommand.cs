using cod.application.Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Commands.CretaeInventory
{
    public interface ICretaeInventoryCommand
    {
        Task<int> Execute(InventoryModel inventory);
    }
}
