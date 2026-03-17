using cod.application.Alerts.Queries.GetUserAlertPreview;
using cod.application.Alerts.Queries.GetUserAlerts;
using cod.application.Alerts.Queries.GetUserAlertsData;
using cod.application.Factory.UserAlertInfo;
using cod.application.Features.Models;
using cod.domain.Features.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class AlertController : BaseController
    {
        private readonly IGetUserAlertsQuery _getUserAlertsQuery;
        private readonly IGetUserAlertsDataQuery _getUserAlertsDataQuery;
        private readonly IGetUserAlertPreviewQuery _getUserAlertPreviewQuery;

        public AlertController(IGetUserAlertsQuery getUserAlertsQuery, 
            IGetUserAlertsDataQuery getUserAlertsDataQuery,
            IGetUserAlertPreviewQuery getUserAlertPreviewQuery)
        {
            _getUserAlertsQuery = getUserAlertsQuery;
            _getUserAlertsDataQuery = getUserAlertsDataQuery;
            _getUserAlertPreviewQuery = getUserAlertPreviewQuery;
        }

        [HttpGet]
        public async Task<List<UserAlertInfoModel>> GetAlertsInfo()
        {
            var user = User.Identity.Name;
            return await this._getUserAlertsQuery.Execute(user);
        }
        [HttpGet]
        public async Task<List<UserAlertDataModel>> GetAlertsData()
        {
            var user = User.Identity.Name;
            return await this._getUserAlertsDataQuery.Execute(user);
        }

        [HttpGet]
        public async Task<AlertPreviewModel> GetAlertsPreview(DomainEnums.UserAlertOption userAlert)
        {
            return await this._getUserAlertPreviewQuery.Execute(userAlert);
        }
    }
}
