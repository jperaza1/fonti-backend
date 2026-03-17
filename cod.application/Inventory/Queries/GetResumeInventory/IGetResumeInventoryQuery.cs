using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetResumeInventory
{
    public interface IGetResumeInventoryQuery
    {
        Task<List<ResumeInventoryModel>> Execute(int id);
    }
}
