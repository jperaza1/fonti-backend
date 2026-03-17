using cod.application.Features.Models;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface IAlertDataLoaderQuery
    {
        Task LoadCounts();
        int GetCount(DomainEnums.UserAlertOption alert);
        bool Loaded { get; set; }
        Task<AlertPreviewModel> GetPreview(DomainEnums.UserAlertOption alert);
    }
}
