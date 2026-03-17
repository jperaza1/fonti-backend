using cod.application.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface ITwilioService
    {
        Task SendAsync(TwilioServiceModel model);
    }
}
