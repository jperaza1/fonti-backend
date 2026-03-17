using cod.application.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface IUserAlertDataQuery
    {
        Task<int> GetCount();
        Task<AlertPreviewModel> GetPreview();
    }
}
