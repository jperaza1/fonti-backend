using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface IZonitelService
    {
        Task SendAsync(string message, string contact);
    }
}
