using cod.application.Factory.UserAlertData;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlertsData
{
    public class GetUserAlertsDataQuery: BaseQuery, IGetUserAlertsDataQuery
    {
        private readonly IUserAlertDataFactory _userAlertDataFactory;

        public GetUserAlertsDataQuery(IRepository repository, IUserAlertDataFactory userAlertDataFactory) : base(repository)
        {
            _userAlertDataFactory = userAlertDataFactory;
        }

        public async Task<List<UserAlertDataModel>> Execute(string user)
        {
            var userAlerts = await _repository.QueryActive<cod.domain.Authorization.ApplicationUserAlert>()
                .Where(userAlert => userAlert.ApplicationUser.UserName.ToUpper() == user.ToUpper())
                .Select(userAlert => userAlert.UserAlertOption)
                .ToListAsync();

            var alertData = new List<UserAlertDataModel>();

            foreach (var alert in userAlerts)
            {
                var query = _userAlertDataFactory.GetInstance(alert);

                var row = new UserAlertDataModel()
                {
                    UserAlert = alert
                };

                if (query != null)
                    row.Count = await query.GetCount();

                alertData.Add(row);
            }

            return alertData;
        }
    }
}
