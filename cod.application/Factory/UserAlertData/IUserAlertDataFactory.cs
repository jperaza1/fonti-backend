using cod.application.Interfaces;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData
{
    public interface IUserAlertDataFactory
    {
        IUserAlertDataQuery GetInstance(DomainEnums.UserAlertOption alert, bool preview = false);
    }
}
