using cod.application.Factory.UserAlertInfo;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlerts
{
    public class GetUserAlertsQuery : BaseQuery, IGetUserAlertsQuery
    {
        private readonly IUserAlertInfoFactory _userAlertInfoFactory;
        public GetUserAlertsQuery(IRepository repository, IUserAlertInfoFactory userAlertInfoFactory) : base(repository)
        {
            _userAlertInfoFactory = userAlertInfoFactory;
        }

        public async Task<List<UserAlertInfoModel>> Execute(string user)
        {
            var userAlerts = await _repository.QueryActive<cod.domain.Authorization.ApplicationUserAlert>()
                .Where(userAlert => userAlert.ApplicationUser.UserName.ToUpper() == user.ToUpper())
                .Select(userAlert => userAlert.UserAlertOption)
                .ToListAsync();

            return _userAlertInfoFactory.GetInstances(userAlerts);
        }
    }
}
