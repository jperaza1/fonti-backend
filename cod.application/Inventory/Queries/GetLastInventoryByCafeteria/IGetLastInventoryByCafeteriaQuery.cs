using cod.application.Cafeteria.Commands;
using cod.application.Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetLastInventoryByCafeteria
{
    public interface IGetLastInventoryByCafeteriaQuery
    {
        Task<InventoryModel> Execute(int cafeteriaId);
    }
}
