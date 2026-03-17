using cod.application.SideItem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Queries.GetSideItems
{
    public interface IGetSideItemsQuery
    {
        Task<List<SideItemModel>> Execute();
    }
}
