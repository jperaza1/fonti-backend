using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SideItem.Commands.DisableSideItem
{
    public interface IDisableSideItemCommand
    {
        Task Execute(int id);
    }
}
