using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertInfo
{
    public class UserAlertInfoModel
    {
        public string Color { get; set; }

        public string TextColor { get; set; }

        public string Label { get; set; }

        public string Icon { get; set; }
        public DomainEnums.UserAlertOption UserAlert { get; set; }
    }
}
