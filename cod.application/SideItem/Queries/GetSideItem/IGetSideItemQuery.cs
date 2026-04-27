using cod.application.SideItem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Queries.GetSideItem
{
    public interface IGetSideItemQuery
    {
        Task<SideItemModel> Execute(int Id);
    }
}
