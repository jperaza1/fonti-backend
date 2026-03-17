using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Commands.EnableSideItem
{
    public interface IEnableSideItemCommand
    {
        Task Execute(int id);
    }
}
