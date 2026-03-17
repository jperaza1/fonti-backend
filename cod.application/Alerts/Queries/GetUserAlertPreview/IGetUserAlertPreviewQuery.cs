using cod.application.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Alerts.Queries.GetUserAlertPreview
{
    public interface IGetUserAlertPreviewQuery
    {
        Task<AlertPreviewModel> Execute(UserAlertOption userAlert);
    }
}
