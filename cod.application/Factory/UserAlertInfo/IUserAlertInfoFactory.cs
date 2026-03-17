using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertInfo
{
    public interface IUserAlertInfoFactory
    {
        List<UserAlertInfoModel> GetInstances(List<DomainEnums.UserAlertOption> alerts);
    }
}
