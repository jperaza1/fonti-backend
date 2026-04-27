using cod.application.Dish.Common;
using cod.application.SideItem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Commands.UpdateSideItem
{
    public interface IUpdateSideItemCommand
    {
        Task Execute(SideItemModel model, int id);
    }
}
