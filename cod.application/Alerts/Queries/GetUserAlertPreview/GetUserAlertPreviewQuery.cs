using cod.application.Factory.UserAlertData;
using cod.application.Features.Models;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlertPreview
{
    public class GetUserAlertPreviewQuery : IGetUserAlertPreviewQuery
    {
        private readonly IUserAlertDataFactory _userAlertDataFactory;

        public GetUserAlertPreviewQuery(IUserAlertDataFactory userAlertDataFactory)
        {
            _userAlertDataFactory = userAlertDataFactory;
        }
        public async Task<AlertPreviewModel> Execute(DomainEnums.UserAlertOption userAlert)
        {
            var instance = _userAlertDataFactory.GetInstance(userAlert, true);
            return await instance.GetPreview();
        }
    }
}
